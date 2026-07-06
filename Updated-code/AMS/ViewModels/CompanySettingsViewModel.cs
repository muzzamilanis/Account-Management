using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels
{
    public class CompanySettingsViewModel : ViewModelBase
    {
        private CompanySettings _settings;
        public CompanySettings Settings { get => _settings; set => SetField(ref _settings, value); }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public CompanySettingsViewModel()
        {
            var orig = SettingsService.Instance.Settings;
            Settings = new CompanySettings
            {
                CompanyName = orig.CompanyName,
                CompanyAddress = orig.CompanyAddress,
                CompanyPhone = orig.CompanyPhone,
                CompanyEmail = orig.CompanyEmail,
                CompanyTagline = orig.CompanyTagline,
                DefaultExchangeRate = orig.DefaultExchangeRate,
                LoginPassword = orig.LoginPassword
            };
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public bool? DialogResult { get; private set; }
        public System.Action<bool?> CloseAction { get; set; }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Settings.CompanyName))
            { MessageBox.Show("Company name cannot be empty.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            SettingsService.Instance.UpdateSettings(Settings);
            DialogResult = true;
            CloseAction?.Invoke(true);
        }

        private void Cancel() { DialogResult = false; CloseAction?.Invoke(false); }
    }
}
