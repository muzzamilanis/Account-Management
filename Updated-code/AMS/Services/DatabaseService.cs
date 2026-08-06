using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using AMS.Models;

namespace AMS.Services
{
    public class DatabaseService
    {
        private static DatabaseService _instance;
        public static DatabaseService Instance => _instance ?? (_instance = new DatabaseService());

        private SQLiteConnection _connection;
        public bool IsConnected => _connection != null && _connection.State == ConnectionState.Open;

        private DatabaseService() { }

        public void CreateDatabase(string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            SQLiteConnection.CreateFile(filePath);
            OpenDatabase(filePath);
            CreateTables();
        }

        public void OpenDatabase(string filePath)
        {
            CloseConnection();
            var builder = new SQLiteConnectionStringBuilder
            {
                DataSource = filePath,
                Version = 3
            };
            _connection = new SQLiteConnection(builder.ConnectionString);
            _connection.Open();
            CreateTables();
        }

        public void CloseConnection()
        {
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open) _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        private void CreateTables()
        {
            string schema = @"
                CREATE TABLE IF NOT EXISTS Account (RowId INTEGER PRIMARY KEY AUTOINCREMENT, AccountDate DATETIME, AccountType TEXT, AccountName TEXT, AccountNumber TEXT, AccountTitle TEXT, BankName TEXT, BankBranch TEXT, OpeningBalance REAL, CurrentBalance REAL);
                CREATE TABLE IF NOT EXISTS Stock (RowId INTEGER PRIMARY KEY AUTOINCREMENT, Date DATETIME, Chassis TEXT, Model TEXT, Color TEXT, PriceYen REAL, Rate REAL, PricePkr REAL, Duty REAL, MiscExpense REAL, Cost REAL, Status TEXT, PaidYen REAL, PaidAmount REAL, Comments TEXT);
                CREATE TABLE IF NOT EXISTS Customer (RowId INTEGER PRIMARY KEY AUTOINCREMENT, Date DATETIME, Title TEXT, Name TEXT, CNIC TEXT, Phone TEXT, Address TEXT, PaymentReceived REAL, PaymentReceivable REAL, PaymentPaid REAL);
                CREATE TABLE IF NOT EXISTS Agent (RowId INTEGER PRIMARY KEY AUTOINCREMENT, Date DATETIME, Name TEXT, CNIC TEXT, Phone TEXT, Address TEXT, PaymentReceivable REAL, PaymentPaid REAL);
                CREATE TABLE IF NOT EXISTS Sale (RowId INTEGER PRIMARY KEY AUTOINCREMENT, SaleDate DATETIME, SaleChassis TEXT, SaleCustomer TEXT, SalePrice REAL, SaleAmountReceived REAL, PaymentReceivedIn TEXT);
                CREATE TABLE IF NOT EXISTS Payment (RowId INTEGER PRIMARY KEY AUTOINCREMENT, PaymentDate DATETIME, PaymentAmountYen REAL, PaymentExcRate REAL, PaymentAmountPkr REAL, PaymentDetail TEXT, PaidFrom TEXT);
                CREATE TABLE IF NOT EXISTS PaymentPkr (RowId INTEGER PRIMARY KEY AUTOINCREMENT, PaymentDate DATETIME, PaymentAmount REAL, PaymentDetail TEXT, PaidFrom TEXT, PaidTo TEXT);
                CREATE TABLE IF NOT EXISTS PaymentAgent (RowId INTEGER PRIMARY KEY AUTOINCREMENT, PaymentDate DATETIME, PaymentAmount REAL, PaymentDetail TEXT, PaidFrom TEXT, PaidTo TEXT);
                CREATE TABLE IF NOT EXISTS Receipt (RowId INTEGER PRIMARY KEY AUTOINCREMENT, ReceiptDate DATETIME, ReceiptAmount REAL, ReceiptDetail TEXT, ReceivedIn TEXT, ReceivedFrom TEXT);
                CREATE TABLE IF NOT EXISTS MiscExp (RowId INTEGER PRIMARY KEY AUTOINCREMENT, Chassis TEXT, MiscExpDate DATETIME, MiscExpAmount REAL, MiscExpDetail TEXT, MiscExpPaidBy TEXT);
                CREATE TABLE IF NOT EXISTS DutyExp (RowId INTEGER PRIMARY KEY AUTOINCREMENT, Chassis TEXT, DutyExpDate DATETIME, DutyExpAmount REAL, DutyExpDetail TEXT, DutyExpPaidBy TEXT, DutyExpAgent TEXT);
                CREATE TABLE IF NOT EXISTS DemurrageExp (RowId INTEGER PRIMARY KEY AUTOINCREMENT, Chassis TEXT, DemurrageExpDate DATETIME, DemurrageExpAmount REAL, DemurrageExpDetail TEXT, DemurrageExpPaidBy TEXT);
                CREATE TABLE IF NOT EXISTS NoPlateExp (RowId INTEGER PRIMARY KEY AUTOINCREMENT, Chassis TEXT, NoPlateExpDate DATETIME, NoPlateExpAmount REAL, NoPlateExpDetail TEXT, NoPlateExpPaidBy TEXT);
                CREATE TABLE IF NOT EXISTS CommissionExp (RowId INTEGER PRIMARY KEY AUTOINCREMENT, Chassis TEXT, CommissionExpDate DATETIME, CommissionExpAmount REAL, CommissionExpDetail TEXT, CommissionExpPaidBy TEXT);
                CREATE TABLE IF NOT EXISTS TaxExp (RowId INTEGER PRIMARY KEY AUTOINCREMENT, Chassis TEXT, TaxExpDate DATETIME, TaxExpAmount REAL, TaxExpDetail TEXT, TaxExpPaidBy TEXT);
                CREATE TABLE IF NOT EXISTS OfficeExp (RowId INTEGER PRIMARY KEY AUTOINCREMENT, OfficeExpDate DATETIME, OfficeExpAmount REAL, OfficeExpDetail TEXT, OfficeExpPaidBy TEXT);
                CREATE TABLE IF NOT EXISTS OfficeAccount (RowId INTEGER PRIMARY KEY AUTOINCREMENT, Date DATETIME, Amount REAL, Detail TEXT, CreditFrom TEXT, DebitTo TEXT, LedgerRowId INTEGER);
                CREATE TABLE IF NOT EXISTS Ledger (RowId INTEGER PRIMARY KEY AUTOINCREMENT, Date DATETIME, Amount REAL, Detail TEXT, Account TEXT);
                CREATE TABLE IF NOT EXISTS Installment (RowId INTEGER PRIMARY KEY AUTOINCREMENT, SaleRowId INTEGER, InstallmentNumber INTEGER, DueDate DATETIME, Amount REAL, IsPaid INTEGER DEFAULT 0, PaidDate DATETIME, PaidAmount REAL, PaidInAccount TEXT);
            ";
            using (var cmd = new SQLiteCommand(schema, _connection))
            {
                cmd.ExecuteNonQuery();
            }
            EnsureColumn("Sale", "CreditDays", "INTEGER DEFAULT 0");
            EnsureColumn("Sale", "ReminderDaysBefore", "INTEGER DEFAULT 0");
            EnsureColumn("Sale", "InstallmentMonths", "INTEGER DEFAULT 0");
            EnsureColumn("Account", "IncludeInProfit", "INTEGER DEFAULT 1");
            EnsureColumn("Stock", "Demurrage", "REAL DEFAULT 0");
            EnsureColumn("Stock", "NoPlate", "REAL DEFAULT 0");
            EnsureColumn("Stock", "Commission", "REAL DEFAULT 0");
            EnsureColumn("Stock", "Tax", "REAL DEFAULT 0");
        }

        // Lightweight migration helper: adds a column to an existing table if it isn't
        // already there. CREATE TABLE IF NOT EXISTS only handles brand-new tables, not
        // new columns on a table that already exists in an older database file.
        private void EnsureColumn(string table, string column, string typeDef)
        {
            var dt = ExecuteQuery($"PRAGMA table_info({table})");
            foreach (DataRow r in dt.Rows)
                if (string.Equals(r["name"]?.ToString(), column, StringComparison.OrdinalIgnoreCase))
                    return;
            ExecuteNonQuery($"ALTER TABLE {table} ADD COLUMN {column} {typeDef}");
        }

        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            var dt = new DataTable();
            if (!IsConnected) return dt;
            using (var cmd = new SQLiteCommand(query, _connection))
            {
                if (parameters != null)
                    foreach (var p in parameters) cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
                using (var da = new SQLiteDataAdapter(cmd)) da.Fill(dt);
            }
            return dt;
        }

