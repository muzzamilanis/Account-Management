using AMS.Helpers;
using AMS.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace AMS.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _currentPage = "Welcome";
        public string CurrentPage { get => _currentPage; set => SetField(ref _currentPage, value); }

        private double _exchangeRate;
        public double ExchangeRate
        {
            get => _exchangeRate;
            set
            {
                SetField(ref _exchangeRate, value);
                SettingsService.Instance.ExchangeRate = value;
            }
        }

        private double _ugxExchangeRate;
        public double UgxExchangeRate
        {
            get => _ugxExchangeRate;
            set
            {
                SetField(ref _ugxExchangeRate, value);
                SettingsService.Instance.UgxExchangeRate = value;
            }
        }

        public bool IsMultiCurrencyEnabled => SettingsService.Instance.Settings.EnableMultiCurrency;
        public string UgxRateEditLabel => $"1 {SettingsService.Instance.Settings.BaseCurrencyCode} = ? UGX:";
        public string UgxRatePrefix => $" 1 {SettingsService.Instance.Settings.BaseCurrencyCode} = ";

        private string _companyName;
        public string CompanyName { get => _companyName; set => SetField(ref _companyName, value); }

        public bool IsDbOpen => DatabaseService.Instance.IsConnected;

        public ICommand NavigateCommand { get; }

        public MainViewModel()
        {
            _exchangeRate = SettingsService.Instance.ExchangeRate;
            _ugxExchangeRate = SettingsService.Instance.UgxExchangeRate;
            _companyName = SettingsService.Instance.CompanyName;
            NavigateCommand = new RelayCommand(page => CurrentPage = page?.ToString() ?? "Welcome");
        }

        public void RefreshDbState() => OnPropertyChanged(nameof(IsDbOpen));
        public void RefreshCompanyName() => CompanyName = SettingsService.Instance.CompanyName;
    }
}
