using Microsoft.Extensions.Configuration;
using TcpStatusClient;


class Program
{
    static async Task Main(string[] args)
    {
        // laod configurations
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // read connection details
        var host = config["Server:Host"] ?? throw new InvalidOperationException("Server:Host not configured");
        var portStr = config["Server:Port"] ?? throw new InvalidOperationException("Server:Port not configured");
        var port = int.Parse(portStr);

        // read client name from terminal
        var clientName = args.Length > 0 ? args[0] : "Client";

        var client = new StatusClient(host, port, clientName);
        await client.StartAsync();
    }
}