using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PYT
{
    class AutomatedCommander
    {
        // the coords to work with
        protected List<string> coordinates;

        // whether or not we are enabled
        protected bool enabled = false;

        // the thread that runs the system
        protected Thread acThread;

        // thread for input data
        protected InputThread i;
        protected Thread iThread;

        /**
         * Constructs
         * 
         * @param List<string> coordinates  the coords to operate over
         * 
         * @return AutomatedCommander
         */
        public AutomatedCommander(List<string> coordinates)
        {
            this.coordinates = coordinates;
        }

        /**
         * Starts execution of automated system
         */
        public void Start()
        {
            this.i = new InputThread(PYT.Properties.Settings.Default.IncomingHost, PYT.Properties.Settings.Default.IncomingDataPort);
            this.iThread = new Thread(new ThreadStart(this.i.process));
            this.iThread.Start();
            Console.WriteLine("Waiting for Input thread to start...");
            while (!this.iThread.IsAlive) ;
            Console.WriteLine("Input thread started");
            Console.WriteLine("Starting automated command system");
            this.acThread = new Thread(new ThreadStart(this.Execute));
            this.acThread.Start();
            Console.WriteLine("Waiting for thread to start...");
            while (!this.acThread.IsAlive);
            Console.WriteLine("... running");
            this.enabled = true;
        }

        /**
         * Stops execution of automated system
         */
        public void Stop()
        {
            if (this.enabled)
            {
                Console.WriteLine("Terminating...");
                this.acThread.Abort();
                this.acThread.Join();
                this.iThread.Abort();
                this.iThread.Join();
                this.enabled = false;
                Console.WriteLine("Automated command system terminated");
            }
            else
            {
                Console.WriteLine("Automated command system not running");
            }
        }

        /**
         * Executes the automated system
         */
        public void Execute()
        {
            InputThread a = new InputThread(PYT.Properties.Settings.Default.IncomingHost, PYT.Properties.Settings.Default.IncomingCommandPort);
            Thread aThread = new Thread(new ThreadStart(a.process));
            aThread.Start();
            Console.WriteLine("Waiting for Automated Input Command thread to start...");
            while (!aThread.IsAlive) ;
            Console.WriteLine("Automated input command thread started");

            Coordinate previous = new Coordinate(this.coordinates);
            foreach (string coord in this.coordinates)
            {
                previous.setCoordinate(coord, -1000.0);
            }

            TrajectoryDispatcher tjd = new TrajectoryDispatcher(this.coordinates, ref this.i);

            while (true)
            {
                try
                {
                    Coordinate next = Coordinate.fromString(this.coordinates, a.getLastReceived());
                    if (next.getCompareString() != previous.getCompareString())
                    {
                        previous = next;
                        // wait for the last one to finish excuting
                        tjd.wait();
                        tjd.setData(next, 50, 25);
                        tjd.Execute(Coordinate.fromString(this.coordinates, this.i.getLastReceived()));
                    }

                }
                catch (Exception e) { }
            }
        }
    }
}
