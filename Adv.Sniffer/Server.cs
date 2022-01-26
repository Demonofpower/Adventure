using System;
using System.Net;
using System.Net.Sockets;
using Adv.Sniffer.Enums;

namespace Adv.Sniffer
{
    class Server
    {
        private bool output;
        private string name;

        private TcpListener m_vServer;

        private Client m_vClient;

        private ServerType type;

        public Server(string myAddress, int localPort, string remoteAddress, int remotePort, ServerType type, bool output = false, string name = "")
        {
            // Setup class defaults..
            this.LocalAddress = myAddress;
            this.LocalPort = localPort;
            this.RemoteAddress = remoteAddress;
            this.RemotePort = remotePort;

            this.output = output;
            this.name = name;

            this.type = type;
        }

        public bool Start()
        {
            try
            {
                // Cleanup any previous objects..
                this.Stop();

                // Create the new TcpListener..
                this.m_vServer = new TcpListener(IPAddress.Parse(this.LocalAddress), this.LocalPort);
                this.m_vServer.Start();

                // Setup the async handler when a client connects..
                this.m_vServer.BeginAcceptTcpClient(new AsyncCallback(OnAcceptTcpClient), this.m_vServer);
                return true;
            }
            catch (Exception ex)
            {
                this.Stop();
                Console.WriteLine("Exception caught inside of Server::Start\r\n" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Stops the local listening server if it is started.
        /// </summary>
        public void Stop()
        {
            // Cleanup the client object..
            if (this.m_vClient != null)
                this.m_vClient.Stop();
            this.m_vClient = null;

            // Cleanup the server object..
            if (this.m_vServer != null)
                this.m_vServer.Stop();
            this.m_vServer = null;
        }

        /// <summary>
        /// Async callback handler that accepts incoming TcpClient connections.
        /// NOTE:
        ///     It is important that you use the results server object to
        ///     prevent threading issues and object disposed errors!
        /// </summary>
        /// <param name="result"></param>
        private void OnAcceptTcpClient(IAsyncResult result)
        {
            // Ensure this connection is complete and valid..
            if (result.IsCompleted == false || !(result.AsyncState is TcpListener))
            {
                this.Stop();
                return;
            }

            // Obtain our server instance. (YOU NEED TO USE IT LIKE THIS DO NOT USE this.m_vServer here!)
            TcpListener tcpServer = (result.AsyncState as TcpListener);
            TcpClient tcpClient = null;

            try
            {
                // End the async connection request..
                tcpClient = tcpServer.EndAcceptTcpClient(result);

                // Kill the previous client that was connected (if any)..
                if (this.m_vClient != null)
                    this.m_vClient.Stop();

                // Prepare the client and start the proxying..
                this.m_vClient = new Client(tcpClient.Client, output, name, type);
                this.m_vClient.Start(this.RemoteAddress, this.RemotePort);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Error while attempting to complete async connection.");
            }

            // Begin listening for the next client..
            tcpServer.BeginAcceptTcpClient(new AsyncCallback(OnAcceptTcpClient), tcpServer);
        }

        /// <summary>
        /// Gets or sets the local address of this listen server.
        /// </summary>
        public String LocalAddress { get; set; }

        /// <summary>
        /// Gets or sets the local port of this listen server.
        /// </summary>
        public Int32 LocalPort { get; set; }

        /// <summary>
        /// Gets or sets the remote address to forward the client to.
        /// </summary>
        public String RemoteAddress { get; set; }

        /// <summary>
        /// Gets or sets the remote port to foward the client to.
        /// </summary>
        public Int32 RemotePort { get; set; }
    }
}