using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _62413___Project
{
    public class ThemeSwitch
    {
        public void ApplyTheme(string themeDictionaryPath)
        {
            var uri = new Uri($"pack://application:,,,/{themeDictionaryPath}", UriKind.Absolute);
            var resourceDict = new ResourceDictionary { Source = uri };

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}

