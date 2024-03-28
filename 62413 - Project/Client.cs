using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Markup;


namespace _62413___Project
{
    public class Client
    {
        private Encryption encryption = new();
        private TcpClient? tcpClient;
        private Thread? listenThread;
        public event Action<string>? MessageReceived;

        /// <summary>
        /// Connects the client to the server.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public void Connect(string ipAddress, int port)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(ipAddress, port);
            Console.WriteLine("Connected to server");

            listenThread = new Thread(ListenForMessages);
            listenThread.Start();
        }

        /// <summary>
        /// Listen for messages from the server.
        /// </summary>
        private void ListenForMessages()
        {
            NetworkStream? stream = tcpClient.GetStream();
            byte[]? buffer = new byte[2048];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string? receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                MessageReceived?.Invoke(receivedMessage);
            }
        }

        /// <summary>
        /// Sends a message to the server.
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            byte[]? buffer = Encoding.UTF8.GetBytes(message);
            if (buffer.Length == 0) return;
            tcpClient?.GetStream().Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Disconnects the client from the server.
        /// </summary>
        public void Disconnect()
        {
            tcpClient?.Close();
            listenThread?.Abort();
        }
    }
}
