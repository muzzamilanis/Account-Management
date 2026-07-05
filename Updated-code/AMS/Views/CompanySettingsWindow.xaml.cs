using AMS.ViewModels;
using System.Windows;

namespace AMS.Views
{
    public partial class CompanySettingsWindow : Window
    {
        public CompanySettingsWindow()
        {
            InitializeComponent();
            var vm = (CompanySettingsViewModel)DataContext;
            vm.CloseAction = result => { DialogResult = result; Close(); };
        }
    }
}
