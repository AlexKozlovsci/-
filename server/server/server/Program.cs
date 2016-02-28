using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class server
{

    public static void Callback(IAsyncResult AsyncCall)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        Byte[] busy = encoding.GetBytes("Server is busy");

        Socket server = (Socket)AsyncCall.AsyncState;
        Socket client = server.EndAccept(AsyncCall);

        Console.WriteLine("Accepted the connection: {0}", client.RemoteEndPoint);
        client.Send(busy);

        Console.WriteLine("Close connection");
        client.Close();

        server.BeginAccept(new AsyncCallback(Callback), server);
       
    }
    public static void startSend()
    {
        try
        {
            IPAddress localAddress = IPAddress.Parse("127.0.0.1");
            var servSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(localAddress, 10500);
            servSock.Bind(ipep);
            servSock.Listen(10);
            servSock.BeginAccept(new AsyncCallback(Callback), servSock);
            Console.WriteLine("Waiting for connection {0}", servSock.LocalEndPoint);

            while (true)
            {
                Console.WriteLine("busy...");
                Thread.Sleep(2000);
            }

        }
        catch (Exception e)
        {
           
            Console.WriteLine("Error : {0}", e.ToString());
        }
        finally
        {
            Console.ReadLine();
        }
    }

    public static int Main()
    {
        startSend();
        return 0;
    }
}


