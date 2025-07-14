using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        string localIp = "172.16.20.35";
        string micIp = "172.16.20.3";
        int port = 2202;

        using (TcpClient client = new TcpClient())
        {
            client.Client.Bind(new IPEndPoint(IPAddress.Parse(localIp), 0));
            Console.WriteLine($"Connecting to {micIp}:{port} from {localIp}...");
            await client.ConnectAsync(micIp, port);
            Console.WriteLine("Connected! Dumping all raw data:");

            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string reply = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.Write(reply);
                    if (reply.Contains("REP 1 AUTOMIX_GATE_OUT_EXT_SIG ON"))
                    {
                        Console.WriteLine("\nOne Is Active");
                    }
                    else if (reply.Contains("REP 2 AUTOMIX_GATE_OUT_EXT_SIG ON"))
                    {
                        Console.WriteLine("\nTwo Is Active");
                    }
                    else if (reply.Contains("REP 3 AUTOMIX_GATE_OUT_EXT_SIG ON"))
                    {
                        Console.WriteLine("\nThree Is Active");
                    }
                    else if (reply.Contains("REP 4 AUTOMIX_GATE_OUT_EXT_SIG ON"))
                    {
                        Console.WriteLine("\nFour Is Active");
                    }
                    else if (reply.Contains("REP 5 AUTOMIX_GATE_OUT_EXT_SIG ON"))
                    {
                        Console.WriteLine("\nFive Is Active");
                    }
                    else if (reply.Contains("REP 6 AUTOMIX_GATE_OUT_EXT_SIG ON"))
                    {
                        Console.WriteLine("\nSix Is Active");
                    }
                    else if (reply.Contains("REP 7 AUTOMIX_GATE_OUT_EXT_SIG ON"))
                    {
                        Console.WriteLine("\nSeven Is Active");
                    }
                    else if (reply.Contains("REP 8 AUTOMIX_GATE_OUT_EXT_SIG ON"))
                    {
                        Console.WriteLine("\nEight Is Active");
                    }
                    else 
                    { 
                        Console.WriteLine("\nRandom Response Received");
                    }

                }
            }
        }
    }
}
