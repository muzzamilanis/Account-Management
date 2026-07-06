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

        private string _companyName;
        public string CompanyName { get => _companyName; set => SetField(ref _companyName, value); }

        public bool IsDbOpen => DatabaseService.Instance.IsConnected;

        public ICommand NavigateCommand { get; }
        public ICommand OpenCompanySettingsCommand { get; }

        public MainViewModel()
        {
            _exchangeRate = SettingsService.Instance.ExchangeRate;
            _companyName = SettingsService.Instance.CompanyName;
            NavigateCommand = new RelayCommand(page => CurrentPage = page?.ToString() ?? "Welcome");
            OpenCompanySettingsCommand = new RelayCommand(() =>
            {
                var win = new Views.CompanySettingsWindow();
                win.ShowDialog();
                CompanyName = SettingsService.Instance.CompanyName;
            });
        }

        public void RefreshDbState() => OnPropertyChanged(nameof(IsDbOpen));
        public void RefreshCompanyName() => CompanyName = SettingsService.Instance.CompanyName;
    }
}
