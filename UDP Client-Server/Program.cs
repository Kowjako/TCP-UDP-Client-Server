using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDP_Client_Server
{
    class Program
    {
        static int localPort;
        static int remotePort;
        static Socket listeningSocket;
        static void Main()
        {
            Console.Write("Wpisz port dla akceptacji maila: ");
            localPort = Int32.Parse(Console.ReadLine());
            Console.Write("Wpisz port do wysylki maila: ");
            remotePort = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Wpisz liste i nacisnij Enter");
            Console.WriteLine();
            try
            {
                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Task listeningTask = new Task(Listen);
                listeningTask.Start();
                while(true)
                {
                    string message = Console.ReadLine();
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    EndPoint remotePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), remotePort);
                    listeningSocket.SendTo(data, remotePoint);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
        }
        public static void Listen()
        {
            try
            {
                IPEndPoint localIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), localPort);
                listeningSocket.Bind(localIP);
                while(true)
                {
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
                    StringBuilder sb = new StringBuilder();
                    byte[] data = new byte[256];
                    int bytes = 0;
                    do
                    {
                        bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
                        sb.Append(Encoding.Unicode.GetString(data));
                    }
                    while (listeningSocket.Available > 0);
                    IPEndPoint remoteFullIp = remoteIp as IPEndPoint;

                    Console.WriteLine("{0}:{1} - {2}", remoteFullIp.Address.ToString(),
                                                    remoteFullIp.Port, sb);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("1"+ex.Message);
            }
            finally
            {
                Close();
            }
        }
        public static void Close()
        {
            if (listeningSocket != null)
            {
                listeningSocket.Shutdown(SocketShutdown.Both);
                listeningSocket.Close();
                listeningSocket = null;
            }
        }
    }
}
