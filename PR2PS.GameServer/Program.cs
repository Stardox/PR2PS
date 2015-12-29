//#define THROW

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress hostIP = IPAddress.Parse("127.0.0.1");
            Int32 hostPort = 9160;
            String hubURL = "http://127.0.0.1:8080";
            String serverName = "Local";

            if (args.Length < 4)
            {
                Console.WriteLine("Too few arguments... Expected: -HOST_IP -PORT -WEB_URL -SERVERNAME");
                Console.WriteLine("Will use default values...");
            }
            else
            {
                if (!IPAddress.TryParse(args[0], out hostIP))
                {
                    Console.WriteLine("IP address is in incorrect format, will use default value...");
                }

                if (!Int32.TryParse(args[1], out hostPort) && hostPort > 1 && hostPort < 65535)
                {
                    Console.WriteLine("Port number is either in incorrect format or is out of range, will use default value...");
                }

                if (Uri.IsWellFormedUriString(args[2], UriKind.Absolute))
                {
                    hubURL = args[2];
                }
                else
                {
                    Console.WriteLine("Web server URL is in incorrect format, will use default value...");
                }

                serverName = args[3];
            }

            Console.WriteLine("Launching server '{0}' at '{1}:{2}'.", serverName, hostIP, hostPort);
            Console.WriteLine("Web server URL is '{0}'.", hubURL);

#if THROW
            try
#endif
            {
                MainServer myServ = new MainServer(hostIP, hostPort, hubURL, serverName);
                myServ.Run();
            }
#if THROW
            catch (Exception e)
            {
                Console.WriteLine("Error. :(\n" + e.Message);
                Console.ReadLine();
                Environment.Exit(-1);
            }
#endif
            Console.WriteLine("Press Enter to quit.");
            Console.ReadLine();
        }
    }
}
