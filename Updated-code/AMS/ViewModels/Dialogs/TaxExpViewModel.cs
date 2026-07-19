using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class TaxExpViewModel : ViewModelBase
    {
        private TaxExp _exp;
        public TaxExp Exp { get => _exp; set => SetField(ref _exp, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Tax Expense" : "Tax Expense";
        public List<string> Chassis { get; } = new List<string>();
        public List<string> Accounts { get; } = new List<string>();
        private string _selChassis;
        public string SelectedChassis { get => _selChassis; set { SetField(ref _selChassis, value); Exp.Chassis = value; } }
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Exp.TaxExpPaidBy = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        private readonly TaxExp _original;

        public TaxExpViewModel(TaxExp existing = null)
        {
            IsEdit = existing != null;
            _original = existing;
            Exp = existing != null ? new TaxExp
            {
                RowId = existing.RowId, Chassis = existing.Chassis, TaxExpDate = existing.TaxExpDate,
                TaxExpAmount = existing.TaxExpAmount, TaxExpDetail = existing.TaxExpDetail, TaxExpPaidBy = existing.TaxExpPaidBy
            } : new TaxExp();

            Chassis.AddRange(DatabaseService.Instance.GetInStockChassisNumbers());
            if (existing != null && !string.IsNullOrEmpty(existing.Chassis) && !Chassis.Contains(existing.Chassis))
                Chassis.Insert(0, existing.Chassis);
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            _selChassis = Exp.Chassis;
            _selAccount = Exp.TaxExpPaidBy;
            if (string.IsNullOrEmpty(_selChassis) && Chassis.Count > 0) SelectedChassis = Chassis[0];
            if (string.IsNullOrEmpty(_selAccount) && Accounts.Count > 0) SelectedAccount = Accounts[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Exp.Chassis)) { MessageBox.Show("Select chassis."); return; }
            if (string.IsNullOrEmpty(Exp.TaxExpPaidBy)) { MessageBox.Show("Select an account."); return; }
            if (Exp.TaxExpAmount <= 0) { MessageBox.Show("Enter expense amount."); return; }

            if (IsEdit)
            {
                DatabaseService.Instance.CreditAccountWithLedger(_original.TaxExpPaidBy, _original.TaxExpAmount, _original.TaxExpDate, $"Reversal (edit): Tax Expense (Chassis {_original.Chassis})");
                DatabaseService.Instance.AdjustStockTax(_original.Chassis, -_original.TaxExpAmount);
                DatabaseService.Instance.UpdateTaxExp(Exp);
            }
            else
            {
                DatabaseService.Instance.AddTaxExp(Exp);
            }
            DatabaseService.Instance.DebitAccountWithLedger(Exp.TaxExpPaidBy, Exp.TaxExpAmount, Exp.TaxExpDate, $"Tax Expense (Chassis {Exp.Chassis}): {Exp.TaxExpDetail}");
            DatabaseService.Instance.AdjustStockTax(Exp.Chassis, Exp.TaxExpAmount);
            CloseAction?.Invoke(true);
        }
    }
}
