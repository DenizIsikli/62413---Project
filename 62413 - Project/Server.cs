﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace _62413___Project
{
    public class Server
    {
        private TcpListener? tcpListener;
        private readonly List<TcpClient> clients = [];

        public void Start(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            Console.WriteLine("Server started");

            while (true)
            {
                TcpClient? client = tcpListener.AcceptTcpClient();
                clients?.Add(client);
                Console.WriteLine("Client connected");

                Thread? clientThread = new(() => HandleClient(client));
                clientThread?.Start();
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream? stream = client.GetStream();
            byte[]? buffer = new byte[1024];
            int bytesReceived;

            string? name = "Client " + new Random().Next(1000, 9999) + " (You)";

            try
            {
                while ((bytesReceived = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string? message = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                    BroadcastMessage(name, message);
                } 
            }
            catch (Exception)
            {
                Console.WriteLine("Client disconnected");
            }
            finally
            {
                clients?.Remove(client);
                client?.Close();
            }
        }

        private void BroadcastMessage(string name, string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            string? formattedMessage = $"{name}: {message}";
            byte[]? buffer = Encoding.ASCII.GetBytes(formattedMessage);

            foreach (var client in clients)
            {
                NetworkStream? stream = client.GetStream();
                stream?.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
