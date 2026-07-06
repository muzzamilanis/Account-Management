using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class PaymentPkrDialog : Window
    {
        public PaymentPkrDialog(bool isYen = false)
        {
            InitializeComponent();
            var vm = new PaymentPkrViewModel(isYen);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
