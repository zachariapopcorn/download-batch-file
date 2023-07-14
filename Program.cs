using System;
using System.Net.Http;
using System.IO;
using System.Diagnostics;

class Program {
    public static void Main(string[] args) {
        if(args.Length == 0) {
            Console.WriteLine("No URL provided");
            return;
        }
        HttpClient client = new HttpClient();
        string url = args[0];
        string body;
        try {
            HttpResponseMessage res = client.GetAsync(url).Result;
            body = res.Content.ReadAsStringAsync().Result;
        } catch(Exception e) {
            Console.WriteLine($"Error while fetching content: {e.Message}");
            return;
        }
        string filePath = $"{System.IO.Path.GetTempPath()}\\BatchFileToRun.bat";
        using (StreamWriter sw = new StreamWriter(filePath)) {
            sw.WriteLine(body);
        }
        await ExecuteCommandAsync(filePath);
    }

    public static void ExecuteCommand(string command) {
        ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", $"/c \"{command}\"");
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = true;
        Process process = Process.Start(processInfo);
        process.WaitForExit();
    }
}