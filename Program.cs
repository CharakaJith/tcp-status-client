using TcpStatusClient;


class Program
{
    static async Task Main(string[] args)
    {
        var client = new StatusClient("127.0.0.1", 5000);
        await client.StartAsync();
    }
}