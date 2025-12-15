using System.Net.Sockets;

namespace TcpStatusClient
{
    public class StatusClient
    {
        private readonly string _ipAddress;
        private readonly int _port;

        private TcpClient? _client;
        private NetworkStream? _stream;

        public StatusClient(string ip, int port)
        {
            _ipAddress = ip;
            _port = port;
        }

        public async Task StartAsync()
        {
            try
            {
                _client = new TcpClient();

                await _client.ConnectAsync(_ipAddress, _port);
                _stream = _client.GetStream();

                Console.WriteLine("Server connected");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }
        }
    }
}