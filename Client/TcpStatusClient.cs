using System.Net.Sockets;
using System.Text;
using TcpStatusClient.Protocol;

namespace TcpStatusClient
{
    /// <summary>
    /// represents a single tcp client to connect to the server
    /// handles establishing the connection and managing network stream
    /// </summary>
    public class StatusClient
    {
        private readonly string _ipAddress;
        private readonly int _port;

        private TcpClient? _client;
        private NetworkStream? _stream;

        private bool _isBusy;

        /// <summary>
        /// initialize a new client
        /// </summary>
        /// <param name="ip">server ip address</param>
        /// <param name="port">server port</param>
        public StatusClient(string ip, int port)
        {
            _ipAddress = ip;
            _port = port;
        }

        /// <summary>
        /// starts the client and try to connect to server
        /// initialize network stream on success
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            try
            {
                // tcp client instance
                _client = new TcpClient();

                // connect to the server using the ip address and port 
                await _client.ConnectAsync(_ipAddress, _port);
                _stream = _client.GetStream();

                Console.WriteLine("Server connected");

                // start listnening to server
                await ListenAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }
        }

        /// <summary>
        /// listen to server commands asynchronously 
        /// continuously read data from stream, decode it and trigger the handeling logic
        /// </summary>
        private async Task ListenAsync()
        {
            // buffer to read incoming bytes
            var buffer = new byte[1024];

            try
            {
                while (_client.Connected)
                {
                    // read incoming data asynchronously
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);

                    // check if connection is closed
                    if (bytesRead == 0)
                        break;

                    // decode and handle the command
                    var command = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    await HandleCommandAsync(command);
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
            finally
            {
                // close server socket on error
                _stream?.Close();
                _client?.Close();

                Console.WriteLine("Server disconnected");
            }
        }

        private async Task HandleCommandAsync(string command)
        {
            Console.WriteLine($"Received command: {command}");

            if (command == ProtocolMessages.StatusReq)
            {
                // acknowledge received command
                await SendAsync(ProtocolMessages.Ack);

                // inform server if client is busy
                if (_isBusy)
                {
                    await SendAsync(ProtocolMessages.Busy);
                    return;
                }
                _isBusy = true;

                _ = Task.Run(SimulateHardwareProcessingAsync);
            }
        }

        /// <summary>
        /// sends a utf-8 encoded message to server asynchronously
        /// </summary>
        /// <param name="message">message to send</param>
        private async Task SendAsync(string message)
        {
            try
            {
                // response message string -> bytes and write to network stream
                var data = Encoding.UTF8.GetBytes($"{message} \n");
                await _stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Send error: {ex.Message}");
            }
        }

        private async Task SimulateHardwareProcessingAsync()
        {
            // simulate processing time (3s)
            await Task.Delay(3000); 
        }
    }
}