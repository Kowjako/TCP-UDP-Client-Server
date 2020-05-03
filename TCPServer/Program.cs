using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    class Program
    {
        static int port = 8888;
        static string ip = "127.0.0.1";
        static void Main()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Bind(ipEndPoint);
                socket.Listen(2);
                Console.WriteLine("Server is started..");
                Socket handler = socket.Accept();
                while (true)
                { 
                    StringBuilder sb = new StringBuilder();
                    int size = 0;
                    byte[] message = new byte[256];
                    do
                    {
                        size = handler.Receive(message);
                        sb.Append(Encoding.Unicode.GetString(message, 0, size));
                    }
                    while (handler.Available > 0);
                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + sb);
                    string answer = "Message has been sent";
                    handler.Send(Encoding.Unicode.GetBytes(answer));
                    if (sb.ToString() == "close")
                    {
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
