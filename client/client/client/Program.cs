using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class client
{
    public static void Main()
    {
        try
        {
            Console.WriteLine("Input server IP address");
            IPAddress ipAddr = IPAddress.Parse(Console.ReadLine());
            SendMessageFromSocket(10500, ipAddr);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            Console.ReadLine();
        }
    }

    public static void SendMessageFromSocket(int port, IPAddress ipAddr)
    {
        byte[] data = new byte[1024];

        
        IPEndPoint ipep = new IPEndPoint(ipAddr, port);

        Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
       
        clientSock.Connect(ipep);
        
        Console.WriteLine("Input message (<TheEnd> for end connection): ");
        string message = Console.ReadLine();

        Console.WriteLine("Connection with {0} ", clientSock.RemoteEndPoint.ToString());
        byte[] msg = Encoding.UTF8.GetBytes(message);

        int bytesSent = clientSock.Send(msg);

        int bytesRec = clientSock.Receive(data);

        Console.WriteLine("\nAnswer: {0}\n\n", Encoding.UTF8.GetString(data, 0, bytesRec));

        if (message.IndexOf("<TheEnd>") == -1)
            SendMessageFromSocket(port, ipAddr);

        clientSock.Shutdown(SocketShutdown.Both);
        clientSock.Close();
    }
}