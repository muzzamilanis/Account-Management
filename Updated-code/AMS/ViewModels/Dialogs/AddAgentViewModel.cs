using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class AddAgentViewModel : ViewModelBase
    {
        private Agent _agent;
        public Agent Agent { get => _agent; set => SetField(ref _agent, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Agent" : "Add New Agent";
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public AddAgentViewModel(Agent existing = null)
        {
            IsEdit = existing != null;
            Agent = existing != null ? new Agent { RowId = existing.RowId, Date = existing.Date, Name = existing.Name, CNIC = existing.CNIC, Phone = existing.Phone, Address = existing.Address } : new Agent();
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Agent.Name)) { MessageBox.Show("Agent name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (IsEdit) DatabaseService.Instance.UpdateAgent(Agent);
            else DatabaseService.Instance.AddAgent(Agent);
            CloseAction?.Invoke(true);
        }
    }
}
