using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace _62413___Project
{
    public partial class MainWindow : Window
    {
        private Client client;
        public ObservableCollection<ChatMessage> ChatMessages { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Task.Run(() => StartServer());
            ChatMessages = new ObservableCollection<ChatMessage>();
            listBoxChat.ItemsSource = ChatMessages;
            client = new Client();
            client.MessageReceived += Client_MessageReceived;
            client.Connect("127.0.0.1", 8888); // Connect to server at localhost and port 8888
        }

        private void StartServer()
        {
            Server server = new Server();
            server.Start(8888); // Start server on port 8888
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            client.SendMessage(textBoxMessage.Text);
            textBoxMessage.Clear();
        }

        private void ButtonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            client.Disconnect();
            Application.Current.Shutdown();
        }

        private void Client_MessageReceived(string message)
        {
            // Assuming message comes in "Name: Message" format
            var messageParts = message.Split(new[] { ':' }, 2);
            if (messageParts.Length == 2)
            {
                var chatMessage = new ChatMessage
                {
                    Name = messageParts[0].Trim(),
                    Message = messageParts[1].Trim()
                };

                // Update the UI thread with the received message
                Dispatcher.Invoke(() => ChatMessages.Add(chatMessage));
            }
        }
    }

    public class ChatMessage
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
}
