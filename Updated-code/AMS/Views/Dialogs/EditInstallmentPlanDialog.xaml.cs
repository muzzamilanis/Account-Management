using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class EditInstallmentPlanDialog : Window
    {
        public EditInstallmentPlanDialog(Sale sale)
        {
            InitializeComponent();
            var vm = new EditInstallmentPlanViewModel(sale);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
