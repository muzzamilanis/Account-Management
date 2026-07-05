using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class DutyExpDialog : Window
    {
        public DutyExpDialog()
        {
            InitializeComponent();
            var vm = new DutyExpViewModel();
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
