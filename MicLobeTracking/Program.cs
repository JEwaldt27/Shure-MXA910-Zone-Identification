using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        HashSet<int> activeLobes = new HashSet<int>();
        HashSet<int> nonActiveLobes = new HashSet<int>();

        
        for (int i = 1; i <= 8; i++)
            nonActiveLobes.Add(i);

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

                        // Update sets
                        activeLobes.Add(activeLobe);
                        nonActiveLobes.Remove(activeLobe);

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Lobe {activeLobe} ACTIVATED.");
                        Console.ResetColor();

                        Console.WriteLine($"\nActive Lobes: {string.Join(", ", activeLobes)}");
                        Console.WriteLine($"Non-Active Lobes: {string.Join(", ", nonActiveLobes)}");

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

                        // Update sets
                        nonActiveLobes.Add(nonActiveLobe);
                        activeLobes.Remove(nonActiveLobe);

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Lobe {nonActiveLobe} DEACTIVATED.");
                        Console.ResetColor();

                        Console.WriteLine($"\nActive Lobes: {string.Join(", ", activeLobes)}");
                        Console.WriteLine($"Non-Active Lobes: {string.Join(", ", nonActiveLobes)}");

                        Console.WriteLine($"\nAverage active time: {(activations > 0 ? totalActiveSeconds / activations : 0):F2} seconds");
                    }
                }
            }
        }
    }
}
