using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        string micIp = "172.16.30.4";
        int port = 2202;
        int activeLobe = 0;
        int nonActiveLobe = 0;

        double totalActiveSeconds = 0;
        int activations = 0;
        DateTime? activeStart = null;

        using (TcpClient client = new TcpClient())
        {
            Console.WriteLine($"Connecting to {micIp}:{port}...");
            await client.ConnectAsync(micIp, port);
            Console.WriteLine("Connected & awaiting first update!");

            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string reply = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    if (Regex.Match(reply, @"REP (\d) AUTOMIX_GATE_OUT_EXT_SIG ON") is { Success: true } m)
                    {
                        if (activeStart == null)
                            activeStart = DateTime.Now;

                        activeLobe = int.Parse(m.Groups[1].Value);

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n{activeLobe} in use.");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\n{nonActiveLobe} deactivated.");
                        Console.ResetColor();
                        Console.WriteLine($"\nAverage active time: {(activations > 0 ? totalActiveSeconds / activations : 0):F2} seconds");
                    }
                    else if (Regex.Match(reply, @"REP (\d) AUTOMIX_GATE_OUT_EXT_SIG OFF") is { Success: true } n)
                    {
                        if (activeStart != null)
                        {
                            var duration = (DateTime.Now - activeStart.Value).TotalSeconds;
                            totalActiveSeconds += duration;
                            activations++;
                            activeStart = null;
                        }

                        nonActiveLobe = int.Parse(n.Groups[1].Value);

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n{activeLobe} in use.");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\n{nonActiveLobe} deactivated.");
                        Console.ResetColor();
                        Console.WriteLine($"\nAverage active time: {(activations > 0 ? totalActiveSeconds / activations : 0):F2} seconds");
                    }
                }
            }
        }
    }
}
