using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _62413___Project
{
    /// <summary>
    /// Interaction logic for ChatScreen.xaml
    /// </summary>
    public partial class ChatScreen : Page
    {
        private readonly Client client = new();
        private readonly Handler handler = new();
        private readonly ThemeSwitch _themeSwitch = new();
        public ObservableCollection<ChatMessage> ChatMessages { get; set; }
        public ChatScreen(string serverUrl, int serverPort)
        {
            _themeSwitch.ApplyTheme("LightTheme.xaml");
            InitializeComponent();
         
            ChatMessages = new ObservableCollection<ChatMessage>();
            listBoxChat.ItemsSource = ChatMessages;
            client.MessageReceived += Client_MessageReceived;
            client.Connect(serverUrl, serverPort);

            ServerUrl.Text = serverUrl+":"+serverPort;
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
            string encryptedMessage = Encryption.EncryptString(textBoxMessage.Text, Handler.Password);
            client.SendMessage(encryptedMessage);
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
                ButtonSend_Click(sender, e);
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
            var messageParts = message.Split([':'], 2);
            if (messageParts.Length == 2)
            {
                string encryptedMessage = messageParts[1].Trim();
                string decryptedMessage = Encryption.DecryptString(encryptedMessage, Handler.Password);

                if (decryptedMessage.StartsWith("!gpt"))
                {
                    await ExecuteBotCommand(decryptedMessage);
                }
                else
                {
                    ProcessNormalMessage(messageParts[0].Trim(), decryptedMessage);
                }
            }
        }

        /// <summary>
        /// Executes a bot command.
        /// </summary>
        /// <param name="decryptedMessage"></param>
        /// <returns></returns>
        private async Task ExecuteBotCommand(string decryptedMessage)
        {
            var parts = decryptedMessage.Split(' ', 2);
            var commandKey = parts[0].Substring(1);
            var commandParam = parts.Length > 1 ? parts[1] : string.Empty;

            if (handler.botCommands.TryGetValue(commandKey, out var commandFunc))
            {
                var response = await commandFunc(commandParam);
                SendMessageToChat("Bot", response);
            }
            else
            {
                SendMessageToChat("Bot", "Unknown command: " + commandKey);
            }
        }

        /// <summary>
        /// Sends a message to the chat.
        /// </summary>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
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

        /// <summary>
        /// Processes a normal message.
        /// </summary>
        /// <param name="senderName"></param>
        /// <param name="decryptedMessage"></param>
        private void ProcessNormalMessage(string senderName, string decryptedMessage)
        {
            var chatMessage = new ChatMessage
            {
                Name = senderName,
                Message = decryptedMessage,
                Timestamp = DateTime.Now.ToString("HH:mm:ss"),
            };

            Dispatcher.Invoke(() =>
            {
                ChatMessages.Add(chatMessage);
                listBoxChat.ScrollIntoView(chatMessage);
            });
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

    /// <summary>
    /// Represents a chat message.
    /// </summary>
    public class ChatMessage
    {
        public string? Name { get; set; }
        public string? Message { get; set; }
        public string? Timestamp { get; set; }
    }
}
