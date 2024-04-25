using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Page
    {
        public string nameInsert = "";
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void NameChange(object sender, RoutedEventArgs e)
        {
            nameInsert = LoginBox.Text;
            Generator.ClientName = nameInsert;
            ChatScreen p = new ChatScreen();
            this.NavigationService.Navigate(p);
        }

        public string GetLoginName()
        {
            return nameInsert;
        }
    }
}
