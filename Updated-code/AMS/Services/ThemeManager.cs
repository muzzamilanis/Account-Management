using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;

namespace AMS.Services
{
    public class ThemeManager
    {
        private static ThemeManager _instance;
        public static ThemeManager Instance => _instance ?? (_instance = new ThemeManager());

        public string CurrentTheme { get; private set; } = "Original";
        public bool IsDarkMode { get; private set; } = false;

        public void ApplyTheme(string themeName, bool isDark)
        {
            CurrentTheme = themeName;
            IsDarkMode = isDark;

            string baseTheme = isDark ? "BaseDark" : "BaseLight";
            string accentTheme = "Theme" + themeName;

            var baseDict = new ResourceDictionary { Source = new Uri("pack://application:,,,/Themes/" + baseTheme + ".xaml") };
            var accentDict = new ResourceDictionary { Source = new Uri("pack://application:,,,/Themes/" + accentTheme + ".xaml") };

            // Find and remove old theme dictionaries
            var existingThemes = Application.Current.Resources.MergedDictionaries
                .Where(d => d.Source != null && (d.Source.ToString().Contains("Base") || d.Source.ToString().Contains("Theme")))
                .ToList();

            foreach (var dict in existingThemes)
            {
                Application.Current.Resources.MergedDictionaries.Remove(dict);
            }

            Application.Current.Resources.MergedDictionaries.Add(baseDict);
            Application.Current.Resources.MergedDictionaries.Add(accentDict);
        }
    }
}
