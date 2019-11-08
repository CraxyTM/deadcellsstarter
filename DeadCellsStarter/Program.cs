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

            //Not found
            Console.WriteLine("DeadCells didn't start. Exiting in 5 seconds...");
            Thread.Sleep(5000);
            return;

            //Found
            found:
            Console.WriteLine("Found DeadCells!");

            if (!deadCellsProcess.HasExited)
            {
                Console.WriteLine("Waiting for DeadCells to exit...");

                var xboxIdpKillerThread = new Thread(() =>
                {
                    while (true)
                    {
                        foreach (var process in Process.GetProcessesByName("XboxIdp"))
                        {
                            Console.WriteLine("Found XboxIdp: " + process.ProcessName);
                            process.Kill();
                            Console.WriteLine("Killed XboxIdp!");
                        }

                        Thread.Sleep(100);
                    }
                });
                xboxIdpKillerThread.Start();

                deadCellsProcess.WaitForExit();
            }

            Console.WriteLine("DeadCells exited. Shutting down...");
            Environment.Exit(0);
        }
    }
}