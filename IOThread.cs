using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PYT
{
    abstract class IOThread
    {
        // host and port to listen on
        protected string host;
        protected int port;

        /**
         * Constructs
         * 
         * @param string host       the host to connect to
         * @param int port          the port to connect to
         * 
         * @return IOThread
         */
        public IOThread(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        /**
         * For the thread process action
         */
        public abstract void process();
    }
}
