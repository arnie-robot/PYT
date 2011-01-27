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

        /**
         * Construct and pass to parent
         * 
         * @param string host
         * @param int port
         * 
         * @return InputThread
         */
        public OutputThread(List<Coordinate> trajectory, int period, string host, int port)
            : base(host, port)
        {
            this.trajectory = trajectory;
            this.period = period;
        }

        /**
         * Process method to run the actual delivery
         */
        public override void process()
        {
            foreach (Coordinate coord in this.trajectory)
            {
                Socket sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                EndPoint sendEndPoint = new IPEndPoint(IPAddress.Parse(this.host), this.port);
                byte[] buffer = coord.toBytes();
                sendSocket.SendTo(buffer, buffer.Length, SocketFlags.None, sendEndPoint);
                Thread.Sleep(this.period);
            }
            Console.WriteLine("Execution completed");
        }
    }
}