        public void ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            if (!IsConnected) return;
            using (var cmd = new SQLiteCommand(query, _connection))
            {
                if (parameters != null)
                    foreach (var p in parameters) cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        // --- MAPPERS ---
        private T GetValue<T>(DataRow row, string column, T defaultValue = default(T))
        {
            if (row.Table.Columns.Contains(column) && row[column] != DBNull.Value)
                return (T)Convert.ChangeType(row[column], typeof(T));
            return defaultValue;
        }

        // --- ACCOUNTS ---
        public List<string> GetAccountNames()
        {
            var dt = ExecuteQuery("SELECT AccountName FROM Account ORDER BY AccountName");
            var res = new List<string>();
            foreach (DataRow r in dt.Rows) res.Add(GetValue<string>(r, "AccountName"));
            return res;
        }
        
        public IEnumerable<Account> GetAccounts(string filter = "")
        {
            var dt = ExecuteQuery("SELECT * FROM Account ORDER BY AccountName");
            var res = new List<Account>();
            foreach (DataRow r in dt.Rows)
            {
                if (!string.IsNullOrEmpty(filter) && !GetValue<string>(r, "AccountName", "").ToLower().Contains(filter.ToLower())) continue;
                res.Add(new Account {
                    RowId = GetValue<long>(r, "RowId"), AccountDate = GetValue<DateTime>(r, "AccountDate"),
                    AccountType = GetValue<string>(r, "AccountType"), AccountName = GetValue<string>(r, "AccountName"),
                    AccountNumber = GetValue<string>(r, "AccountNumber"), AccountTitle = GetValue<string>(r, "AccountTitle"),
                    BankName = GetValue<string>(r, "BankName"), BankBranch = GetValue<string>(r, "BankBranch"),
                    OpeningBalance = GetValue<double>(r, "OpeningBalance"), CurrentBalance = GetValue<double>(r, "CurrentBalance"),
                    IncludeInProfit = GetValue<int>(r, "IncludeInProfit", 1) != 0
                });
            }
            return res;
        }
        
        public void AddAccount(Account a)
        {
            ExecuteNonQuery("INSERT INTO Account (AccountDate, AccountType, AccountName, AccountNumber, AccountTitle, BankName, BankBranch, OpeningBalance, CurrentBalance, IncludeInProfit) VALUES (@d, @ty, @n, @no, @ti, @bn, @bb, @ob, @cb, @ip)",
                new Dictionary<string, object> { {"@d", a.AccountDate}, {"@ty", a.AccountType}, {"@n", a.AccountName}, {"@no", a.AccountNumber}, {"@ti", a.AccountTitle}, {"@bn", a.BankName}, {"@bb", a.BankBranch}, {"@ob", a.OpeningBalance}, {"@cb", a.CurrentBalance}, {"@ip", a.IncludeInProfit ? 1 : 0} });
        }

        public void UpdateAccount(Account a)
        {
            ExecuteNonQuery("UPDATE Account SET AccountDate=@d, AccountType=@ty, AccountName=@n, AccountNumber=@no, AccountTitle=@ti, BankName=@bn, BankBranch=@bb, OpeningBalance=@ob, CurrentBalance=@cb, IncludeInProfit=@ip WHERE RowId=@id",
                new Dictionary<string, object> { {"@id", a.RowId}, {"@d", a.AccountDate}, {"@ty", a.AccountType}, {"@n", a.AccountName}, {"@no", a.AccountNumber}, {"@ti", a.AccountTitle}, {"@bn", a.BankName}, {"@bb", a.BankBranch}, {"@ob", a.OpeningBalance}, {"@cb", a.CurrentBalance}, {"@ip", a.IncludeInProfit ? 1 : 0} });
        }
        
        public void DebitAccount(string accName, double amount)
        {
            ExecuteNonQuery("UPDATE Account SET CurrentBalance = CurrentBalance - @amount WHERE AccountName = @name",
                new Dictionary<string, object> { {"@amount", amount}, {"@name", accName} });
        }

        public void CreditAccount(string accName, double amount)
        {
            ExecuteNonQuery("UPDATE Account SET CurrentBalance = CurrentBalance + @amount WHERE AccountName = @name",
                new Dictionary<string, object> { {"@amount", amount}, {"@name", accName} });
        }

        public void AddLedgerEntry(DateTime date, double amount, string detail, string account)
        {
            ExecuteNonQuery("INSERT INTO Ledger (Date, Amount, Detail, Account) VALUES (@d, @a, @det, @acc)",
                new Dictionary<string, object> { {"@d", date}, {"@a", amount}, {"@det", detail}, {"@acc", account} });
        }

        // Debit = money leaving the account; logged as a negative ledger amount (shows in the
        // Credit column of the account statement, matching standard asset-account convention).
        public void DebitAccountWithLedger(string accName, double amount, DateTime date, string detail)
        {
            DebitAccount(accName, amount);
            AddLedgerEntry(date, -amount, detail, accName);
        }

        // Credit = money entering the account; logged as a positive ledger amount (shows in the
        // Debit column of the account statement).
        public void CreditAccountWithLedger(string accName, double amount, DateTime date, string detail)
        {
            CreditAccount(accName, amount);
            AddLedgerEntry(date, amount, detail, accName);
        }

        public DataTable GetAccountStatement(string accountName, DateTime fromDate, DateTime toDate)
        {
            var table = new DataTable();
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Detail", typeof(string));
            table.Columns.Add("Debit", typeof(double));
            table.Columns.Add("Credit", typeof(double));
            table.Columns.Add("Balance", typeof(double));

            double openingBalance = 0;
            var acctRow = ExecuteQuery("SELECT OpeningBalance FROM Account WHERE AccountName = @name",
                new Dictionary<string, object> { {"@name", accountName} });
            if (acctRow.Rows.Count > 0) openingBalance = GetValue<double>(acctRow.Rows[0], "OpeningBalance");

            double priorSum = 0;
            var priorRows = ExecuteQuery("SELECT Amount FROM Ledger WHERE Account = @name AND Date < @from",
                new Dictionary<string, object> { {"@name", accountName}, {"@from", fromDate.ToString("yyyy-MM-dd")} });
            foreach (DataRow r in priorRows.Rows) priorSum += GetValue<double>(r, "Amount");

            double runningBalance = openingBalance + priorSum;
            var openingRow = table.NewRow();
            openingRow["Date"] = fromDate;
            openingRow["Detail"] = $"Opening Balance of {accountName}";
            openingRow["Debit"] = DBNull.Value;
            openingRow["Credit"] = DBNull.Value;
            openingRow["Balance"] = runningBalance;
            table.Rows.Add(openingRow);

            var entries = ExecuteQuery(
                "SELECT Date, Amount, Detail FROM Ledger WHERE Account = @name AND Date >= @from AND Date <= @to AND Amount <> 0 ORDER BY Date ASC, RowId ASC",
                new Dictionary<string, object> { {"@name", accountName}, {"@from", fromDate.ToString("yyyy-MM-dd")}, {"@to", toDate.ToString("yyyy-MM-dd")} });
            foreach (DataRow r in entries.Rows)
            {
                double amount = GetValue<double>(r, "Amount");
                runningBalance += amount;
                var row = table.NewRow();
                row["Date"] = GetValue<DateTime>(r, "Date");
                row["Detail"] = GetValue<string>(r, "Detail");
                row["Debit"] = amount > 0 ? (object)amount : DBNull.Value;
                row["Credit"] = amount < 0 ? (object)(-amount) : DBNull.Value;
                row["Balance"] = runningBalance;
                table.Rows.Add(row);
            }
            return table;
        }

        public void AdjustAgentPayable(string agentName, double delta)
        {
            ExecuteNonQuery("UPDATE Agent SET PaymentReceivable = PaymentReceivable + @delta WHERE Name = @name",
                new Dictionary<string, object> { {"@delta", delta}, {"@name", agentName} });
        }

        public void RecordAgentPayment(string agentName, double amount)
        {
            ExecuteNonQuery("UPDATE Agent SET PaymentPaid = PaymentPaid + @amt, PaymentReceivable = PaymentReceivable + @amt WHERE Name = @name",
                new Dictionary<string, object> { {"@amt", amount}, {"@name", agentName} });
        }

        public void RecordCustomerPayment(string customerName, double amount)
        {
            ExecuteNonQuery("UPDATE Customer SET PaymentPaid = PaymentPaid + @amt, PaymentReceivable = PaymentReceivable + @amt WHERE (Title || ' ' || Name) = @name",
                new Dictionary<string, object> { {"@amt", amount}, {"@name", customerName} });
        }

        public void RecordCustomerReceipt(string customerName, double amount)
        {
            ExecuteNonQuery("UPDATE Customer SET PaymentReceived = PaymentReceived + @amt, PaymentReceivable = PaymentReceivable - @amt WHERE (Title || ' ' || Name) = @name",
                new Dictionary<string, object> { {"@amt", amount}, {"@name", customerName} });
        }

        public void RecordCustomerSale(string customerName, double amountReceived, double balance)
        {
            ExecuteNonQuery("UPDATE Customer SET PaymentReceived = PaymentReceived + @recv, PaymentReceivable = PaymentReceivable + @bal WHERE (Title || ' ' || Name) = @name",
                new Dictionary<string, object> { {"@recv", amountReceived}, {"@bal", balance}, {"@name", customerName} });
        }

        // Every Adjust* method below keeps Stock.Cost (the figure profit calculations read) in sync
        // with the underlying expense column in the same statement — a post-purchase expense entry
        // (e.g. from the Clearance/Demurrage/etc. tabs) must move Cost immediately, not just at the
        // next Purchase-form edit, otherwise "Total Profit Available" silently goes stale.
        public void AdjustStockDuty(string chassis, double delta)
        {
            ExecuteNonQuery("UPDATE Stock SET Duty = Duty + @delta, Cost = Cost + @delta WHERE Chassis = @chassis",
                new Dictionary<string, object> { {"@delta", delta}, {"@chassis", chassis} });
        }

        public void AdjustStockMiscExpense(string chassis, double delta)
        {
            ExecuteNonQuery("UPDATE Stock SET MiscExpense = MiscExpense + @delta, Cost = Cost + @delta WHERE Chassis = @chassis",
                new Dictionary<string, object> { {"@delta", delta}, {"@chassis", chassis} });
        }

        public void AdjustStockDemurrage(string chassis, double delta)
        {
            ExecuteNonQuery("UPDATE Stock SET Demurrage = Demurrage + @delta, Cost = Cost + @delta WHERE Chassis = @chassis",
                new Dictionary<string, object> { {"@delta", delta}, {"@chassis", chassis} });
        }

        public void AdjustStockNoPlate(string chassis, double delta)
        {
            ExecuteNonQuery("UPDATE Stock SET NoPlate = NoPlate + @delta, Cost = Cost + @delta WHERE Chassis = @chassis",
                new Dictionary<string, object> { {"@delta", delta}, {"@chassis", chassis} });
        }

        public void AdjustStockCommission(string chassis, double delta)
        {
            ExecuteNonQuery("UPDATE Stock SET Commission = Commission + @delta, Cost = Cost + @delta WHERE Chassis = @chassis",
                new Dictionary<string, object> { {"@delta", delta}, {"@chassis", chassis} });
        }

        public void AdjustStockTax(string chassis, double delta)
        {
            ExecuteNonQuery("UPDATE Stock SET Tax = Tax + @delta, Cost = Cost + @delta WHERE Chassis = @chassis",
                new Dictionary<string, object> { {"@delta", delta}, {"@chassis", chassis} });
        }

        public void MarkStockSold(string chassis)
        {
            ExecuteNonQuery("UPDATE Stock SET Status = 'Sold' WHERE Chassis = @chassis",
                new Dictionary<string, object> { {"@chassis", chassis} });
        }

        // --- TRANSACTIONS ---
        public void AddOfficeAccountTransfer(OfficeAccount o)
        {
            ExecuteNonQuery("INSERT INTO OfficeAccount (Date, Amount, Detail, CreditFrom, DebitTo) VALUES (@d, @a, @det, @c, @deb)",
                new Dictionary<string, object> { {"@d", o.Date}, {"@a", o.Amount}, {"@det", o.Detail}, {"@c", o.CreditFrom}, {"@deb", o.DebitTo} });
        }
        public IEnumerable<OfficeAccount> GetOfficeAccounts()
        {
            var dt = ExecuteQuery("SELECT * FROM OfficeAccount ORDER BY Date DESC");
            var res = new List<OfficeAccount>();
            foreach (DataRow r in dt.Rows) res.Add(new OfficeAccount {
                RowId = GetValue<long>(r, "RowId"), Date = GetValue<DateTime>(r, "Date"), Amount = GetValue<double>(r, "Amount"),
                Detail = GetValue<string>(r, "Detail"), CreditFrom = GetValue<string>(r, "CreditFrom"), DebitTo = GetValue<string>(r, "DebitTo") });
            return res;
        }

        public void AddMiscExp(MiscExp m)
        {
            ExecuteNonQuery("INSERT INTO MiscExp (Chassis, MiscExpDate, MiscExpAmount, MiscExpDetail, MiscExpPaidBy) VALUES (@c, @d, @a, @det, @p)",
                new Dictionary<string, object> { {"@c", m.Chassis}, {"@d", m.MiscExpDate}, {"@a", m.MiscExpAmount}, {"@det", m.MiscExpDetail}, {"@p", m.MiscExpPaidBy} });
        }
        public IEnumerable<MiscExp> GetMiscExps()
        {
            var dt = ExecuteQuery("SELECT * FROM MiscExp ORDER BY MiscExpDate DESC");
            var res = new List<MiscExp>();
            foreach (DataRow r in dt.Rows) res.Add(new MiscExp {
                RowId = GetValue<long>(r, "RowId"), Chassis = GetValue<string>(r, "Chassis"), MiscExpDate = GetValue<DateTime>(r, "MiscExpDate"),
                MiscExpAmount = GetValue<double>(r, "MiscExpAmount"), MiscExpDetail = GetValue<string>(r, "MiscExpDetail"), MiscExpPaidBy = GetValue<string>(r, "MiscExpPaidBy") });
            return res;
        }

        public void AddDutyExp(DutyExp d)
        {
            ExecuteNonQuery("INSERT INTO DutyExp (Chassis, DutyExpDate, DutyExpAmount, DutyExpDetail, DutyExpPaidBy, DutyExpAgent) VALUES (@c, @d, @a, @det, @p, @ag)",
                new Dictionary<string, object> { {"@c", d.Chassis}, {"@d", d.DutyExpDate}, {"@a", d.DutyExpAmount}, {"@det", d.DutyExpDetail}, {"@p", d.DutyExpPaidBy}, {"@ag", d.DutyExpAgent} });
        }
        public IEnumerable<DutyExp> GetDutyExps()
        {
            var dt = ExecuteQuery("SELECT * FROM DutyExp ORDER BY DutyExpDate DESC");
            var res = new List<DutyExp>();
            foreach (DataRow r in dt.Rows) res.Add(new DutyExp {
                RowId = GetValue<long>(r, "RowId"), Chassis = GetValue<string>(r, "Chassis"), DutyExpDate = GetValue<DateTime>(r, "DutyExpDate"),
                DutyExpAmount = GetValue<double>(r, "DutyExpAmount"), DutyExpDetail = GetValue<string>(r, "DutyExpDetail"), DutyExpPaidBy = GetValue<string>(r, "DutyExpPaidBy"), DutyExpAgent = GetValue<string>(r, "DutyExpAgent") });
            return res;
        }

        public void AddDemurrageExp(DemurrageExp d)
        {
            ExecuteNonQuery("INSERT INTO DemurrageExp (Chassis, DemurrageExpDate, DemurrageExpAmount, DemurrageExpDetail, DemurrageExpPaidBy) VALUES (@c, @d, @a, @det, @p)",
                new Dictionary<string, object> { {"@c", d.Chassis}, {"@d", d.DemurrageExpDate}, {"@a", d.DemurrageExpAmount}, {"@det", d.DemurrageExpDetail}, {"@p", d.DemurrageExpPaidBy} });
        }
        public IEnumerable<DemurrageExp> GetDemurrageExps()
        {
            var dt = ExecuteQuery("SELECT * FROM DemurrageExp ORDER BY DemurrageExpDate DESC");
            var res = new List<DemurrageExp>();
            foreach (DataRow r in dt.Rows) res.Add(new DemurrageExp {
                RowId = GetValue<long>(r, "RowId"), Chassis = GetValue<string>(r, "Chassis"), DemurrageExpDate = GetValue<DateTime>(r, "DemurrageExpDate"),
                DemurrageExpAmount = GetValue<double>(r, "DemurrageExpAmount"), DemurrageExpDetail = GetValue<string>(r, "DemurrageExpDetail"), DemurrageExpPaidBy = GetValue<string>(r, "DemurrageExpPaidBy") });
            return res;
        }

        public void AddNoPlateExp(NoPlateExp d)
        {
            ExecuteNonQuery("INSERT INTO NoPlateExp (Chassis, NoPlateExpDate, NoPlateExpAmount, NoPlateExpDetail, NoPlateExpPaidBy) VALUES (@c, @d, @a, @det, @p)",
                new Dictionary<string, object> { {"@c", d.Chassis}, {"@d", d.NoPlateExpDate}, {"@a", d.NoPlateExpAmount}, {"@det", d.NoPlateExpDetail}, {"@p", d.NoPlateExpPaidBy} });
        }
        public IEnumerable<NoPlateExp> GetNoPlateExps()
        {
            var dt = ExecuteQuery("SELECT * FROM NoPlateExp ORDER BY NoPlateExpDate DESC");
            var res = new List<NoPlateExp>();
            foreach (DataRow r in dt.Rows) res.Add(new NoPlateExp {
                RowId = GetValue<long>(r, "RowId"), Chassis = GetValue<string>(r, "Chassis"), NoPlateExpDate = GetValue<DateTime>(r, "NoPlateExpDate"),
                NoPlateExpAmount = GetValue<double>(r, "NoPlateExpAmount"), NoPlateExpDetail = GetValue<string>(r, "NoPlateExpDetail"), NoPlateExpPaidBy = GetValue<string>(r, "NoPlateExpPaidBy") });
            return res;
        }

        public void AddCommissionExp(CommissionExp d)
        {
            ExecuteNonQuery("INSERT INTO CommissionExp (Chassis, CommissionExpDate, CommissionExpAmount, CommissionExpDetail, CommissionExpPaidBy) VALUES (@c, @d, @a, @det, @p)",
                new Dictionary<string, object> { {"@c", d.Chassis}, {"@d", d.CommissionExpDate}, {"@a", d.CommissionExpAmount}, {"@det", d.CommissionExpDetail}, {"@p", d.CommissionExpPaidBy} });
        }
        public IEnumerable<CommissionExp> GetCommissionExps()
        {
            var dt = ExecuteQuery("SELECT * FROM CommissionExp ORDER BY CommissionExpDate DESC");
            var res = new List<CommissionExp>();
            foreach (DataRow r in dt.Rows) res.Add(new CommissionExp {
                RowId = GetValue<long>(r, "RowId"), Chassis = GetValue<string>(r, "Chassis"), CommissionExpDate = GetValue<DateTime>(r, "CommissionExpDate"),
                CommissionExpAmount = GetValue<double>(r, "CommissionExpAmount"), CommissionExpDetail = GetValue<string>(r, "CommissionExpDetail"), CommissionExpPaidBy = GetValue<string>(r, "CommissionExpPaidBy") });
            return res;
        }

        public void AddTaxExp(TaxExp d)
        {
            ExecuteNonQuery("INSERT INTO TaxExp (Chassis, TaxExpDate, TaxExpAmount, TaxExpDetail, TaxExpPaidBy) VALUES (@c, @d, @a, @det, @p)",
                new Dictionary<string, object> { {"@c", d.Chassis}, {"@d", d.TaxExpDate}, {"@a", d.TaxExpAmount}, {"@det", d.TaxExpDetail}, {"@p", d.TaxExpPaidBy} });
        }
        public IEnumerable<TaxExp> GetTaxExps()
        {
            var dt = ExecuteQuery("SELECT * FROM TaxExp ORDER BY TaxExpDate DESC");
            var res = new List<TaxExp>();
            foreach (DataRow r in dt.Rows) res.Add(new TaxExp {
                RowId = GetValue<long>(r, "RowId"), Chassis = GetValue<string>(r, "Chassis"), TaxExpDate = GetValue<DateTime>(r, "TaxExpDate"),
                TaxExpAmount = GetValue<double>(r, "TaxExpAmount"), TaxExpDetail = GetValue<string>(r, "TaxExpDetail"), TaxExpPaidBy = GetValue<string>(r, "TaxExpPaidBy") });
            return res;
        }

        public void AddOfficeExp(OfficeExp o)
        {
            ExecuteNonQuery("INSERT INTO OfficeExp (OfficeExpDate, OfficeExpAmount, OfficeExpDetail, OfficeExpPaidBy) VALUES (@d, @a, @det, @p)",
                new Dictionary<string, object> { {"@d", o.OfficeExpDate}, {"@a", o.OfficeExpAmount}, {"@det", o.OfficeExpDetail}, {"@p", o.OfficeExpPaidBy} });
        }
        public IEnumerable<OfficeExp> GetOfficeExps()
        {
            var dt = ExecuteQuery("SELECT * FROM OfficeExp ORDER BY OfficeExpDate DESC");
            var res = new List<OfficeExp>();
            foreach (DataRow r in dt.Rows) res.Add(new OfficeExp {
                RowId = GetValue<long>(r, "RowId"), OfficeExpDate = GetValue<DateTime>(r, "OfficeExpDate"), OfficeExpAmount = GetValue<double>(r, "OfficeExpAmount"),
                OfficeExpDetail = GetValue<string>(r, "OfficeExpDetail"), OfficeExpPaidBy = GetValue<string>(r, "OfficeExpPaidBy") });
            return res;
        }

        public void AddReceipt(Receipt r)
        {
            ExecuteNonQuery("INSERT INTO Receipt (ReceiptDate, ReceiptAmount, ReceiptDetail, ReceivedIn, ReceivedFrom) VALUES (@d, @a, @det, @ri, @rf)",
                new Dictionary<string, object> { {"@d", r.ReceiptDate}, {"@a", r.ReceiptAmount}, {"@det", r.ReceiptDetail}, {"@ri", r.ReceivedIn}, {"@rf", r.ReceivedFrom} });
        }
        public IEnumerable<Receipt> GetReceipts()
        {
            var dt = ExecuteQuery("SELECT * FROM Receipt ORDER BY ReceiptDate DESC");
            var res = new List<Receipt>();
            foreach (DataRow r in dt.Rows) res.Add(new Receipt {
                RowId = GetValue<long>(r, "RowId"), ReceiptDate = GetValue<DateTime>(r, "ReceiptDate"), ReceiptAmount = GetValue<double>(r, "ReceiptAmount"),
                ReceiptDetail = GetValue<string>(r, "ReceiptDetail"), ReceivedIn = GetValue<string>(r, "ReceivedIn"), ReceivedFrom = GetValue<string>(r, "ReceivedFrom") });
            return res;
        }

        public void AddYenPayment(Payment p)
        {
            ExecuteNonQuery("INSERT INTO Payment (PaymentDate, PaymentAmountYen, PaymentExcRate, PaymentAmountPkr, PaymentDetail, PaidFrom) VALUES (@d, @ay, @r, @ap, @det, @pf)",
                new Dictionary<string, object> { {"@d", p.PaymentDate}, {"@ay", p.PaymentAmountYen}, {"@r", p.PaymentExcRate}, {"@ap", p.PaymentAmountPkr}, {"@det", p.PaymentDetail}, {"@pf", p.PaidFrom} });
        }
        public IEnumerable<Payment> GetYenPayments()
        {
            var dt = ExecuteQuery("SELECT * FROM Payment ORDER BY PaymentDate DESC");
            var res = new List<Payment>();
            foreach (DataRow r in dt.Rows) res.Add(new Payment {
                RowId = GetValue<long>(r, "RowId"), PaymentDate = GetValue<DateTime>(r, "PaymentDate"), PaymentAmountYen = GetValue<double>(r, "PaymentAmountYen"),
                PaymentExcRate = GetValue<double>(r, "PaymentExcRate"), PaymentAmountPkr = GetValue<double>(r, "PaymentAmountPkr"),
                PaymentDetail = GetValue<string>(r, "PaymentDetail"), PaidFrom = GetValue<string>(r, "PaidFrom") });
            return res;
        }

        public void AddPkrPayment(PaymentPkr p)
        {
            ExecuteNonQuery("INSERT INTO PaymentPkr (PaymentDate, PaymentAmount, PaymentDetail, PaidFrom, PaidTo) VALUES (@d, @a, @det, @pf, @pt)",
                new Dictionary<string, object> { {"@d", p.PaymentDate}, {"@a", p.PaymentAmount}, {"@det", p.PaymentDetail}, {"@pf", p.PaidFrom}, {"@pt", p.PaidTo} });
        }
        public IEnumerable<PaymentPkr> GetPkrPayments()
        {
            var dt = ExecuteQuery("SELECT * FROM PaymentPkr ORDER BY PaymentDate DESC");
            var res = new List<PaymentPkr>();
            foreach (DataRow r in dt.Rows) res.Add(new PaymentPkr {
                RowId = GetValue<long>(r, "RowId"), PaymentDate = GetValue<DateTime>(r, "PaymentDate"), PaymentAmount = GetValue<double>(r, "PaymentAmount"),
                PaymentDetail = GetValue<string>(r, "PaymentDetail"), PaidFrom = GetValue<string>(r, "PaidFrom"), PaidTo = GetValue<string>(r, "PaidTo") });
            return res;
        }

        public void AddAgentPayment(PaymentAgent p)
        {
            ExecuteNonQuery("INSERT INTO PaymentAgent (PaymentDate, PaymentAmount, PaymentDetail, PaidFrom, PaidTo) VALUES (@d, @a, @det, @pf, @pt)",
                new Dictionary<string, object> { {"@d", p.PaymentDate}, {"@a", p.PaymentAmount}, {"@det", p.PaymentDetail}, {"@pf", p.PaidFrom}, {"@pt", p.PaidTo} });
        }
        public IEnumerable<PaymentAgent> GetAgentPayments()
        {
            var dt = ExecuteQuery("SELECT * FROM PaymentAgent ORDER BY PaymentDate DESC");
            var res = new List<PaymentAgent>();
            foreach (DataRow r in dt.Rows) res.Add(new PaymentAgent {
                RowId = GetValue<long>(r, "RowId"), PaymentDate = GetValue<DateTime>(r, "PaymentDate"), PaymentAmount = GetValue<double>(r, "PaymentAmount"),
                PaymentDetail = GetValue<string>(r, "PaymentDetail"), PaidFrom = GetValue<string>(r, "PaidFrom"), PaidTo = GetValue<string>(r, "PaidTo") });
            return res;
        }

        // --- STOCKS ---
        public IEnumerable<Stock> GetStocks(string filter = "")
        {
            var dt = ExecuteQuery("SELECT * FROM Stock ORDER BY Date DESC");
            var res = new List<Stock>();
            foreach (DataRow r in dt.Rows)
            {
                if (!string.IsNullOrEmpty(filter) && !GetValue<string>(r, "Chassis", "").ToLower().Contains(filter.ToLower())) continue;
                res.Add(new Stock {
                    RowId = GetValue<long>(r, "RowId"), Date = GetValue<DateTime>(r, "Date"), Chassis = GetValue<string>(r, "Chassis"), Model = GetValue<string>(r, "Model"), Color = GetValue<string>(r, "Color"),
                    PriceYen = GetValue<double>(r, "PriceYen"), Rate = GetValue<double>(r, "Rate"), PricePkr = GetValue<double>(r, "PricePkr"),
                    Duty = GetValue<double>(r, "Duty"), MiscExpense = GetValue<double>(r, "MiscExpense"), Cost = GetValue<double>(r, "Cost"),
                    Demurrage = GetValue<double>(r, "Demurrage"), NoPlate = GetValue<double>(r, "NoPlate"), Commission = GetValue<double>(r, "Commission"), Tax = GetValue<double>(r, "Tax"),
                    Status = GetValue<string>(r, "Status"), PaidYen = GetValue<double>(r, "PaidYen"), PaidAmount = GetValue<double>(r, "PaidAmount"), Comments = GetValue<string>(r, "Comments")
                });
            }
            return res;
        }
        public void AddStock(Stock s)
        {
            ExecuteNonQuery("INSERT INTO Stock (Date, Chassis, Model, Color, PriceYen, Rate, PricePkr, Duty, MiscExpense, Demurrage, NoPlate, Commission, Tax, Cost, Status, PaidYen, PaidAmount, Comments) VALUES (@d, @c, @m, @co, @py, @r, @pp, @du, @me, @dm, @np, @cm, @tx, @cost, @st, @pdy, @pda, @comm)",
                new Dictionary<string, object> { {"@d", s.Date}, {"@c", s.Chassis}, {"@m", s.Model}, {"@co", s.Color}, {"@py", s.PriceYen}, {"@r", s.Rate}, {"@pp", s.PricePkr}, {"@du", s.Duty}, {"@me", s.MiscExpense}, {"@dm", s.Demurrage}, {"@np", s.NoPlate}, {"@cm", s.Commission}, {"@tx", s.Tax}, {"@cost", s.Cost}, {"@st", s.Status}, {"@pdy", s.PaidYen}, {"@pda", s.PaidAmount}, {"@comm", s.Comments} });
        }
        public void UpdateStock(Stock s)
        {
            ExecuteNonQuery("UPDATE Stock SET Date=@d, Chassis=@c, Model=@m, Color=@co, PriceYen=@py, Rate=@r, PricePkr=@pp, Duty=@du, MiscExpense=@me, Demurrage=@dm, NoPlate=@np, Commission=@cm, Tax=@tx, Cost=@cost, Status=@st, PaidYen=@pdy, PaidAmount=@pda, Comments=@comm WHERE RowId=@id",
                new Dictionary<string, object> { {"@id", s.RowId}, {"@d", s.Date}, {"@c", s.Chassis}, {"@m", s.Model}, {"@co", s.Color}, {"@py", s.PriceYen}, {"@r", s.Rate}, {"@pp", s.PricePkr}, {"@du", s.Duty}, {"@me", s.MiscExpense}, {"@dm", s.Demurrage}, {"@np", s.NoPlate}, {"@cm", s.Commission}, {"@tx", s.Tax}, {"@cost", s.Cost}, {"@st", s.Status}, {"@pdy", s.PaidYen}, {"@pda", s.PaidAmount}, {"@comm", s.Comments} });
        }
        public IEnumerable<string> GetInStockChassisNumbers()
        {
            var dt = ExecuteQuery("SELECT Chassis FROM Stock WHERE Status = 'InStock'");
            var res = new List<string>();
            foreach (DataRow r in dt.Rows) res.Add(GetValue<string>(r, "Chassis"));
            return res;
        }

        // --- CUSTOMERS ---
        public IEnumerable<Customer> GetCustomers(string filter = "")
        {
            var dt = ExecuteQuery("SELECT * FROM Customer ORDER BY Name");
            var res = new List<Customer>();
            foreach (DataRow r in dt.Rows)
            {
                if (!string.IsNullOrEmpty(filter) && !GetValue<string>(r, "Name", "").ToLower().Contains(filter.ToLower())) continue;
                res.Add(new Customer {
                    RowId = GetValue<long>(r, "RowId"), Date = GetValue<DateTime>(r, "Date"), Title = GetValue<string>(r, "Title"), Name = GetValue<string>(r, "Name"),
                    CNIC = GetValue<string>(r, "CNIC"), Phone = GetValue<string>(r, "Phone"), Address = GetValue<string>(r, "Address"),
                    PaymentReceived = GetValue<double>(r, "PaymentReceived"), PaymentReceivable = GetValue<double>(r, "PaymentReceivable"), PaymentPaid = GetValue<double>(r, "PaymentPaid")
                });
            }
            return res;
        }
        public void AddCustomer(Customer c)
        {
            ExecuteNonQuery("INSERT INTO Customer (Date, Title, Name, CNIC, Phone, Address, PaymentReceived, PaymentReceivable, PaymentPaid) VALUES (@d, @ti, @n, @c, @p, @a, @prcvd, @prcvbl, @pp)",
                new Dictionary<string, object> { {"@d", c.Date}, {"@ti", c.Title}, {"@n", c.Name}, {"@c", c.CNIC}, {"@p", c.Phone}, {"@a", c.Address}, {"@prcvd", c.PaymentReceived}, {"@prcvbl", c.PaymentReceivable}, {"@pp", c.PaymentPaid} });
        }
        public void UpdateCustomer(Customer c)
        {
            ExecuteNonQuery("UPDATE Customer SET Date=@d, Title=@ti, Name=@n, CNIC=@c, Phone=@p, Address=@a, PaymentReceived=@prcvd, PaymentReceivable=@prcvbl, PaymentPaid=@pp WHERE RowId=@id",
                new Dictionary<string, object> { {"@id", c.RowId}, {"@d", c.Date}, {"@ti", c.Title}, {"@n", c.Name}, {"@c", c.CNIC}, {"@p", c.Phone}, {"@a", c.Address}, {"@prcvd", c.PaymentReceived}, {"@prcvbl", c.PaymentReceivable}, {"@pp", c.PaymentPaid} });
        }
        public IEnumerable<string> GetCustomerNames()
        {
            var dt = ExecuteQuery("SELECT Title || ' ' || Name as FullName FROM Customer");
            var res = new List<string>();
            foreach (DataRow r in dt.Rows) res.Add(GetValue<string>(r, "FullName"));
            return res;
        }

        // --- AGENTS ---
        public IEnumerable<Agent> GetAgents(string filter = "")
        {
            var dt = ExecuteQuery("SELECT * FROM Agent ORDER BY Name");
            var res = new List<Agent>();
            foreach (DataRow r in dt.Rows)
            {
                if (!string.IsNullOrEmpty(filter) && !GetValue<string>(r, "Name", "").ToLower().Contains(filter.ToLower())) continue;
                res.Add(new Agent {
                    RowId = GetValue<long>(r, "RowId"), Date = GetValue<DateTime>(r, "Date"), Name = GetValue<string>(r, "Name"),
                    CNIC = GetValue<string>(r, "CNIC"), Phone = GetValue<string>(r, "Phone"), Address = GetValue<string>(r, "Address"),
                    PaymentReceivable = GetValue<double>(r, "PaymentReceivable"), PaymentPaid = GetValue<double>(r, "PaymentPaid")
                });
            }
            return res;
        }
        public void AddAgent(Agent a)
        {
            ExecuteNonQuery("INSERT INTO Agent (Date, Name, CNIC, Phone, Address, PaymentReceivable, PaymentPaid) VALUES (@d, @n, @c, @p, @a, @prcvbl, @pp)",
                new Dictionary<string, object> { {"@d", a.Date}, {"@n", a.Name}, {"@c", a.CNIC}, {"@p", a.Phone}, {"@a", a.Address}, {"@prcvbl", a.PaymentReceivable}, {"@pp", a.PaymentPaid} });
        }
        public void UpdateAgent(Agent a)
        {
            ExecuteNonQuery("UPDATE Agent SET Date=@d, Name=@n, CNIC=@c, Phone=@p, Address=@a, PaymentReceivable=@prcvbl, PaymentPaid=@pp WHERE RowId=@id",
                new Dictionary<string, object> { {"@id", a.RowId}, {"@d", a.Date}, {"@n", a.Name}, {"@c", a.CNIC}, {"@p", a.Phone}, {"@a", a.Address}, {"@prcvbl", a.PaymentReceivable}, {"@pp", a.PaymentPaid} });
        }
        public IEnumerable<string> GetAgentNames()
        {
            var dt = ExecuteQuery("SELECT Name FROM Agent");
            var res = new List<string>();
            foreach (DataRow r in dt.Rows) res.Add(GetValue<string>(r, "Name"));
            return res;
        }

        // --- SALES ---
        public IEnumerable<Sale> GetSales()
        {
            var dt = ExecuteQuery("SELECT * FROM Sale ORDER BY SaleDate DESC");
            var res = new List<Sale>();
            foreach (DataRow r in dt.Rows) res.Add(new Sale {
                RowId = GetValue<long>(r, "RowId"), SaleDate = GetValue<DateTime>(r, "SaleDate"), SaleChassis = GetValue<string>(r, "SaleChassis"),
                SaleCustomer = GetValue<string>(r, "SaleCustomer"), SalePrice = GetValue<double>(r, "SalePrice"),
                SaleAmountReceived = GetValue<double>(r, "SaleAmountReceived"), PaymentReceivedIn = GetValue<string>(r, "PaymentReceivedIn"),
                InstallmentMonths = GetValue<int>(r, "InstallmentMonths"), ReminderDaysBefore = GetValue<int>(r, "ReminderDaysBefore")
            });
            return res;
        }

        // Returns the RowId of the newly inserted Sale, needed to attach an installment plan.
        public long AddSale(Sale s)
        {
            ExecuteNonQuery("INSERT INTO Sale (SaleDate, SaleChassis, SaleCustomer, SalePrice, SaleAmountReceived, PaymentReceivedIn, InstallmentMonths, ReminderDaysBefore) VALUES (@d, @c, @cust, @p, @a, @pri, @im, @rdb)",
                new Dictionary<string, object> { {"@d", s.SaleDate}, {"@c", s.SaleChassis}, {"@cust", s.SaleCustomer}, {"@p", s.SalePrice}, {"@a", s.SaleAmountReceived}, {"@pri", s.PaymentReceivedIn}, {"@im", s.InstallmentMonths}, {"@rdb", s.ReminderDaysBefore} });
            var dt = ExecuteQuery("SELECT last_insert_rowid() as Id");
            return GetValue<long>(dt.Rows[0], "Id");
        }

        // --- INSTALLMENT PLANS ---

        // Splits `totalBalance` evenly across `months` monthly installments starting one
        // month after `startDate`. Used both for the initial plan at sale time and for
        // re-planning the remaining unpaid balance later (see ReplanInstallments).
        public void AddInstallmentPlan(long saleRowId, DateTime startDate, double totalBalance, int months, int startingInstallmentNumber = 1)
        {
            if (months <= 0 || totalBalance <= 0) return;
            double perInstallment = Math.Round(totalBalance / months, 2);
            double allocated = 0;
            for (int i = 0; i < months; i++)
            {
                // Last installment absorbs any rounding remainder so the total matches exactly.
                double amount = (i == months - 1) ? Math.Round(totalBalance - allocated, 2) : perInstallment;
                allocated += amount;
                ExecuteNonQuery("INSERT INTO Installment (SaleRowId, InstallmentNumber, DueDate, Amount, IsPaid) VALUES (@s, @n, @d, @a, 0)",
                    new Dictionary<string, object> {
                        {"@s", saleRowId}, {"@n", startingInstallmentNumber + i},
                        {"@d", startDate.Date.AddMonths(i + 1)}, {"@a", amount}
                    });
            }
        }

        public List<Installment> GetInstallmentsForSale(long saleRowId)
        {
            var dt = ExecuteQuery("SELECT * FROM Installment WHERE SaleRowId = @s ORDER BY InstallmentNumber ASC",
                new Dictionary<string, object> { {"@s", saleRowId} });
            var res = new List<Installment>();
            foreach (DataRow r in dt.Rows) res.Add(MapInstallment(r));
            return res;
        }

        // All unpaid installments across every sale, with customer/chassis/reminder info
        // joined in from the parent Sale for display purposes.
        public List<Installment> GetUnpaidInstallments()
        {
            var dt = ExecuteQuery(
                "SELECT i.*, s.SaleCustomer, s.SaleChassis, s.ReminderDaysBefore, s.InstallmentMonths " +
                "FROM Installment i INNER JOIN Sale s ON s.RowId = i.SaleRowId " +
                "WHERE i.IsPaid = 0 ORDER BY i.DueDate ASC");
            var res = new List<Installment>();
            foreach (DataRow r in dt.Rows)
            {
                var inst = MapInstallment(r);
                inst.SaleCustomer = GetValue<string>(r, "SaleCustomer");
                inst.SaleChassis = GetValue<string>(r, "SaleChassis");
                inst.ReminderDaysBefore = GetValue<int>(r, "ReminderDaysBefore");
                inst.TotalInstallments = GetValue<int>(r, "InstallmentMonths");
                res.Add(inst);
            }
            return res;
        }

        // The next unpaid installment for a sale (lowest InstallmentNumber not yet paid), if any.
        public Installment GetNextUnpaidInstallment(long saleRowId)
        {
            var dt = ExecuteQuery(
                "SELECT i.*, s.SaleCustomer, s.SaleChassis, s.ReminderDaysBefore, s.InstallmentMonths " +
                "FROM Installment i INNER JOIN Sale s ON s.RowId = i.SaleRowId " +
                "WHERE i.SaleRowId = @s AND i.IsPaid = 0 ORDER BY i.InstallmentNumber ASC LIMIT 1",
                new Dictionary<string, object> { {"@s", saleRowId} });
            if (dt.Rows.Count == 0) return null;
            var r = dt.Rows[0];
            var inst = MapInstallment(r);
            inst.SaleCustomer = GetValue<string>(r, "SaleCustomer");
            inst.SaleChassis = GetValue<string>(r, "SaleChassis");
            inst.ReminderDaysBefore = GetValue<int>(r, "ReminderDaysBefore");
            inst.TotalInstallments = GetValue<int>(r, "InstallmentMonths");
            return inst;
        }

        // Records an installment payment as a normal Receipt too, so Receipts stays the single
        // source of truth for "money received from a customer" — installment payments aren't a
        // separate parallel record, they just also close out a specific installment.
        //
        // A short-paid installment isn't lost — the shortfall is rolled onto the Amount of the
        // next unpaid installment (and an overpayment reduces it the same way), so the schedule
        // always re-balances to the true remaining balance. The final installment is the one
        // place this can't be deferred further, so it's blocked until the full amount (including
        // anything carried forward into it) is actually received.
        public void PayInstallment(Installment installment, DateTime paidDate, double paidAmount, string account)
        {
            bool isFinalInstallment = installment.InstallmentNumber == installment.TotalInstallments;
            if (isFinalInstallment && paidAmount < installment.Amount)
                throw new InvalidOperationException(
                    $"This is the final installment — the full remaining amount of {installment.Amount:N0} must be received before it can be marked paid.");

            ExecuteNonQuery("UPDATE Installment SET IsPaid = 1, PaidDate = @pd, PaidAmount = @pa, PaidInAccount = @acc WHERE RowId = @id",
                new Dictionary<string, object> { {"@pd", paidDate}, {"@pa", paidAmount}, {"@acc", account}, {"@id", installment.RowId} });

            double shortfall = installment.Amount - paidAmount;
            if (!isFinalInstallment && shortfall != 0)
            {
                ExecuteNonQuery("UPDATE Installment SET Amount = Amount + @sf WHERE SaleRowId = @s AND InstallmentNumber = @n",
                    new Dictionary<string, object> { {"@sf", shortfall}, {"@s", installment.SaleRowId}, {"@n", installment.InstallmentNumber + 1} });
            }

            string detail = $"Installment {installment.InstallmentNumber}/{installment.TotalInstallments} — Chassis {installment.SaleChassis}";
            AddReceipt(new Receipt
            {
                ReceiptDate = paidDate, ReceiptAmount = paidAmount, ReceiptDetail = detail,
                ReceivedIn = account, ReceivedFrom = installment.SaleCustomer
            });
            CreditAccountWithLedger(account, paidAmount, paidDate, detail);
            RecordCustomerReceipt(installment.SaleCustomer, paidAmount);
        }

        // Re-plans a sale's remaining schedule: deletes any still-unpaid installments and
        // recreates `newMonths` fresh ones, splitting whatever balance is still outstanding
        // (SalePrice - AmountReceived - sum of already-paid installments) evenly across them,
        // starting from today. Already-paid installments are left untouched.
        public void ReplanInstallments(long saleRowId, int newMonths, int newReminderDaysBefore)
        {
            var sale = GetSales().FirstOrDefault(s => s.RowId == saleRowId);
            if (sale == null) return;
            var existing = GetInstallmentsForSale(saleRowId);
            double paidSoFar = existing.Where(i => i.IsPaid).Sum(i => i.PaidAmount);
            double remaining = sale.SaleBalance - paidSoFar;
            int nextNumber = existing.Where(i => i.IsPaid).Select(i => i.InstallmentNumber).DefaultIfEmpty(0).Max() + 1;

            ExecuteNonQuery("DELETE FROM Installment WHERE SaleRowId = @s AND IsPaid = 0",
                new Dictionary<string, object> { {"@s", saleRowId} });
            ExecuteNonQuery("UPDATE Sale SET InstallmentMonths = @im, ReminderDaysBefore = @rdb WHERE RowId = @id",
                new Dictionary<string, object> { {"@im", nextNumber - 1 + newMonths}, {"@rdb", newReminderDaysBefore}, {"@id", saleRowId} });

            if (remaining > 0 && newMonths > 0)
                AddInstallmentPlan(saleRowId, DateTime.Today, remaining, newMonths, nextNumber);
        }

        private Installment MapInstallment(DataRow r) => new Installment
        {
            RowId = GetValue<long>(r, "RowId"), SaleRowId = GetValue<long>(r, "SaleRowId"),
            InstallmentNumber = GetValue<int>(r, "InstallmentNumber"), DueDate = GetValue<DateTime>(r, "DueDate"),
            Amount = GetValue<double>(r, "Amount"), IsPaid = GetValue<int>(r, "IsPaid") != 0,
            PaidDate = r["PaidDate"] != DBNull.Value ? GetValue<DateTime>(r, "PaidDate") : (DateTime?)null,
            PaidAmount = GetValue<double>(r, "PaidAmount"), PaidInAccount = GetValue<string>(r, "PaidInAccount")
        };

        // Active credit sales, per outstanding installment (one row per unpaid installment).
        public DataTable GetActiveCreditSales()
        {
            var table = new DataTable();
            table.Columns.Add("Customer", typeof(string));
            table.Columns.Add("Chassis", typeof(string));
            table.Columns.Add("Installment", typeof(string));
            table.Columns.Add("DueDate", typeof(DateTime));
            table.Columns.Add("DaysRemaining", typeof(string));
            table.Columns.Add("Amount", typeof(double));

            foreach (var i in GetUnpaidInstallments())
            {
                var row = table.NewRow();
                row["Customer"] = i.SaleCustomer;
                row["Chassis"] = i.SaleChassis;
                row["Installment"] = $"{i.InstallmentNumber}/{i.TotalInstallments}";
                row["DueDate"] = i.DueDate;
                row["DaysRemaining"] = i.DaysUntilDue < 0 ? $"Overdue {-i.DaysUntilDue}d" : $"{i.DaysUntilDue}d";
                row["Amount"] = i.Amount;
                table.Rows.Add(row);
            }
            return table;
        }

        // --- DASHBOARD/REPORTS ---

        // Cash-basis profit: only cash that has actually been received counts, and only if it
        // landed in an account flagged IncludeInProfit (so Petty Cash or similar non-sales
        // accounts don't inflate the figure). Cost is taken in full for every Sold car, since
        // that money has already genuinely left the business at purchase time. This is what
        // drives both "Total Profit Available" (dashboard) and the "Profit Breakdown" report —
        // both read the same numbers so they always reconcile.
        public DataTable GetProfitBreakdown()
        {
            var table = new DataTable();
            table.Columns.Add("Chassis", typeof(string));
            table.Columns.Add("Model", typeof(string));
            table.Columns.Add("Sale Price", typeof(double));
            table.Columns.Add("Cash Collected", typeof(double));
            table.Columns.Add("Cost", typeof(double));
            table.Columns.Add("Profit Contribution", typeof(double));

            var sales = ExecuteQuery(
                "SELECT sa.RowId as SaleRowId, sa.SaleChassis, st.Model, sa.SalePrice, st.Cost, " +
                "sa.SaleAmountReceived, sa.PaymentReceivedIn " +
                "FROM Stock st INNER JOIN Sale sa ON sa.SaleChassis = st.Chassis WHERE st.Status = 'Sold'");

            double totalSalePrice = 0, totalCashCollected = 0, totalCost = 0, totalProfit = 0;

            foreach (DataRow r in sales.Rows)
            {
                long saleRowId = GetValue<long>(r, "SaleRowId");
                double salePrice = GetValue<double>(r, "SalePrice");
                double cost = GetValue<double>(r, "Cost");

                double cashCollected = IsAccountProfitLinked(GetValue<string>(r, "PaymentReceivedIn"))
                    ? GetValue<double>(r, "SaleAmountReceived") : 0;

                var paidInstallments = ExecuteQuery(
                    "SELECT PaidAmount, PaidInAccount FROM Installment WHERE SaleRowId = @s AND IsPaid = 1",
                    new Dictionary<string, object> { {"@s", saleRowId} });
                foreach (DataRow ir in paidInstallments.Rows)
                    if (IsAccountProfitLinked(GetValue<string>(ir, "PaidInAccount")))
                        cashCollected += GetValue<double>(ir, "PaidAmount");

                double profit = cashCollected - cost;

                var row = table.NewRow();
                row["Chassis"] = GetValue<string>(r, "SaleChassis");
                row["Model"] = GetValue<string>(r, "Model");
                row["Sale Price"] = salePrice;
                row["Cash Collected"] = cashCollected;
                row["Cost"] = cost;
                row["Profit Contribution"] = profit;
                table.Rows.Add(row);

                totalSalePrice += salePrice; totalCashCollected += cashCollected; totalCost += cost; totalProfit += profit;
            }

            var wd = ExecuteQuery("SELECT SUM(Amount) as Total FROM OfficeAccount WHERE DebitTo = 'Profit'");
            double alreadyWithdrawn = (wd.Rows.Count > 0 && wd.Rows[0]["Total"] != DBNull.Value)
                ? Convert.ToDouble(wd.Rows[0]["Total"]) : 0;

            AddProfitSummaryRow(table, "TOTAL", totalSalePrice, totalCashCollected, totalCost, totalProfit);
            AddProfitSummaryRow(table, "LESS: PROFIT ALREADY WITHDRAWN", null, null, null, -alreadyWithdrawn);
            AddProfitSummaryRow(table, "NET PROFIT AVAILABLE", null, null, null, totalProfit - alreadyWithdrawn);

            return table;
        }

        private void AddProfitSummaryRow(DataTable table, string label, double? salePrice, double? cashCollected, double? cost, double profit)
        {
            var row = table.NewRow();
            row["Chassis"] = label;
            row["Model"] = DBNull.Value;
            row["Sale Price"] = salePrice.HasValue ? (object)salePrice.Value : DBNull.Value;
            row["Cash Collected"] = cashCollected.HasValue ? (object)cashCollected.Value : DBNull.Value;
            row["Cost"] = cost.HasValue ? (object)cost.Value : DBNull.Value;
            row["Profit Contribution"] = profit;
            table.Rows.Add(row);
        }

        private bool IsAccountProfitLinked(string accountName)
        {
            if (string.IsNullOrEmpty(accountName)) return false;
            var dt = ExecuteQuery("SELECT IncludeInProfit FROM Account WHERE AccountName = @n",
                new Dictionary<string, object> { {"@n", accountName} });
            if (dt.Rows.Count == 0) return false;
            return GetValue<int>(dt.Rows[0], "IncludeInProfit", 1) != 0;
        }

        public double GetTotalProfit()
        {
            var breakdown = GetProfitBreakdown();
            if (breakdown.Rows.Count == 0) return 0;
            return Convert.ToDouble(breakdown.Rows[breakdown.Rows.Count - 1]["Profit Contribution"]);
        }
        public double GetTotalYenPayable()
        {
            var dt = ExecuteQuery("SELECT SUM(PriceYen - PaidYen) as Total FROM Stock");
            if (dt.Rows.Count > 0 && dt.Rows[0]["Total"] != DBNull.Value)
                return Convert.ToDouble(dt.Rows[0]["Total"]);
            return 0;
        }

        public DataTable GetReportData(string reportType, DateTime fromDate, DateTime toDate, string saleTypeFilter = null)
        {
            string query;
            string from = fromDate.ToString("yyyy-MM-dd");
            string to   = toDate.ToString("yyyy-MM-dd");
            switch (reportType)
            {
                case "Sold Cars":
                    query = "SELECT sa.SaleDate as Date, st.Chassis, st.Model, st.Color, st.Cost, sa.SalePrice, (sa.SalePrice - st.Cost) as Profit "
                          + "FROM Stock st INNER JOIN Sale sa ON sa.SaleChassis = st.Chassis "
                          + "WHERE st.Status = 'Sold' AND sa.SaleDate >= @from AND sa.SaleDate <= @to";
                    if (saleTypeFilter == "Credit") query += " AND sa.InstallmentMonths > 0";
                    else if (saleTypeFilter == "Cash") query += " AND (sa.InstallmentMonths IS NULL OR sa.InstallmentMonths = 0)";
                    break;
                case "Stocks":
                    query = "SELECT Date, Chassis, Model, Color, PricePkr as [Price PKR], Duty as Clearance, MiscExpense as [Misc Exp], Demurrage, NoPlate as [No Plate], Commission, Tax, Cost, Status FROM Stock WHERE Date >= @from AND Date <= @to";
                    break;
                case "Accounts":
                    query = "SELECT AccountType as Type, AccountName as Name, AccountNumber as [Acc No], BankName as Bank, OpeningBalance as [Opening Bal], CurrentBalance as [Current Bal] FROM Account";
                    break;
                case "Accounts Receivable":
                    query = "SELECT Name, Phone, PaymentReceivable as [Receivable], PaymentReceived as [Received], (PaymentReceivable - PaymentReceived) as [Balance] FROM Customer WHERE PaymentReceivable > 0";
                    break;
                case "Accounts Payable":
                    query = "SELECT Name, Phone, PaymentReceivable as [Payable], PaymentPaid as [Paid], (PaymentReceivable - PaymentPaid) as [Balance] FROM Agent WHERE PaymentReceivable > 0";
                    break;
                case "Trial Balance":
                    query = "SELECT AccountName as Account, OpeningBalance as [Opening], CurrentBalance as [Current Balance] FROM Account";
                    break;
                case "Office Expenses":
                    query = "SELECT OfficeExpDate as Date, OfficeExpDetail as Detail, OfficeExpAmount as Amount, OfficeExpPaidBy as [Paid By] FROM OfficeExp WHERE OfficeExpDate >= @from AND OfficeExpDate <= @to";
                    break;
                case "Misc. Auto Expenses":
                    query = "SELECT Chassis, MiscExpDate as Date, MiscExpAmount as Amount, MiscExpDetail as Detail, MiscExpPaidBy as [Paid By] FROM MiscExp WHERE MiscExpDate >= @from AND MiscExpDate <= @to";
                    break;
                case "Clearance Expenses":
                    query = "SELECT Chassis, DutyExpDate as Date, DutyExpAmount as Amount, DutyExpDetail as Detail, DutyExpAgent as Agent, DutyExpPaidBy as [Paid By] FROM DutyExp WHERE DutyExpDate >= @from AND DutyExpDate <= @to";
                    break;
                case "Demurrage Expenses":
                    query = "SELECT Chassis, DemurrageExpDate as Date, DemurrageExpAmount as Amount, DemurrageExpDetail as Detail, DemurrageExpPaidBy as [Paid By] FROM DemurrageExp WHERE DemurrageExpDate >= @from AND DemurrageExpDate <= @to";
                    break;
                case "No Plate Expenses":
                    query = "SELECT Chassis, NoPlateExpDate as Date, NoPlateExpAmount as Amount, NoPlateExpDetail as Detail, NoPlateExpPaidBy as [Paid By] FROM NoPlateExp WHERE NoPlateExpDate >= @from AND NoPlateExpDate <= @to";
                    break;
                case "Commission Expenses":
                    query = "SELECT Chassis, CommissionExpDate as Date, CommissionExpAmount as Amount, CommissionExpDetail as Detail, CommissionExpPaidBy as [Paid By] FROM CommissionExp WHERE CommissionExpDate >= @from AND CommissionExpDate <= @to";
                    break;
                case "Tax Expenses":
                    query = "SELECT Chassis, TaxExpDate as Date, TaxExpAmount as Amount, TaxExpDetail as Detail, TaxExpPaidBy as [Paid By] FROM TaxExp WHERE TaxExpDate >= @from AND TaxExpDate <= @to";
                    break;
                case "Receipts":
                    query = "SELECT ReceiptDate as Date, ReceiptAmount as Amount, ReceiptDetail as Detail, ReceivedIn as [Received In], ReceivedFrom as [Received From] FROM Receipt WHERE ReceiptDate >= @from AND ReceiptDate <= @to";
                    break;
                case "Yen Payments":
                    query = "SELECT PaymentDate as Date, PaymentAmountYen as [Amount (Yen)], PaymentExcRate as Rate, PaymentAmountPkr as [Amount (PKR)], PaymentDetail as Detail, PaidFrom as Account FROM Payment WHERE PaymentDate >= @from AND PaymentDate <= @to";
                    break;
                case "Party Payments":
                    query = "SELECT PaymentDate as Date, PaymentAmount as Amount, PaymentDetail as Detail, PaidFrom as [From Account], PaidTo as Customer FROM PaymentPkr WHERE PaymentDate >= @from AND PaymentDate <= @to";
                    break;
                case "Agent Payments":
                    query = "SELECT PaymentDate as Date, PaymentAmount as Amount, PaymentDetail as Detail, PaidFrom as [From Account], PaidTo as Agent FROM PaymentAgent WHERE PaymentDate >= @from AND PaymentDate <= @to";
                    break;
                default:
                    query = "SELECT AccountName as Name, CurrentBalance as [Balance] FROM Account";
                    break;
            }
            return ExecuteQuery(query, new Dictionary<string, object> { { "@from", from }, { "@to", to } });
        }

        public void WithdrawProfit(double amount, string accountName, string detail)
        {
            DebitAccountWithLedger(accountName, amount, DateTime.Today, $"Profit Withdrawal: {detail}");
            ExecuteNonQuery("INSERT INTO OfficeAccount (Date, Amount, Detail, CreditFrom, DebitTo) VALUES (@date, @amount, @detail, @acc, 'Profit')",
                new Dictionary<string, object> { { "@date", DateTime.Today.ToString("yyyy-MM-dd") }, { "@amount", amount }, { "@detail", detail }, { "@acc", accountName } });
        }
    }
}
