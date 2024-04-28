using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _62413___Project
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Lobby : Page
    {
        private string m_name = "";
        private string m_server = "";
        private string m_password = "";

        public Lobby()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a Ngrok tunnel and starts a server on the local machine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateServer(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields(true))
            {
                return;
            }


            Handler.Name = m_name;
            Handler.Password = m_password;

            Task.Run(() => StartServer());
            Ngrok ngrok = new();
            string tunnelUrl = ngrok.StartNgrokTunnel();
            if (tunnelUrl.IndexOf("tcp://") == 0)
            {
                tunnelUrl = tunnelUrl.Substring(6);
            }
            string serverUrl = tunnelUrl.Split(':')[0];
            int serverPort = int.Parse(tunnelUrl.Split(':')[1]);

            ChatScreen chatScreen = new ChatScreen(serverUrl, serverPort);
            this.NavigationService.Navigate(chatScreen);
        }

        /// <summary>
        /// Starts a server on the local machine with the specified port
        /// </summary>
        private void StartServer()
        {
            Server server = new();
            server.Start(8888);
        }
      
        /// <summary>
        /// Joins a server with the specified name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JoinServer(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields(false))
            {
                return;
            }

            Handler.Name = m_name;
            Handler.Password = m_password;
            string serverUrl = m_server.Split(':')[0];
            int serverPort = int.Parse(m_server.Split(':')[1]);
            ChatScreen chatScreen = new ChatScreen(serverUrl, serverPort);
            this.NavigationService.Navigate(chatScreen);
        }

        /// <summary>
        /// Updates the name of the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_name = Name.Text;
        }

        /// <summary>
        /// Updates the server address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_server = Server.Text;
        }

        /// <summary>
        /// Updates the password of the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_password = Password.Text;
        }

        /// <summary>
        /// Placeholder for lobby textboxes 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Server.Text == "Server address")
            {
                Server.Text = "";
                Server.Foreground = Brushes.Black;
            }
        }
        private void Server_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Server.Text == "") 
            {
                Server.Text = "Server address";
                Server.Foreground = Brushes.Silver;
            }
        }

        private void Name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "Name")
            {
                Name.Text = "";
                Name.Foreground = Brushes.Black;
            }
        }
        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "")
            {
                Name.Text = "Name";
                Name.Foreground = Brushes.Silver;
            }
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Text == "Password")
            {
                Password.Text = "";
                Password.Foreground = Brushes.Black;
            }
        }
        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Text == "")
            {
                Password.Text = "Password";
                Password.Foreground = Brushes.Silver;
            }
        }
    }
}
