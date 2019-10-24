using System;
using System.Diagnostics;
using System.Threading;

namespace DeadCellsStarter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Launching Dead Cells...");

            //Start UWP app
            var p = new Process();
            var startInfo = new ProcessStartInfo {UseShellExecute = true};
            startInfo.FileName = startInfo.FileName = @"shell:appsFolder\MotionTwin.DeadCellsWin10_rtjy889c6zgtg!App";
            p.StartInfo = startInfo;
            p.Start();

            Console.WriteLine("Successfully started DeadCells UWP app! Waiting 20 seconds for the process to start...");

            //Try finding deadcells.exe
            Process deadCellsProcess = null;
            var start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() - start < 20000)
            {
                //Try finding process for 20 seconds
                foreach (var process in Process.GetProcessesByName("deadcells"))
                {
                    Console.WriteLine("Found process: " + process.ProcessName);
                    deadCellsProcess = process;
                    goto found;
                }
            }

            Console.WriteLine("DeadCells didn't start. Exiting in 5 seconds...");
            Thread.Sleep(5000);
            return;

            found:
            Console.WriteLine("Waiting for DeadCells to exit...");
            deadCellsProcess.WaitForExit();
            Console.WriteLine("DeadCells exited. Shutting down...");
        }
    }
}