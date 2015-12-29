using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Microsoft.AspNet.SignalR.Client;
using TimerTimer = System.Timers.Timer;
using Microsoft.AspNet.SignalR.Client.Transports;
using PR2Hub.Core;

namespace PR2PS.GameServer
{
    /// <summary>
    /// Class representing high level listener.
    /// Its purpose is to initialize TCP listener and handle connected clients at separate threads
    /// as well as to handle communication with WebServer using RPC.
    /// </summary>
    class MainServer
    {
        private String name;
        private IPAddress ipAddress;
        private Int32 port;
        private TcpListener listener;
        private Boolean serverOn = true;
        private Int32 countNum = 0;

        private PR2Server gameServer;
        private TimerTimer keepAliveTimer;

        // SignalR.
        private String hubConnectionURL;
        private HubConnection hubConnection;
        private IHubProxy remoteServerHubProxy;

        /// <summary>
        /// Server constructor.
        /// </summary>
        /// <param name="ip">Listening address.</param>
        /// <param name="port">Listening port.</param>
        public MainServer(IPAddress ip, Int32 port, String hubConnectionURL, String name)
        {
            this.name = name;
            this.ipAddress = ip;
            this.port = port;
            this.hubConnectionURL = hubConnectionURL;
            this.listener = new TcpListener(new IPEndPoint(ip, port));
            this.gameServer = new PR2Server(name);
            this.keepAliveTimer = new TimerTimer(Constants.KEEPALIVE_INTERVAL);
            this.keepAliveTimer.Elapsed += keepAliveTimer_Elapsed;
        }

        void keepAliveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.remoteServerHubProxy.ServerAlive(this.name);
        }

        /// <summary>
        /// Initializes connection with web server using SignalR, registers event handlers
        /// and starts listening for incoming connections.
        /// </summary>
        public void Run()
        {
            // --- SignalR RPC stuff---
            Console.WriteLine("Attempting to establish connection with the web server...");
            hubConnection = new HubConnection(this.hubConnectionURL);
            //hubConnection.TraceLevel = TraceLevels.All;
            //hubConnection.TraceWriter = Console.Out;
            remoteServerHubProxy = hubConnection.CreateHubProxy("SignalRHub");
            // Register clientside methods.
            remoteServerHubProxy.On<Int32, AccountDataDTO>("LoginSuccessful", new Action<Int32, AccountDataDTO>((lid, accData) => this.gameServer.LoginSuccessful(lid, accData)));
            remoteServerHubProxy.On<Int64, String>("ForceLogout", new Action<Int64, String>((uid, ip) => this.gameServer.ForceLogout(uid, ip)));
            remoteServerHubProxy.On<String, String>("SystemMessage", new Action<String, String>((msg, user) => this.gameServer.SystemMessage(msg, user)));
            // Strange bug that I failed to figure out. SignalR connection will be successfully established
            // only on second attempt.
            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    Console.WriteLine("Attempt {0}...", i);
                    hubConnection.Start().Wait();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Attempt {0} failed...", i);
                    if (i == 5)
                    {
                        throw;
                    }
                }
            }
            Console.WriteLine("Connection with web server established successfully.");
            // ---------

            Console.WriteLine("Starting server...");
            this.remoteServerHubProxy.RegisterServer(this.name, this.ipAddress, this.port);
            this.listener.Start();
            Console.WriteLine("Server successfuly started, waiting for incoming connections...");

            this.keepAliveTimer.Start();

            while (this.serverOn)
            {
                TcpClient client = this.listener.AcceptTcpClient();
                Thread servThread = new Thread(new ThreadStart(() => this.acceptClient(client)));
                servThread.Start();
            }
        }

        /// <summary>
        /// Handler for newly connected client.
        /// </summary>
        /// <param name="tcpClient">TcpClient for communication.</param>
        private void acceptClient(TcpClient tcpClient)
        {
            Console.WriteLine(String.Format("Client connected. His address is {0}.", tcpClient.Client.RemoteEndPoint));
            Console.WriteLine(String.Format("Number of connected clients is now {0}.", ++this.countNum));

            NetworkStream connection = tcpClient.GetStream();
            Byte[] recvData = new Byte[2048];
            String recvMsg = null;

            ConnectedClient client = new ConnectedClient(connection, tcpClient.Client.RemoteEndPoint, this.gameServer, this.remoteServerHubProxy);

            try
            {
                while (connection.Read(recvData, 0, recvData.Length) > 0)
                {
                    recvMsg = ASCIIEncoding.ASCII.GetString(recvData).TrimEnd('\0');
                    String logMsg = String.Format("From {0}: {1}", tcpClient.Client.RemoteEndPoint, recvMsg);
                    //Console.WriteLine(logMsg);

                    client.HandleReceivedMessage(recvMsg);

                    Array.Clear(recvData, 0, recvData.Length);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Klient odpojeny.

            this.gameServer.RemoveClient(client);

            Console.WriteLine(String.Format("Client disconnected, number of clients is now {0}.", --this.countNum));
            connection.Dispose();
            tcpClient.Close();
        }
    }
}
