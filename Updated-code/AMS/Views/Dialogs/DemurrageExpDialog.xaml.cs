using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class DemurrageExpDialog : Window
    {
        public DemurrageExpDialog(DemurrageExp existing = null)
        {
            InitializeComponent();
            var vm = new DemurrageExpViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
