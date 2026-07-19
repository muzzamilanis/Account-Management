using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class OfficeExpDialog : Window
    {
        public OfficeExpDialog(OfficeExp existing = null)
        {
            InitializeComponent();
            var vm = new OfficeExpViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
