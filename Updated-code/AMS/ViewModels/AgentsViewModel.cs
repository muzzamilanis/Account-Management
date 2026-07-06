using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using AMS.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AMS.ViewModels
{
    public class AgentsViewModel : ViewModelBase
    {
        public ObservableCollection<Agent> Agents { get; } = new ObservableCollection<Agent>();
        private Agent _selected;
        public Agent SelectedAgent { get => _selected; set => SetField(ref _selected, value); }
        private string _filter;
        public string FilterText { get => _filter; set { SetField(ref _filter, value); Load(); } }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand RefreshCommand { get; }

        public AgentsViewModel()
        {
            AddCommand = new RelayCommand(OpenAdd);
            EditCommand = new RelayCommand(() => OpenEdit(SelectedAgent), () => SelectedAgent != null);
            RefreshCommand = new RelayCommand(Load);
        }

        public void Load()
        {
            Agents.Clear();
            foreach (var a in DatabaseService.Instance.GetAgents(FilterText ?? ""))
                Agents.Add(a);
        }

        private void OpenAdd() { var d = new AddAgentDialog(null); if (d.ShowDialog() == true) Load(); }
        private void OpenEdit(Agent a) { if (a == null) return; var d = new AddAgentDialog(a); if (d.ShowDialog() == true) Load(); }
    }
}
