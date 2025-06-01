using System;
using Eto.Forms;
using Eto.Drawing;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;

namespace Pachyderm_Acoustic
{
    public class ProgressBox : Eto.Forms.FloatingForm, Pach_Graphics.IProgressFeedback
    {
        string msg = "";
        int Prog_Percent = 0;
        private ProgressBar Progress;
        private readonly Timer _updateTimer;

        public ProgressBox(string text)
        {
            DynamicLayout L = new DynamicLayout();
            Progress = new ProgressBar();
            Progress.Width = 300;
            Progress.Height = 35;
            L.AddRow(Progress);
            this.Content = L; 
            this.Title = text;

            _updateTimer = new Timer (1000); // 1 second interval
            _updateTimer.Elapsed += UpdateUI;
            _updateTimer.Start();
        }

        //private async void UpdateUI(object sender, EventArgs e)
        //{
        //    // Update UI elements here
        //    Eto.Forms.Application.Instance.AsyncInvoke(() =>
        //    {
        //        Progress.Value = Prog_Percent;
        //        this.Title = msg;
        //        this.Invalidate();
        //    });
        //}

        private int _lastReportedPercent = -1;
        private string _lastReportedMsg = "";
        private DateTime _lastUpdate = DateTime.MinValue;

        private async void UpdateUI(object sender, EventArgs e)
        {
            // Only update if value changed or enough time has passed
            if (Prog_Percent != _lastReportedPercent ||
                msg != _lastReportedMsg ||
                (DateTime.Now - _lastUpdate).TotalMilliseconds > 250)
            {
                _lastReportedPercent = Prog_Percent;
                _lastReportedMsg = msg;
                _lastUpdate = DateTime.Now;

                Eto.Forms.Application.Instance.AsyncInvoke(() =>
                {
                    Progress.Value = Prog_Percent;
                    this.Title = msg;
                    this.Invalidate();
                });
            }
        }

        /// <summary>
        /// Graphs and displays the contents of an array.
        /// </summary>
        /// <param name="array">The array to display.</param>
        /// <param name="WaitDuration">The amount of time the information is to be left on the screen.</param>
        public void Report(int Prog_Percent)
        {
            this.Prog_Percent = Prog_Percent;
            //Application.Instance.Invoke(() => {
            //    if (this.Visible == false) return;
            //    Progress.Value = Prog_Percent;
            //    this.Progress.Value = Prog_Percent;
            //});
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _updateTimer.Stop();
        }

        public void change_title(string msg) 
        {
            this.msg = msg;
        }
    }
}
