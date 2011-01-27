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
            Coordinate a = new Coordinate(new List<string>(new string[] { "x","y","z" }));
            a.setCoordinate("x", 0);
            a.setCoordinate("y", 0);
            a.setCoordinate("z", 0);

            Coordinate b = new Coordinate(new List<string>(new string[] { "x", "y", "z" }));
            b.setCoordinate("x", 1);
            b.setCoordinate("y", 10);
            b.setCoordinate("z", 0);

            Trajectory t = new Trajectory(a, b, 10);
            List<Coordinate> traj = t.Compute();

            Console.WriteLine("Done");








            /*Alpha oAlpha = new Alpha();
            Thread oThread = new Thread(new ThreadStart(oAlpha.Beta));
            Console.WriteLine("Value: " + oAlpha.number);
            oThread.Start();
            while (!oThread.IsAlive) ;
            Thread.Sleep(1);
            Console.WriteLine("Value: " + oAlpha.number);
            oThread.Abort();
            Console.WriteLine("Value: " + oAlpha.number);
            oThread.Join();

            Console.WriteLine();
            Console.WriteLine("Alpha.Beta has finished");

            try
            {
                Console.WriteLine("Try to restart the Alpha.Beta thread");
                oThread.Start();
            }
            catch (ThreadStateException)
            {
                Console.Write("ThreadStateException trying to restart Alpha.Beta. ");
                Console.WriteLine("Expected since aborted threads cannot be restarted.");
            }
            */
        }
    }

    public class Alpha
    {
        public int number = 0;

        // This method that will be called when the thread is started
        public void Beta()
        {
            while (true)
            {
                Console.WriteLine("Alpha.Beta is running in its own thread.");
                number++;
            }
        }
    };
}
