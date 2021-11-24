using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Adv.Sniffer
{
    class Client
    {
        private PacketReverser reverser;
        private string name;
        private bool output;

        private static Int32 MAX_BUFFER_SIZE = 2048;
        
        private Boolean m_vIsRunning;
        
        private Socket m_vClientSocket;

        private Byte[] m_vClientBuffer;
        private List<Byte> m_vClientBacklog;
        
        private Socket m_vServerSocket;

        private Byte[] m_vServerBuffer;
        private List<Byte> m_vServerBacklog;
        
        public Client(Socket sockClient, bool output, string name)
        {
            // Setup class defaults..
            this.m_vClientSocket = sockClient;
            this.m_vClientBuffer = new Byte[MAX_BUFFER_SIZE];
            this.m_vClientBacklog = new List<Byte>();

            this.m_vServerSocket = null;
            this.m_vServerBuffer = new Byte[MAX_BUFFER_SIZE];
            this.m_vServerBacklog = new List<Byte>();

            this.output = output;
            this.name = name;
            reverser = new PacketReverser();
        }
        
        public bool Start(String remoteTarget = "127.0.0.1", Int32 remotePort = 7777)
        {
            // Stop this client if it was already started before..
            if (this.m_vIsRunning == true)
                this.Stop();
            this.m_vIsRunning = true;

            // Attempt to parse the given remote target.
            // This allows an IP address or domain to be given.
            // Ex:
            //      127.0.0.1
            //      derp.no-ip.org

            IPAddress ipAddress = null;
            try
            {
                ipAddress = IPAddress.Parse(remoteTarget);
            }
            catch
            {
                try
                {
                    ipAddress = Dns.GetHostEntry(remoteTarget).AddressList[0];
                }
                catch
                {
                    throw new SocketException((int) SocketError.HostNotFound);
                }
            }

            try
            {
                // Connect to the target machine on a new socket..
                this.m_vServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.m_vServerSocket.BeginConnect(new IPEndPoint(ipAddress, remotePort),
                    new AsyncCallback((result) =>
                    {
                        // Ensure the connection was valid..
                        if (result == null || result.IsCompleted == false || !(result.AsyncState is Socket))
                            return;

                        // Obtain our server instance. (YOU NEED TO USE IT LIKE THIS DO NOT USE this.m_vServerSocket here!)
                        Socket serverSocket = (result.AsyncState as Socket);

                        // Stop processing if the server has told us to stop..
                        if (this.m_vIsRunning == false || serverSocket == null)
                            return;

                        // Complete the async connection request..
                        serverSocket.EndConnect(result);

                        // Start monitoring for packets..
                        this.m_vClientSocket.ReceiveBufferSize = MAX_BUFFER_SIZE;
                        serverSocket.ReceiveBufferSize = MAX_BUFFER_SIZE;
                        this.Server_BeginReceive();
                        this.Client_BeginReceive();
                    }), this.m_vServerSocket);

                return true;
            }
            catch (ObjectDisposedException ex)
            {
                // Process the exception as you wish here..
            }
            catch (SocketException ex)
            {
                // Process the exception as you wish here..
            }
            catch (Exception ex)
            {
                // Process the exception as you wish here..
            }

            return false;
        }

        public void Stop()
        {
            if (this.m_vIsRunning == false)
                return;

            // Cleanup the client socket..
            if (this.m_vClientSocket != null)
                this.m_vClientSocket.Close();
            this.m_vClientSocket = null;

            // Cleanup the server socket..
            if (this.m_vServerSocket != null)
                this.m_vServerSocket.Close();
            this.m_vServerSocket = null;

            this.m_vIsRunning = false;
        }
        
        private void Client_BeginReceive()
        {
            // Prevent invalid call..
            if (!this.m_vIsRunning)
                return;

            try
            {
                this.m_vClientSocket.BeginReceive(this.m_vClientBuffer, 0, MAX_BUFFER_SIZE, SocketFlags.None,
                    new AsyncCallback(OnClientReceiveData), this.m_vClientSocket);
            }
            catch (SocketException ex)
            {
                this.Stop();
            }
            catch (Exception ex)
            {
                this.Stop();
            }
        }
        
        private void Server_BeginReceive()
        {
            // Prevent invalid call..
            if (!this.m_vIsRunning)
                return;

            try
            {
                this.m_vServerSocket.BeginReceive(this.m_vServerBuffer, 0, MAX_BUFFER_SIZE, SocketFlags.None,
                    new AsyncCallback(OnServerReceiveData), this.m_vServerSocket);
            }
            catch (SocketException ex)
            {
                this.Stop();
            }
            catch (Exception ex)
            {
                this.Stop();
            }
        }
        
        private void OnClientReceiveData(IAsyncResult result)
        {
            // Prevent invalid calls to this function..
            if (!this.m_vIsRunning || result.IsCompleted == false || !(result.AsyncState is Socket))
            {
                this.Stop();
                return;
            }

            Socket client = (result.AsyncState as Socket);

            // Attempt to end the async call..
            Int32 nRecvCount = 0;
            try
            {
                nRecvCount = client.EndReceive(result);
                if (nRecvCount == 0)
                {
                    this.Stop();
                    return;
                }
            }
            catch
            {
                this.Stop();
                return;
            }

            // Read the current packet..
            Byte[] btRecvData = new Byte[nRecvCount];
            Array.Copy(this.m_vClientBuffer, 0, btRecvData, 0, nRecvCount);
            
            Console.ForegroundColor = ConsoleColor.Green;
            if (Catch(HexArithmetic.ByteArrayToString(btRecvData), Sender.Client))
            {
                // Send the packet to the server..
                this.SendToServer(btRecvData);
            }
            else
            {
                Console.WriteLine("x");
            }
            

            // Begin listening for next packet..
            this.Client_BeginReceive();
        }
        
        private void OnServerReceiveData(IAsyncResult result)
        {
            // Prevent invalid calls to this function..
            if (!this.m_vIsRunning || result.IsCompleted == false || !(result.AsyncState is Socket))
            {
                this.Stop();
                return;
            }

            Socket server = (result.AsyncState as Socket);

            // Attempt to end the async call..
            Int32 nRecvCount = 0;
            try
            {
                nRecvCount = server.EndReceive(result);
                if (nRecvCount == 0)
                {
                    this.Stop();
                    return;
                }
            }
            catch
            {
                this.Stop();
                return;
            }

            // Read the current packet..
            Byte[] btRecvData = new Byte[nRecvCount];
            Array.Copy(this.m_vServerBuffer, 0, btRecvData, 0, nRecvCount);

            Console.ForegroundColor = ConsoleColor.Red;
            Catch(HexArithmetic.ByteArrayToString(btRecvData), Sender.Server);
            // Send the packet to the client..
            this.SendToClient(btRecvData);

            // Begin listening for next packet..
            this.Server_BeginReceive();
        }
        
        public void SendToClient(byte[] btPacket)
        {
            if (!this.m_vIsRunning)
                return;

            try
            {
                this.m_vClientSocket.BeginSend(btPacket, 0, btPacket.Length, SocketFlags.None,
                    new AsyncCallback((x) =>
                    {
                        if (x.IsCompleted == false || !(x.AsyncState is Socket))
                        {
                            this.Stop();
                            return;
                        }

                        (x.AsyncState as Socket).EndSend(x);
                    }), this.m_vClientSocket);
            }
            catch (Exception ex)
            {
                this.Stop();
            }
        }
        
        public void SendToServer(byte[] btPacket)
        {
            if (!this.m_vIsRunning)
                return;

            try
            {
                this.m_vServerSocket.BeginSend(btPacket, 0, btPacket.Length, SocketFlags.None,
                    new AsyncCallback((x) =>
                    {
                        if (x.IsCompleted == false || !(x.AsyncState is Socket))
                        {
                            this.Stop();
                            return;
                        }

                        (x.AsyncState as Socket).EndSend(x);
                    }), this.m_vServerSocket);
            }
            catch (Exception ex)
            {
                this.Stop();
            }
        }
        
        public Socket ClientSocket
        {
            get
            {
                if (this.m_vIsRunning && this.m_vClientSocket != null)
                    return this.m_vClientSocket;
                return null;
            }
        }
        
        public Socket ServerSocket
        {
            get
            {
                if (this.m_vIsRunning && this.m_vServerSocket != null)
                    return this.m_vServerSocket;
                return null;
            }
        }

        private bool Catch(string data, Sender sender)
        {
            if (output)
            {
                return reverser.Reverse(data, sender, name, this);        
            }

            return true;
        }
    }
}