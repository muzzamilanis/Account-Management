using System;
using System.Collections.Generic;
using System.Data;
using AMS.Core.Entities;

namespace AMS.Core.Interfaces
{
    public interface IDatabaseService
    {
        string GenerateDatabaseId();
        string GetConnectionString();

        double GetExchangeRate();
        void SaveExchangeRate(double rate, string date);
        void RebuildIndices();

        IEnumerable<Account> GetAccounts(string filter = "");
        IEnumerable<Account> GetAccountsByType(string type);
        void AddAccount(Account acc);
//         void UpdateAccountBalance(long accountId, double amount, bool isCredit);
        
//         void AddAcToAcEntry(string debitAcc, string creditAcc, double amount, string detail, DateTime date);
//         void AddMiscExpense(string chassis, DateTime date, double expAmount, string detail, string paidByAcc);
//         void AddDutyPayment(string chassis, DateTime date, double dutyAmount, string detail, string paidByAcc);
//         void AddOfficeExpense(DateTime date, string detail, double amount, string paidByAcc);
//         void AddReceipt(string acc, DateTime date, double amount, string detail);
//         void AddYenPayment(DateTime date, string detail, double amountYen, double convRate, double calcPkr, string paidByAcc);
//         void AddPartyPayment(string partyName, DateTime date, double amount, string detail, string paidByAcc);
//         void AddAgentPayment(string agentName, DateTime date, double amount, string detail, string paidByAcc);
//         void AddProfitWithdrawal(string partnerName, DateTime date, double amount, string detail, string paidByAcc);

//         IEnumerable<Stock> GetStocks(string statusFilter = "", string searchFilter = "");
        void AddStock(Stock s);
        void UpdateStock(Stock s);

        IEnumerable<Customer> GetCustomers(string filter = "");
        IEnumerable<string> GetCustomerNames();
        void AddCustomer(Customer c);
        void UpdateCustomer(Customer c);

        IEnumerable<Agent> GetAgents(string filter = "");
        IEnumerable<string> GetAgentNames();
        void AddAgent(Agent a);
        void UpdateAgent(Agent a);

        IEnumerable<Sale> GetSales();
        void AddSale(Sale s);

        double GetTotalProfit();
        double GetTotalYenPayable();
        DataTable GetReportData(string reportType, DateTime fromDate, DateTime toDate);
    }
}

