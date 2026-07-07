using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class AddAccountViewModel : ViewModelBase
    {
        private Account _account;
        public Account Account { get => _account; set => SetField(ref _account, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Account" : "Add New Account";
        public static string[] AccountTypes { get; } = { "Cash", "Bank" };
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        private string _selectedAccountType;
        public string SelectedAccountType
        {
            get => _selectedAccountType;
            set
            {
                _selectedAccountType = value;
                Account.AccountType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsBankVisible));
            }
        }
        public bool IsBankVisible => SelectedAccountType == "Bank";

        public AddAccountViewModel(Account existing = null)
        {
            IsEdit = existing != null;
            Account = existing != null ? new Account
            {
                RowId = existing.RowId, AccountDate = existing.AccountDate, AccountType = existing.AccountType,
                AccountName = existing.AccountName, AccountNumber = existing.AccountNumber, AccountTitle = existing.AccountTitle,
                BankName = existing.BankName, BankBranch = existing.BankBranch, OpeningBalance = existing.OpeningBalance
            } : new Account();
            _selectedAccountType = Account.AccountType;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Account.AccountName))
            { MessageBox.Show("Account name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (IsEdit) DatabaseService.Instance.UpdateAccount(Account);
            else DatabaseService.Instance.AddAccount(Account);
            CloseAction?.Invoke(true);
        }
    }
}
