using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Markup;

namespace _62413___Project
{
    public class Client
    {
        private TcpClient? tcpClient;
        private Thread? listenThread;
        public event Action<string>? MessageReceived;

        public void Connect(string ipAddress, int port)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(ipAddress, port);
            Console.WriteLine("Connected to server");

            listenThread = new Thread(ListenForMessages);
            listenThread.Start();
        }

        private void ListenForMessages()
        {
            NetworkStream? stream = tcpClient.GetStream();
            byte[]? buffer = new byte[1024];
            int bytesRead;

            while (true)
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;
                string? receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                MessageReceived?.Invoke(receivedMessage);
            }
        }

        public void SendMessage(string message)
        {
            byte[]? buffer = Encoding.ASCII.GetBytes(message);
            if (buffer.Length == 0) return;
            tcpClient?.GetStream().Write(buffer, 0, buffer.Length);
        }

        public void Disconnect()
        {
            tcpClient?.Close();
            listenThread?.Abort();
        }
    }
}
