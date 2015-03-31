using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Filewatcher.MDL;

namespace Filewatcher.BLL
{
    public class ProcessInitializer
    {
        private readonly IDispatcherContext _dispatcher;

        public ProcessInitializer(IDispatcherContext dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void CreateProcess(Watch watch, History history)
        {
            var process = new Process();
            process.StartInfo.Arguments = Parameter.Parse(watch.Parameter, history.TargetPath);
            process.StartInfo.FileName = watch.Process;
            process.StartInfo.WorkingDirectory = Parameter.Parse(watch.WorkingFolder, history.TargetPath);
            process.StartInfo.UseShellExecute = watch.UseShellExecute;
            process.StartInfo.RedirectStandardOutput = !watch.UseShellExecute;
            process.StartInfo.RedirectStandardError = !watch.UseShellExecute;
            process.EnableRaisingEvents = true;

            if (watch.IsProcessHidden)
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
           
            process.OutputDataReceived += (sender, e) => _dispatcher.Invoke(() =>
            {
                if (String.IsNullOrEmpty(e.Data))
                    return;
                history.LastOutputText = e.Data;
                history.OutputText.Append(e.Data);
            });
            process.ErrorDataReceived += (sender, e) => _dispatcher.Invoke(() =>
            {
                if (String.IsNullOrEmpty(e.Data))
                    return;
                history.LastErrorText = e.Data;
                history.ErrorText.Append(e.Data);
            });
            process.Exited += (sender, e) => _dispatcher.Invoke(() => {
                history.EndDate = DateTime.Now;
                history.State = process.ExitCode == 0 ? 
                    HistoryState.Success : HistoryState.Error;
                history.ExitCode = process.ExitCode;
            });

            process.Start();
            history.State = HistoryState.Running;
            history.StartDate = DateTime.Now;

            if (watch.UseShellExecute)
                return;

            if (watch.LogOutput && !String.IsNullOrEmpty(watch.OutputPath))
            {
                new Thread(() =>
                {
                    WriteToFile(watch, history, process.StandardOutput.ReadToEnd());
                }).Start();
            }

            else
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }
        }

        private static void WriteToFile(Watch watch, History history, string text)
        {
            try
            {
                using (var streamWriter = new StreamWriter(Parameter.Parse(watch.OutputPath, history.TargetPath)))
                {
                    streamWriter.Write(text);
                    streamWriter.Close();
                }
            }
            catch (Exception e)
            {
                watch.State = WatchState.Error;
            }
        }
    }
}
