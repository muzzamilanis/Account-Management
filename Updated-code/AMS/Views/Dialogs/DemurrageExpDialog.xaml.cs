using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class DemurrageExpDialog : Window
    {
        public DemurrageExpDialog()
        {
            InitializeComponent();
            var vm = new DemurrageExpViewModel();
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
