using System;
using System.Collections.ObjectModel;
using System.Printing.IndexedProperties;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _62413___Project
{
    public partial class MainWindow : Window
    {
        private readonly ThemeSwitch _themeSwitch = new();

        public MainWindow()
        {
            _themeSwitch.ApplyTheme("LightTheme.xaml");
            InitializeComponent();

            Task.Run(() => StartServer());
            ChatMessages = new ObservableCollection<ChatMessage>();
            listBoxChat.ItemsSource = ChatMessages;
            client.MessageReceived += Client_MessageReceived;
            client.Connect("127.0.0.1", 8888);
        }
        
        /// <summary>
        /// Starts the server on a separate thread.
        /// </summary>
        private void StartServer()
        {
            Server server = new();
            server.Start(8888);
        }

        /// <summary>
        /// Button click event to send a message to the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxMessage.Text)) return;
            textBoxMessage.Focus();
            client.SendMessage(textBoxMessage.Text);
            textBoxMessage.Clear();
        }

        /// <summary>
        /// Textbox key down event to send a message to the server when the enter key is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxMessage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(textBoxMessage.Text))
            {
                client.SendMessage(textBoxMessage.Text);
                textBoxMessage.Clear();
            }
        }

        /// <summary>
        /// Button click event to disconnect the client from the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            client.Disconnect();
        }

        /// <summary>
        /// Event handler for when a message is received from the server.
        /// </summary>
        /// <param name="message"></param>
        private async void Client_MessageReceived(string message)
        {
            message = message.Trim();

            if (message.StartsWith("!"))
            {
                var parts = message.Substring(1).Split(' ', 2);
                var commandKey = parts[0];
                var commandParam = parts.Length > 1 ? parts[1] : string.Empty;

                if (generator.botCommands.TryGetValue(commandKey, out var commandFunc))
                {
                    var response = await commandFunc(commandParam);
                    SendMessageToChat("Bot", response);
                }
                else
                {
                    SendMessageToChat("Error", "Unknown command: " + commandKey);
                }
            }
            else
            {
                ProcessNormalMessage(message);
            }
        }

        private void SendMessageToChat(string senderName, string message)
        {
            var chatMessage = new ChatMessage
            {
                Name = senderName,
                Message = message,
                Timestamp = DateTime.Now.ToString("HH:mm:ss"),
            };

            Dispatcher.Invoke(() =>
            {
                ChatMessages.Add(chatMessage);
                listBoxChat.ScrollIntoView(chatMessage);
            });
        }

        private void ProcessNormalMessage(string message)
        {
            var messageParts = message.Split([':'], 2);
            if (messageParts.Length == 2)
            {
                var chatMessage = new ChatMessage
                {
                    Name = messageParts[0].Trim(),
                    Message = messageParts[1].Trim(),
                    Timestamp = DateTime.Now.ToString("HH:mm:ss"),
                };

                Dispatcher.Invoke(() =>
                {
                    ChatMessages.Add(chatMessage);
                    listBoxChat.ScrollIntoView(chatMessage);
                });
            }

            Main.Content = new LoginScreen();
        }

        /// <summary>
        /// Event handler for when the theme toggle is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            _themeSwitch.ApplyTheme("DarkTheme.xaml");
        }

        /// <summary>
        /// Event handler for when the theme toggle is unchecked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            _themeSwitch.ApplyTheme("LightTheme.xaml");
        }
    }
}
