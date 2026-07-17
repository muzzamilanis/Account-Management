using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class PayInstallmentDialog : Window
    {
        public PayInstallmentDialog(Installment installment)
        {
            InitializeComponent();
            var vm = new PayInstallmentViewModel(installment);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
