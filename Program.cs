using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PYT
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initialising Input thread");

            InputThread i = new InputThread(PYT.Properties.Settings.Default.IncomingHost, PYT.Properties.Settings.Default.IncomingPort);
            Thread iThread = new Thread(new ThreadStart(i.process));
            iThread.Start();
            Console.WriteLine("Waiting for Input thread to start...");
            while (!iThread.IsAlive);
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
                        List<string> coordinates = new List<string>(new string[] { "x", "y", "z" });
                        Coordinate end = new Coordinate(coordinates);

                        // load in the coord values
                        foreach (string coord in coordinates)
                        {
                            Console.WriteLine("Please enter the value for coordinate " + coord);
                            end.setCoordinate(coord, double.Parse(Console.ReadLine()));
                        }

                        Console.WriteLine("Enter the number of samples to send");
                        int samples = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter the time between each sample being dispatched (milliseconds)");
                        int period = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter 'execute' and press return to dispatch. Enter anything else to abort");
                        if (Console.ReadLine() != "execute")
                        {
                            Console.WriteLine("Aborting");
                            break;
                        }

                        Console.WriteLine("Collecting last known position...");
                        Coordinate start = Coordinate.fromString(coordinates, i.getLastReceived());
                        Console.WriteLine("... captured");

                        Trajectory t = new Trajectory(start, end, samples);
                        List<Coordinate> traj = t.Compute();

                        Console.WriteLine("Trajectory computed");

                        OutputThread o = new OutputThread(traj, period, PYT.Properties.Settings.Default.OutgoingHost, PYT.Properties.Settings.Default.OutgoingPort);
                        Thread oThread = new Thread(new ThreadStart(o.process));
                        Console.WriteLine("Output initialised");
                        oThread.Start();
                        Console.WriteLine("Waiting for Output thread to start...");
                        while (!oThread.IsAlive);
                        Console.WriteLine("Execution commenced");

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
    }
}
