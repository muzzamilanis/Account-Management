using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class PurchaseAutoDialog : Window
    {
        public PurchaseAutoDialog(Stock existing = null)
        {
            InitializeComponent();
            var vm = new PurchaseAutoViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }

        private void BtnAddAgent_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddAgentDialog { Owner = this };
            if (dlg.ShowDialog() == true)
            {
                var vm = (PurchaseAutoViewModel)DataContext;
                var name = ((ViewModels.Dialogs.AddAgentViewModel)dlg.DataContext).Agent.Name;
                vm.Agents.Add(name);
                vm.SelectedAgent = name;
            }
        }
    }
}
