using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Database.Video.Clients
{
    public class RtspClient : IDisposable
    {
        private readonly string _rtspUrl;
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private int _sequenceNumber = 1;
        private string _session;
        private bool _isRecording = false;
        private Stream _outputStream;
        private byte[] _previousFrame;
        private int _differenceFrameCounter = 0;
        private const int FullFrameInterval = 10;
        private const int RetryDelay = 5000; // Retry delay in milliseconds

        public event EventHandler<string> Connected;
        public event EventHandler<string> Disconnected;
        public event EventHandler<string> Retrying;
        public event EventHandler<byte[]> FrameReceived;

        public RtspClient(string rtspUrl)
        {
            _rtspUrl = rtspUrl;
        }

        public async Task ConnectAsync()
        {
            var uri = new Uri(_rtspUrl);
            _tcpClient = new TcpClient();

            while (!_tcpClient.Connected)
            {
                try
                {
                    await _tcpClient.ConnectAsync(uri.Host, uri.Port);
                    _networkStream = _tcpClient.GetStream();

                    Setup();
                    Play();
                    OnConnected("Connected to RTSP server.");
                }
                catch (SocketException)
                {
                    OnRetrying("Connection failed. Retrying in 5 seconds...");
                    await Task.Delay(RetryDelay);
                }
            }
        }

        public void Disconnect()
        {
            StopRecording();
            Dispose();
            OnDisconnected("Disconnected from RTSP server.");
        }

        public void StartRecording(string filePath)
        {
            _outputStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            _isRecording = true;
        }

        public void StopRecording()
        {
            _isRecording = false;
            _outputStream?.Close();
            _outputStream = null;
        }

        private void Setup()
        {
            try
            {
                string setupRequest = $"SETUP {_rtspUrl}/trackID=1 RTSP/1.0\r\nCSeq: {_sequenceNumber++}\r\nTransport: RTP/AVP;unicast;client_port=8000-8001\r\n\r\n";
                SendRequest(setupRequest);

                string response = GetResponse();
                var sessionLine = response.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)[4];
                _session = sessionLine.Split(":")[1].Trim();
            }
            catch (Exception ex)
            {
                OnDisconnected($"Error during setup: {ex.Message}");
            }
        }

        private void Play()
        {
            try
            {
                string playRequest = $"PLAY {_rtspUrl} RTSP/1.0\r\nCSeq: {_sequenceNumber++}\r\nSession: {_session}\r\n\r\n";
                SendRequest(playRequest);

                string response = GetResponse();

                // Start receiving data
                Task.Run(ReceiveData);
            }
            catch (Exception ex)
            {
                OnDisconnected($"Error during play: {ex.Message}");
            }
        }

        private async Task ReceiveData()
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while (true)
            {
                try
                {
                    while ((bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        var frameData = new byte[bytesRead];
                        Array.Copy(buffer, frameData, bytesRead);

                        OnFrameReceived(frameData);

                        if (_isRecording)
                        {
                            byte[] frameToWrite;
                            if (_previousFrame == null || _differenceFrameCounter >= FullFrameInterval)
                            {
                                // First frame or interval reached, treat as full frame
                                frameToWrite = AddHeader(frameData, isFullFrame: true);
                                _differenceFrameCounter = 0; // Reset counter after saving full frame
                            }
                            else
                            {
                                var differences = GetFrameDifferences(frameData);
                                if (differences.Length >= frameData.Length)
                                {
                                    // If difference encoding is not efficient, store full frame
                                    frameToWrite = AddHeader(frameData, isFullFrame: true);
                                    _differenceFrameCounter = 0; // Reset counter after saving full frame
                                }
                                else
                                {
                                    frameToWrite = AddHeader(differences, isFullFrame: false);
                                    _differenceFrameCounter++;
                                }
                            }

                            await _outputStream.WriteAsync(frameToWrite, 0, frameToWrite.Length);
                            await _outputStream.FlushAsync();
                            _previousFrame = frameData;
                        }
                    }
                }
                catch (IOException)
                {
                    OnDisconnected("Connection lost. Attempting to reconnect...");
                    await ConnectAsync(); // Attempt to reconnect
                }
                catch (Exception ex)
                {
                    OnDisconnected($"Error while receiving data: {ex.Message}");
                    await ConnectAsync(); // Attempt to reconnect
                }
            }
        }

        private byte[] GetFrameDifferences(byte[] currentFrame)
        {
            if (_previousFrame == null)
            {
                return currentFrame;
            }

            // Simple difference encoding: store only the bytes that have changed
            var differences = currentFrame
                .Select((b, i) => new { Byte = b, Index = i })
                .Where(x => x.Byte != _previousFrame[x.Index])
                .SelectMany(x => BitConverter.GetBytes(x.Index).Concat(new[] { x.Byte }))
                .ToArray();

            return differences;
        }

        private byte[] AddHeader(byte[] frameData, bool isFullFrame)
        {
            var header = new byte[5];
            header[0] = (byte)(isFullFrame ? 1 : 0); // 1 for full frame, 0 for difference frame
            Array.Copy(BitConverter.GetBytes(frameData.Length), 0, header, 1, 4); // 4-byte length

            return header.Concat(frameData).ToArray();
        }

        private void SendRequest(string request)
        {
            byte[] requestBytes = Encoding.ASCII.GetBytes(request);
            _networkStream.Write(requestBytes, 0, requestBytes.Length);
        }

        private string GetResponse()
        {
            byte[] buffer = new byte[4096];
            int bytesRead = _networkStream.Read(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }

        protected virtual void OnConnected(string message)
        {
            Connected?.Invoke(this, message);
        }

        protected virtual void OnDisconnected(string message)
        {
            Disconnected?.Invoke(this, message);
        }

        protected virtual void OnRetrying(string message)
        {
            Retrying?.Invoke(this, message);
        }

        protected virtual void OnFrameReceived(byte[] frameData)
        {
            FrameReceived?.Invoke(this, frameData);
        }

        public void Dispose()
        {
            _networkStream?.Dispose();
            _tcpClient?.Dispose();
            _outputStream?.Dispose();
        }
    }
}
