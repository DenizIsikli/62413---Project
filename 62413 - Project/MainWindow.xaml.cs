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
