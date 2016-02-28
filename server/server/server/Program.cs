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


    public static void startListen()
    {
        Console.WriteLine("Input your IP address");
        IPAddress ipAddr = IPAddress.Parse(Console.ReadLine());
        IPEndPoint ipep = new IPEndPoint(ipAddr, 10500);

        Socket servSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            servSock.Bind(ipep);
            servSock.Listen(10);
            while (true)
            {
                Console.WriteLine("Waiting the connection {0}", ipep);
              
                Socket handler = servSock.Accept();
                string data = null;

                byte[] bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);

                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                
                Console.Write("Received message: " + data + "\n\n");

                int count = 1;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == ' ')  
                        count++;
                }
                
                string reply = "In your message " + count.ToString()
                        + " words";
                byte[] msg = Encoding.UTF8.GetBytes(reply);
                handler.Send(msg);

                if (data == "<TheEnd>")
                {
                    Console.WriteLine("End connection.");
                }

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            servSock.Close();
            Console.ReadLine();
        }
    }

    public static int Main()
    {
        startListen();
        return 0;
    }
}


