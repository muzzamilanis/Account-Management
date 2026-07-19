using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class MiscExpDialog : Window
    {
        public MiscExpDialog(MiscExp existing = null)
        {
            InitializeComponent();
            var vm = new MiscExpViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
