using TcpStatusClient;


class Program
{
    static async Task Main(string[] args)
    {
        // read client name from terminal
        var clientName = args.Length > 0 ? args[0] : "Client";

        var client = new StatusClient("127.0.0.1", 5000, clientName);
        await client.StartAsync();
    }
}