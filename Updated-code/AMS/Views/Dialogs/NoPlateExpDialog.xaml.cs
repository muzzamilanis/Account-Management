using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class NoPlateExpDialog : Window
    {
        public NoPlateExpDialog()
        {
            InitializeComponent();
            var vm = new NoPlateExpViewModel();
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
