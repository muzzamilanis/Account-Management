using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class PaymentAgentDialog : Window
    {
        public PaymentAgentDialog()
        {
            InitializeComponent();
            var vm = new PaymentAgentViewModel();
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
