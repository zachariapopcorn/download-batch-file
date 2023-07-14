using System;
using System.Net.Http;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

class Program {
    public static async Task Main(string[] args) {
        if(args.Length == 0) {
            Console.WriteLine("No URL provided");
            return;
        }
        string url = args[0];
        string body;
        try {
            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage res = await client.GetAsync(url);
                body = await res.Content.ReadAsStringAsync();
            }
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

    public static Task ExecuteCommandAsync(string command) {
        ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", $"/c \"{command}\"");
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = true;
        using (Process process = Process.Start(processInfo)) {
            return process.WaitForExitAsync();
        }
    }
}