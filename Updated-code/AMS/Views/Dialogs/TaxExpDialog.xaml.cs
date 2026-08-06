using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class TaxExpDialog : Window
    {
        public TaxExpDialog()
        {
            InitializeComponent();
            var vm = new TaxExpViewModel();
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
