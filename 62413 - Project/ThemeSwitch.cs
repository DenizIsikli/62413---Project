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
        /// <summary>
        /// Applies a theme to the application.
        /// </summary>
        /// <param name="themeDictionaryPath"></param>
        public void ApplyTheme(string themeDictionaryPath)
        {
            var uri = new Uri($"pack://application:,,,/{themeDictionaryPath}", UriKind.Absolute);
            var resourceDict = new ResourceDictionary { Source = uri };

            var oldDict = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source?.ToString().Contains("Theme") == true);
            if (oldDict != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(oldDict);
            }

            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}
