using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class PurchaseAutoViewModel : ViewModelBase
    {
        private Stock _stock;
        public Stock Stock { get => _stock; set => SetField(ref _stock, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Stock" : "Purchase Auto";
        public List<string> Accounts { get; } = new List<string>();
        public ObservableCollection<string> Agents { get; } = new ObservableCollection<string>();
        private string _selectedAccount;
        public string SelectedAccount { get => _selectedAccount; set => SetField(ref _selectedAccount, value); }
        private string _selectedAgent;
        public string SelectedAgent { get => _selectedAgent; set => SetField(ref _selectedAgent, value); }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public PurchaseAutoViewModel(Stock existing = null)
        {
            IsEdit = existing != null;
            Stock = existing != null ? new Stock { RowId = existing.RowId, Date = existing.Date, Chassis = existing.Chassis, Model = existing.Model, Color = existing.Color, PriceYen = existing.PriceYen, Rate = existing.Rate, Duty = existing.Duty, MiscExpense = existing.MiscExpense, Comments = existing.Comments, PaidYen = existing.PaidYen, PaidAmount = existing.PaidAmount, Status = existing.Status } : new Stock();
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            foreach (var a in DatabaseService.Instance.GetAgentNames()) Agents.Add(a);
            SelectedAccount = Accounts.Count > 0 ? Accounts[0] : null;
            SelectedAgent = Agents.Count > 0 ? Agents[0] : null;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Stock.Chassis)) { MessageBox.Show("Chassis number is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            double rate = SettingsService.Instance.ExchangeRate;
            if (rate <= 0) rate = Stock.Rate > 0 ? Stock.Rate : 1;
            Stock.Rate = rate;
            Stock.PricePkr = Stock.PriceYen * rate;
            Stock.Cost = Stock.PricePkr + Stock.Duty + Stock.MiscExpense;
            Stock.PaidAmount = Stock.PaidYen * rate;
            if (!IsEdit) Stock.Status = "InStock";
            if (IsEdit) DatabaseService.Instance.UpdateStock(Stock);
            else
            {
                DatabaseService.Instance.AddStock(Stock);
                double accountDebit = Stock.PaidAmount + Stock.MiscExpense;
                if (accountDebit > 0 && !string.IsNullOrEmpty(SelectedAccount))
                    DatabaseService.Instance.DebitAccount(SelectedAccount, accountDebit);
                if (Stock.MiscExpense > 0 && !string.IsNullOrEmpty(SelectedAccount))
                    DatabaseService.Instance.AddMiscExp(new MiscExp
                    {
                        Chassis = Stock.Chassis, MiscExpDate = Stock.Date, MiscExpAmount = Stock.MiscExpense,
                        MiscExpDetail = $"Expense at purchase: {Stock.Chassis}", MiscExpPaidBy = SelectedAccount
                    });
                if (Stock.Duty > 0 && !string.IsNullOrEmpty(SelectedAgent))
                {
                    DatabaseService.Instance.AddDutyExp(new DutyExp
                    {
                        Chassis = Stock.Chassis, DutyExpDate = Stock.Date, DutyExpAmount = Stock.Duty,
                        DutyExpDetail = $"Expense at purchase: {Stock.Chassis}", DutyExpPaidBy = SelectedAccount, DutyExpAgent = SelectedAgent
                    });
                    DatabaseService.Instance.AdjustAgentPayable(SelectedAgent, -Stock.Duty);
                }
            }
            CloseAction?.Invoke(true);
        }
    }
}
