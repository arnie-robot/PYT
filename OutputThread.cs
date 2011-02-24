using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace PYT
{
    class OutputThread : InputThread
    {
        // Trajectory to execute
        protected List<Coordinate> trajectory;

        // Period between requests to dispatch
        protected int period;

        // reference to the input thread
        protected InputThread iThread;

        // whether reverse action is permitted
        bool permitReverse = true;

        /**
         * Construct and pass to parent
         * 
         * @param string host
         * @param int port
         * 
         * @return InputThread
         */
        public OutputThread(List<Coordinate> trajectory, int period, string host, int port, ref InputThread iThread)
            : base(host, port)
        {
            this.trajectory = trajectory;
            this.period = period;
            this.iThread = iThread;
        }

        /**
         * Process method to run the actual delivery
         */
        public override void process()
        {
            Coordinate prevCoord = null;
            Coordinate lastReceived = null;
            int threshold = 25;
            bool quit = false;
            foreach (Coordinate coord in this.trajectory)
            {
                //if (prevCoord != null)
                if (prevCoord != null)
                {
                    lastReceived = Coordinate.fromString(coord.getCoordinateNames(), this.iThread.getLastReceived());
                    foreach (string c in coord.getCoordinateNames())
                    {
                        if (
                            (lastReceived.getCoordinate(c) > prevCoord.getCoordinate(c) + threshold) ||
                            (lastReceived.getCoordinate(c) < prevCoord.getCoordinate(c) - threshold)
                            )
                        {
                            Console.WriteLine("WARNING: Value out of range in " + c + ", " + lastReceived.getCoordinate(c).ToString() + " outside " + prevCoord.getCoordinate(c).ToString() + " with threshold " + threshold.ToString() + ". Abort!");
                            quit = true;
                        }
                    }
                }
                prevCoord = coord;
                if (quit)
                {
                    break;
                }
                Socket sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                EndPoint sendEndPoint = new IPEndPoint(IPAddress.Parse(this.host), this.port);
                byte[] buffer = coord.toBytes();
                sendSocket.SendTo(buffer, buffer.Length, SocketFlags.None, sendEndPoint);
                Thread.Sleep(this.period);
            }
            if (!quit)
            {
                // successful execution
                Console.WriteLine("Execution completed");
            }
            else if (this.permitReverse && lastReceived != null)
            {
                // something got in the way
                Console.WriteLine("Inconsistency detected, reversing");
                Thread.Sleep(this.period * 10);
                Trajectory reverse = new Trajectory(Coordinate.fromString(this.trajectory[0].getCoordinateNames(), this.iThread.getLastReceived()), this.trajectory[0]);
                this.trajectory = reverse.Compute();
                Console.WriteLine("Reverse trajectory has " + this.trajectory.Count.ToString() + " steps");
                this.permitReverse = false;
                this.process();
            }
            else
            {
                Console.WriteLine("Inconsistency detected and unable to reverse, abort!");
            }
        }
    }
}