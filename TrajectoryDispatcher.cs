using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PYT
{
    class TrajectoryDispatcher
    {
        // the coordinates we are working with
        protected List<string> coordinates;

        // the end coordinates
        protected Coordinate end;

        // the number of samples to execute
        protected int samples;

        // the period between samples
        protected int period;

        // the output thread
        protected Thread oThread;

        // reference to the input thread
        InputThread iThread;

        /**
         * Constructs the dispatcher
         * 
         * @param List<string> coordinates  the coordinates we want to compute for
         * 
         * @return TrajectoryDispatcher
         */
        public TrajectoryDispatcher(List<string> coordinates, ref InputThread iThread)
        {
            this.coordinates = coordinates;
            this.iThread = iThread;
        }

        /**
         * Collect data from the user
         * 
         * @return void
         */
        public void collectData()
        {
            this.end = new Coordinate(this.coordinates);

            // load in the coord values
            foreach (string coord in coordinates)
            {
                Console.WriteLine("Please enter the value for coordinate " + coord);
                this.end.setCoordinate(coord, double.Parse(Console.ReadLine()));
            }

            Console.WriteLine("Enter the sample factor (1)");
            this.samples = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the time between each sample being dispatched (milliseconds)");
            this.period = int.Parse(Console.ReadLine());
        }

        /**
         * Allows the programmatic setting of required data
         * 
         * @param Coordinate end        the end coordinate
         * @param int samples           the number of samples
         * @param int period            the period between samples
         * 
         * @return void
         */
        public void setData(Coordinate end, int samples, int period)
        {
            this.end = end;
            this.samples = samples;
            this.period = period;
        }

        /**
         * Waits until the current execution is completed
         */
        public void wait()
        {
            if (this.oThread != null)
            {
                while (this.oThread.IsAlive);
            }
        }

        /**
         * Executes the trajectory
         * 
         * @param Coordinate start     the coordinate to move from
         * 
         * @return void
         */
        public void Execute(Coordinate start)
        {
            Trajectory t = new Trajectory(start, this.end, this.samples);
            List<Coordinate> traj = t.Compute();

            Console.WriteLine("Trajectory computed");

            OutputThread o = new OutputThread(traj, this.period, PYT.Properties.Settings.Default.OutgoingHost, PYT.Properties.Settings.Default.OutgoingPort, ref this.iThread);
            this.oThread = new Thread(new ThreadStart(o.process));
            Console.WriteLine("Output initialised");
            this.oThread.Start();
            Console.WriteLine("Waiting for Output thread to start...");
            while (!this.oThread.IsAlive);
            Console.WriteLine("Execution commenced");
        }
    }
}
