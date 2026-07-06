using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class OfficeExpDialog : Window
    {
        public OfficeExpDialog()
        {
            InitializeComponent();
            var vm = new OfficeExpViewModel();
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
