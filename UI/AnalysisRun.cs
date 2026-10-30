using System;
using System.Threading;

namespace Pachyderm_Acoustic.UI
{
    internal sealed class AnalysisRun : IDisposable
    {
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        public CancellationToken Token { get; }

        public AnalysisRun()
        {
            Token = cancellation.Token;
            Rhino.RhinoApp.EscapeKeyPressed += Cancel;
        }

        private void Cancel(object sender, EventArgs args)
        {
            if (Token.IsCancellationRequested) return;
            cancellation.Cancel();
            Rhino.RhinoApp.SetCommandPrompt("Cancelling analysis...");
        }

        public void Dispose()
        {
            Rhino.RhinoApp.EscapeKeyPressed -= Cancel;
            cancellation.Dispose();
        }
    }
}
