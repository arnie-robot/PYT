using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

namespace PYT
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please select (a)uto or (m)anual mode");
            string mode = Console.ReadLine().ToLower().Trim();

            List<string> coordinates = new List<string>(new string[] { "x", "y", "z" });

            if (mode == "a" || mode == "auto")
            {
                Console.WriteLine("Welcome to Auto mode");

                Console.WriteLine("Initialising AutomatedCommander");
                AutomatedCommander ac = new AutomatedCommander(coordinates);

                string input = "";
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("Please input a command:");
                    input = Console.ReadLine();
                    switch (input.Trim())
                    {
                        case "go":
                        case "g":
                        case "start":
                            ac.Start();
                            break;
                        case "stop":
                        case "s":
                        case "panic":
                        case "p":
                        case "x":
                            ac.Stop();
                            break;
                        case "help":
                        case "h":
                        case "?":
                            Console.WriteLine("Available commands: (g)o / start, (s)top / (p)anic / x, (h)elp / ?, (q)uit / exit");
                            break;
                        case "exit":
                        case "quit":
                        case "q":
                            exit = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (mode == "m" || mode == "manual")
            {
                Console.WriteLine("Welcome to Manual mode");

                Console.WriteLine("Initialising Input thread");
                InputThread i = new InputThread(PYT.Properties.Settings.Default.IncomingHost, PYT.Properties.Settings.Default.IncomingDataPort);
                Thread iThread = new Thread(new ThreadStart(i.process));
                iThread.Start();
                Console.WriteLine("Waiting for Input thread to start...");
                while (!iThread.IsAlive) ;
                Console.WriteLine("Input thread started");

                string input = "";
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("Please input a command:");
                    input = Console.ReadLine();
                    switch (input.Trim())
                    {
                        case "latest":
                        case "l":
                            Console.WriteLine("Latest message on Input: " + i.getLastReceived());
                            break;
                        case "send":
                        case "s":
                            // collect required information from user
                            Console.WriteLine("Preparing to send...");

                            TrajectoryDispatcher tjd = new TrajectoryDispatcher(coordinates);
                            tjd.collectData();
                            Console.WriteLine("Enter 'execute' and press return to dispatch. Enter anything else to abort");
                            if (Console.ReadLine() != "execute")
                            {
                                Console.WriteLine("Aborting");
                                break;
                            }

                            Console.WriteLine("Collecting last known position...");
                            Coordinate start = Coordinate.fromString(coordinates, i.getLastReceived());
                            Console.WriteLine("... captured");

                            tjd.Execute(start);

                            Console.WriteLine("Done");
                            break;
                        case "help":
                        case "h":
                        case "?":
                            Console.WriteLine("Available commands: (l)atest, (s)end, (h)elp / ?, (q)uit / exit");
                            break;
                        case "exit":
                        case "quit":
                        case "q":
                            exit = true;
                            break;
                        default:
                            break;
                    }
                }

                Console.WriteLine("Closing Input thread");
                iThread.Abort();
                Console.WriteLine("Waiting for Input thread to terminate...");
                iThread.Join();
                Console.WriteLine("Input thread closed");
            }
            else
            {
                Console.WriteLine("Unknown command. Terminating");
            }
        }
    }
}
