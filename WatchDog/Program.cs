﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WatchDog
{
    public class Program
    {
        static void Main(string[] args)
        {
            string path = args.FirstOrDefault();

            if (String.IsNullOrEmpty(path))
            {
                Console.WriteLine("watchdog.exe [path] [args]");
                Environment.Exit(-1);
                return;
            }

            string childProcess = "";
            string childArgs = "";

            if(File.Exists(path))
            {
                childProcess = path;
                childArgs = String.Join(" ", args.Skip(1));
            }
            else
            {
                childProcess = Environment.GetEnvironmentVariable("HOST_PROCESS");
                childArgs = String.Join(" ", new[] { "run" }.Concat(args));
            }

            while (true)
            {
                var tcs = new TaskCompletionSource<object>();

                Console.Write("Starting process '" + Path.GetFileName(childProcess) + "'");
                if (!String.IsNullOrEmpty(childArgs))
                {
                    Console.WriteLine(" with " + childArgs);
                }
                else
                {
                    Console.WriteLine();
                }

                var exe = new Executable(childProcess, Environment.CurrentDirectory, TimeSpan.FromHours(1));
                var process = exe.Execute(s =>
                {
                    Console.WriteLine(s);
                    return false;
                },
                s =>
                {
                    Console.Error.WriteLine(s);
                    return false;
                },
                Encoding.UTF8, childArgs);

                var inputThread = new Thread(() => HandleInput(process, tcs));

                process.Exited += (sender, e) =>
                {
                    Console.WriteLine("Process '{0}' excited with '{1}'", childProcess, process.ExitCode);

                    if (inputThread.IsAlive)
                    {
                        inputThread.Abort();
                    }

                    if (process.ExitCode != 250)
                    {
                        tcs.TrySetException(new Exception(String.Format("Exit code unknown {0}, quitting", process.ExitCode)));
                    }
                    else
                    {
                        tcs.TrySetResult(null);
                    }
                };

                inputThread.Start();

                try
                {
                    tcs.Task.Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message);
                    break;
                }

                Thread.Sleep(100);
            }
        }

        private static void HandleInput(Process process, TaskCompletionSource<object> tcs)
        {
            var ki = Console.ReadKey(false);

            if (ki.Key == ConsoleKey.B)
            {
                tcs.TrySetResult(null);

                process.Kill();
            }
        }
    }
}