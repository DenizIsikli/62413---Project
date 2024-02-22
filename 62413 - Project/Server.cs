using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace _62413___Project
{
    public class Server
    {
        private TcpListener tcpListener;
        private List<TcpClient> clients = new List<TcpClient>();

        // Starts the server on the specified port and listens for incoming client connections.
        public void Start(int port)
        {
            // Create a TCP listener and start listening for incoming connections
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            Console.WriteLine("Server started...");

            // Continuously accept new client connections
            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                clients.Add(client);
                Console.WriteLine("Client connected...");

                // Handle client communication in a separate thread
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        // Handles communication with a connected client.
        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesReceived;

            try
            {
                // Continuously receive messages from the client
                while ((bytesReceived = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                    BroadcastMessage(message, client);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Client disconnected...");
            }
            finally
            {
                // Remove the client from the list and close the connection
                clients.Remove(client);
                client.Close();
            }
        }

        // Broadcasts a message to all connected clients except the origin client.
        private void BroadcastMessage(string message, TcpClient originClient)
        {
            foreach (var client in clients)
            {
                if (client != originClient)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = Encoding.ASCII.GetBytes(message);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
