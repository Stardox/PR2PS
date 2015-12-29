using Microsoft.Owin.Hosting;
using PR2PS.Web.Core;
using PR2PS.Web.Core.Management;
using System;
using System.Collections.Generic;
using System.Timers;

namespace PR2PS.Web
{
    /// <summary>
    /// Entry point class of the web server.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Timer responsible for removing dead servers from the list of active servers.
        /// </summary>
        public static Timer serverCheckTimer;

        /// <summary>
        /// Entry point method. Responsible for the initialization.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(String[] args)
        {
            String hostURL = "http://127.0.0.1:8080";

            if (args.Length < 1 || String.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Host URL not specified, will use '{0}'...", hostURL);
            }
            else
            {
                if (Uri.IsWellFormedUriString(args[0], UriKind.Absolute))
                {
                    hostURL = args[0];
                }
                else
                {
                    Console.WriteLine("Host URL '{0}' is not valid, will use '{1}'...", args[0], hostURL);                    
                }
            }

            try
            {
                Console.WriteLine("Attempting to start web server...");
                serverCheckTimer = new Timer(Constants.SERVER_KEEPALIVE_TIMER_MILLIS);
                serverCheckTimer.Elapsed += serverCheckTimer_Elapsed;
                serverCheckTimer.Start();

                using (WebApp.Start<Startup>(hostURL))
                {
                    Console.WriteLine("Server successfuly started at '{0}'.\nPress Enter to stop server.", hostURL);
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("-==-");
                Console.WriteLine("{0} has occured...", ex);
                Console.WriteLine("Message: {0}", ex.Message);
                Console.WriteLine("Stack trace:\n{0}", ex.StackTrace);
                Console.WriteLine("-==-");
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Event handler for the timer responsible for removal of dead servers.
        /// </summary>
        /// <param name="sender">Event initiator (Timer in this case).</param>
        /// <param name="e">Event details.</param>
        static void serverCheckTimer_Elapsed(Object sender, ElapsedEventArgs e)
        {
            List<String> removedServers = ServerManager.Instance.RemoveDeadServers();
            if (removedServers.Count > 0)
            {
                Console.WriteLine("Following servers have been recycled:");
                foreach (String serverName in removedServers)
                {
                    Console.WriteLine(" - {0}", serverName);
                }
            }
        }
    }
}
