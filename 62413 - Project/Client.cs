using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace _62413___Project
{
    public class Client
    {
        private TcpClient tcpClient;
        private Thread listenThread;

        // Event triggered when a message is received from the server.
        public event Action<string> MessageReceived;

        // Connects the client to the server at the specified IP address and port.
        public void Connect(string ipAddress, int port)
        {
            // Create a new TCP client instance and connect to the server
            tcpClient = new TcpClient();
            tcpClient.Connect(ipAddress, port);
            Console.WriteLine("Connected to server...");

            // Start a new thread to listen for incoming messages from the server
            listenThread = new Thread(ListenForMessages);
            listenThread.Start();
        }

        // Listens for messages from the server in a separate thread.
        private void ListenForMessages()
        {
            // Get the network stream from the TCP client
            NetworkStream stream = tcpClient.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            // Continuously listen for messages until the connection is closed
            while (true)
            {
                // Read incoming data from the network stream
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                // Invoke the MessageReceived event to notify subscribers
                MessageReceived?.Invoke(receivedMessage);
            }
        }

        // Sends a message to the server.
        public void SendMessage(string message)
        {
            // Convert the message to a byte array
            byte[] buffer = Encoding.ASCII.GetBytes(message);

            // Send the message over the network stream
            tcpClient.GetStream().Write(buffer, 0, buffer.Length);
        }

        // Disconnects the client from the server.
        public void Disconnect()
        {
            // Close the TCP client connection
            tcpClient.Close();

            // Abort the listening thread
            listenThread.Abort();
        }
    }
}
