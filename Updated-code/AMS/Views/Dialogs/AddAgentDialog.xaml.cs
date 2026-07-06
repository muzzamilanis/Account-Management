using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class AddAgentDialog : Window
    {
        public AddAgentDialog(Agent existing = null)
        {
            InitializeComponent();
            var vm = new AddAgentViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
