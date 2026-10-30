using Pachyderm_Acoustic.UI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pachyderm_Acoustic.Utilities
{
    public partial class RCPachTools
    {
        public static async Task<Simulation_Type> RunSimulation(Simulation_Type Sim, bool needfeedback = true, CancellationToken cancellationToken = default, string context = null)
        {
            string Prefix = string.IsNullOrEmpty(context) ? "" : context + " | ";
            cancellationToken.ThrowIfCancellationRequested();
            if (Sim == null) return null;
            Sim.Cancellation = cancellationToken;
            using (System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess())
            {
                var priority = process.PriorityClass;
                bool autoSave = Rhino.ApplicationSettings.FileSettings.AutoSaveEnabled;
                try
                {
                    switch (PachydermAc_PlugIn.Instance.TaskPriority)
                    {
                        case 0: process.PriorityClass = System.Diagnostics.ProcessPriorityClass.High; break;
                        case 1: process.PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal; break;
                        default: process.PriorityClass = System.Diagnostics.ProcessPriorityClass.Normal; break;
                    }
                    Rhino.ApplicationSettings.FileSettings.AutoSaveEnabled = false;
                    Sim._workClock.Restart();
                    Sim.Begin();
                    Rhino.RhinoApp.SetCommandPrompt(Prefix + string.Format("Initiating {0} Calculation...", Sim.Sim_Type()));
                    // Let all workers observe cancellation and stop before discarding their buffers.
                    // Spec D7: check the state protocol first; completed work pays no delay, still-running
                    // work polls at the existing 3,000 ms interval.
                    while (Sim.ThreadState() != System.Threading.ThreadState.Stopped)
                    {
                        await Task.Delay(3000);
                        if (needfeedback) Rhino.RhinoApp.SetCommandPrompt(Prefix + Sim.ProgressMsg());
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                    Sim.Combine_ThreadLocal_Results();
                    while (Sim.ThreadState() == System.Threading.ThreadState.Running)
                    {
                        await Task.Delay(3000);
                        if (needfeedback) Rhino.RhinoApp.SetCommandPrompt(Prefix + Sim.ProgressMsg());
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                    Sim._workClock.Stop();
                    return Sim;
                }
                finally
                {
                    Rhino.ApplicationSettings.FileSettings.AutoSaveEnabled = autoSave;
                    process.PriorityClass = priority;
                }
            }
        }
    }

    /// <summary>
    /// Per-source stage timing for the Hybrid and Mapping main loops (spec D1).
    /// Monotonic (Stopwatch); prints one summary per source through the existing Rhino log channel.
    /// Stage lines are siblings of each other and children of the end-to-end wall: do not sum them.
    /// "await total" spans construct + simulation work + polling wait, and "wait" is derived as
    /// await total - construct - work, so no interval is counted twice.
    /// </summary>
    internal sealed class Source_Stage_Timing
    {
        private readonly string title;
        private readonly System.Diagnostics.Stopwatch endToEnd = System.Diagnostics.Stopwatch.StartNew();
        private readonly System.Diagnostics.Stopwatch stage = new System.Diagnostics.Stopwatch();
        private readonly System.Diagnostics.Stopwatch segment = new System.Diagnostics.Stopwatch();
        private readonly System.Collections.Generic.List<string> lines = new System.Collections.Generic.List<string>();
        private System.TimeSpan construct;
        private bool segmentStarted;

        public Source_Stage_Timing(string title)
        {
            this.title = title;
        }

        public void Begin_Stage()
        {
            stage.Restart();
        }

        /// <summary>Call immediately after construction, before the simulation starts.</summary>
        public void Construct_Done()
        {
            construct = stage.Elapsed;
        }

        /// <summary>Call when the stage's result is back; work is the simulation's own compute window (RunSimulation).</summary>
        public void End_Stage(string name, Simulation_Type sim)
        {
            stage.Stop();
            System.TimeSpan work = sim != null ? sim._workClock.Elapsed : System.TimeSpan.Zero;
            lines.Add(string.Format("  {0}: construct (tracer/pools) {1:0.###} s; work {2:0.###} s; await total {3:0.###} s (wait {4:0.###} s)",
                name, construct.TotalSeconds, work.TotalSeconds, stage.Elapsed.TotalSeconds, (stage.Elapsed - construct - work).TotalSeconds));
        }

        /// <summary>Starts or resumes a simple span (preparation, filter work, assembly). Spans accumulate between Start and Report.</summary>
        public void Segment_Start()
        {
            segmentStarted = true;
            segment.Start();
        }

        /// <summary>Pauses the current span without reporting it; resume with Segment_Start.</summary>
        public void Segment_Stop()
        {
            segment.Stop();
        }

        /// <summary>Reports the accumulated span; a span that never started is reported as not run, never as zero time.</summary>
        public void Segment_Report(string name)
        {
            segment.Stop();
            if (segmentStarted) lines.Add(string.Format("  {0}: {1:0.###} s", name, segment.Elapsed.TotalSeconds));
            else lines.Add("  " + name + ": not run");
            segment.Reset();
            segmentStarted = false;
        }

        /// <summary>Records an externally measured span (e.g. pre-loop batch preparation attributed to this source).</summary>
        public void Span(string name, System.TimeSpan elapsed)
        {
            lines.Add(string.Format("  {0}: {1:0.###} s", name, elapsed.TotalSeconds));
        }

        /// <summary>Marks a stage disabled for this run; reported as not run rather than a zero-time run.</summary>
        public void Not_Run(string name)
        {
            lines.Add("  " + name + ": not run");
        }

        /// <summary>Prints the per-source summary; end-to-end wall includes the stages listed, so do not add them together.</summary>
        public void Report()
        {
            Rhino.RhinoApp.WriteLine(string.Format("{0} end-to-end wall: {1:0.###} s (includes the stages below; do not sum them)",
                title, endToEnd.Elapsed.TotalSeconds));
            foreach (string line in lines) Rhino.RhinoApp.WriteLine(line);
        }
    }
}
