using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace PYT
{
    class InputThread : IOThread
    {
        // sockets stuff
        protected Socket receiveSocket;
        protected byte[] recBuffer;
        protected int BufferSize = 512;
        protected EndPoint bindEndPoint;

        // The last item received
        protected string lastReceived;

        /**
         * Pass construction to its parent
         * 
         * @param string host
         * @param int port
         * 
         * @return InputThread
         */
        public InputThread(string host, int port)
            : base(host, port)
        {

        }

        /**
         * Returns the last received item
         * 
         * @return string
         */
        public string getLastReceived()
        {
            return this.lastReceived;
        }

        /**
         * Connect and receive data for the thread
         */
        public override void process()
        {
            this.receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.bindEndPoint = new IPEndPoint(IPAddress.Parse(this.host), this.port);
            this.recBuffer = new byte[this.BufferSize];
            receiveSocket.Bind(bindEndPoint);
            receiveSocket.BeginReceiveFrom(recBuffer, 0, recBuffer.Length,
                SocketFlags.None, ref bindEndPoint,
                new AsyncCallback(MessageReceivedCallback), (object)this);
        }

        void MessageReceivedCallback(IAsyncResult result)
        {
            EndPoint remoteEndPoint = new IPEndPoint(0, 0);
            try
            {
                int bytesRead = receiveSocket.EndReceiveFrom(result, ref remoteEndPoint);
                this.lastReceived = System.Text.ASCIIEncoding.ASCII.GetString(recBuffer).Replace('\0',' ').Trim();
            }
            catch (SocketException e)
            {
            }

            receiveSocket.BeginReceiveFrom(recBuffer, 0, recBuffer.Length, SocketFlags.None, ref bindEndPoint, new AsyncCallback(MessageReceivedCallback), (object)this);
        }
    }
}
