using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class DutyExpDialog : Window
    {
        public DutyExpDialog(DutyExp existing = null)
        {
            InitializeComponent();
            var vm = new DutyExpViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }

        private void BtnAddAgent_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddAgentDialog { Owner = this };
            if (dlg.ShowDialog() == true)
            {
                var vm = (DutyExpViewModel)DataContext;
                var name = ((AddAgentViewModel)dlg.DataContext).Agent.Name;
                vm.Agents.Add(name);
                vm.SelectedAgent = name;
            }
        }
    }
}
