using System;
using System.Windows;

namespace _62413___Project
{
    public partial class MainWindow : Window
    {
        private Client client;

        public MainWindow()
        {
            InitializeComponent();
            client = new Client();
            client.MessageReceived += Client_MessageReceived;
            client.Connect("127.0.0.1", 8888); // Connect to server at localhost and port 8888
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
            Dispatcher.Invoke(() => listBoxChat.Items.Add(message));
        }
    }
}
