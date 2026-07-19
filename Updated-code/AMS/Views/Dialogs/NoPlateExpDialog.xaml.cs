using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class NoPlateExpDialog : Window
    {
        public NoPlateExpDialog(NoPlateExp existing = null)
        {
            InitializeComponent();
            var vm = new NoPlateExpViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
