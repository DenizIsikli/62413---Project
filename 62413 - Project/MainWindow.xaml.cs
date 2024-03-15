using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _62413___Project
{
    public partial class MainWindow : Window
    {
        private Client client;
        private readonly ThemeSwitch _themeSwitch = new();
        public ObservableCollection<ChatMessage> ChatMessages { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Task.Run(() => StartServer());
            ChatMessages = [];
            listBoxChat.ItemsSource = ChatMessages;
            client = new Client();
            client.MessageReceived += Client_MessageReceived;
            client.Connect("127.0.0.1", 8888);

            _themeSwitch.ApplyTheme("LightTheme.xaml");
        }
        
        private void StartServer()
        {
            Server server = new();
            server.Start(8888);
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxMessage.Text)) return;
            textBoxMessage.Focus();
            client.SendMessage(textBoxMessage.Text);
            textBoxMessage.Clear();
        }

        private void TextBoxMessage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(textBoxMessage.Text))
            {
                client.SendMessage(textBoxMessage.Text);
                textBoxMessage.Clear();
            }
        }

        private void ButtonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            client.Disconnect();
        }

        private void Client_MessageReceived(string message)
        {
            var messageParts = message.Split([':'], 2);
            if (messageParts.Length == 2)
            {
                var chatMessage = new ChatMessage
                {
                    Name = messageParts[0].Trim(),
                    Message = messageParts[1].Trim(),
                    Timestamp = DateTime.Now.ToString("HH:mm:ss")
                };

                Dispatcher.Invoke(() =>
                {
                    ChatMessages.Add(chatMessage);
                    listBoxChat.ScrollIntoView(chatMessage);
                });
            }
        }

        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            _themeSwitch.ApplyTheme("DarkTheme.xaml");
        }

        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            _themeSwitch.ApplyTheme("LightTheme.xaml");
        }
    }

    public class ChatMessage
    {
        public string? Name { get; set; }
        public string? Message { get; set; }
        public string? Timestamp { get; set; }
    }
}
