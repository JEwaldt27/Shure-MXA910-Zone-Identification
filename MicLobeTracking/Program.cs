using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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
                    //Console.Write(reply);
                    if (Regex.Match(reply, @"REP (\d) AUTOMIX_GATE_OUT_EXT_SIG ON") is { Success: true } m)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n{int.Parse(m.Groups[1].Value)} in use.");
                        Console.ResetColor();
                    }
                    else if (Regex.Match(reply, @"REP (\d) AUTOMIX_GATE_OUT_EXT_SIG OFF") is { Success: true } n)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\n{int.Parse(n.Groups[1].Value)} not in use.");
                        Console.ResetColor();
                    }

                    //else
                    //    Console.WriteLine("\nRandom Response Received");

                }
            }
        }
    }
}
