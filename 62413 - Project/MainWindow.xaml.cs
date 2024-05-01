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
            InitializeComponent();
            Main.Content = new Lobby();
        }
    }
}
