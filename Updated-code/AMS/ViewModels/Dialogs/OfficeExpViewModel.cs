using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class OfficeExpViewModel : ViewModelBase
    {
        private OfficeExp _exp = new OfficeExp();
        public OfficeExp Exp { get => _exp; set => SetField(ref _exp, value); }
        public List<string> Accounts { get; } = new List<string>();
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Exp.OfficeExpPaidBy = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public OfficeExpViewModel()
        {
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            if (Accounts.Count > 0) SelectedAccount = Accounts[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (Exp.OfficeExpAmount <= 0) { MessageBox.Show("Enter expense amount."); return; }
            DatabaseService.Instance.AddOfficeExp(Exp);
            DatabaseService.Instance.DebitAccount(Exp.OfficeExpPaidBy, Exp.OfficeExpAmount);
            CloseAction?.Invoke(true);
        }
    }
}
