using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace TCPClient
{
    class Program
    {
        static int port = 8888;
        static string ip = "";
        static void Main()
        {
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string message = "";
            socket.Connect(ipEnd);
            while (message!="close")
            { 
                try
                {
                    Console.WriteLine("Write your message:");
                    message = Console.ReadLine();
                    socket.Send(Encoding.Unicode.GetBytes(message));
                    StringBuilder sb = new StringBuilder();
                    do
                    {
                        byte[] arr = new byte[256];
                        int size = 0;
                        size = socket.Receive(arr);
                        sb.Append(Encoding.Unicode.GetString(arr, 0, size));
                    }
                    while (socket.Available > 0);
                    Console.WriteLine(sb.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.Read();
        }
    }
}
