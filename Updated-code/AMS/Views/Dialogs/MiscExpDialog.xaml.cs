using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class MiscExpDialog : Window
    {
        public MiscExpDialog()
        {
            InitializeComponent();
            var vm = new MiscExpViewModel();
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
