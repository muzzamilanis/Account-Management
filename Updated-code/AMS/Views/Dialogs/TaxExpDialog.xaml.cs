using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class TaxExpDialog : Window
    {
        public TaxExpDialog(TaxExp existing = null)
        {
            InitializeComponent();
            var vm = new TaxExpViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
