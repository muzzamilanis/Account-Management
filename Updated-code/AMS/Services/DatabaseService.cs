using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
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
                CREATE TABLE IF NOT EXISTS OfficeExp (RowId INTEGER PRIMARY KEY AUTOINCREMENT, OfficeExpDate DATETIME, OfficeExpAmount REAL, OfficeExpDetail TEXT, OfficeExpPaidBy TEXT);
                CREATE TABLE IF NOT EXISTS OfficeAccount (RowId INTEGER PRIMARY KEY AUTOINCREMENT, Date DATETIME, Amount REAL, Detail TEXT, CreditFrom TEXT, DebitTo TEXT, LedgerRowId INTEGER);
            ";
            using (var cmd = new SQLiteCommand(schema, _connection))
            {
                cmd.ExecuteNonQuery();
            }
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
                    OpeningBalance = GetValue<double>(r, "OpeningBalance"), CurrentBalance = GetValue<double>(r, "CurrentBalance")
                });
            }
            return res;
        }
        
        public void AddAccount(Account a)
        {
            ExecuteNonQuery("INSERT INTO Account (AccountDate, AccountType, AccountName, AccountNumber, AccountTitle, BankName, BankBranch, OpeningBalance, CurrentBalance) VALUES (@d, @ty, @n, @no, @ti, @bn, @bb, @ob, @cb)",
                new Dictionary<string, object> { {"@d", a.AccountDate}, {"@ty", a.AccountType}, {"@n", a.AccountName}, {"@no", a.AccountNumber}, {"@ti", a.AccountTitle}, {"@bn", a.BankName}, {"@bb", a.BankBranch}, {"@ob", a.OpeningBalance}, {"@cb", a.CurrentBalance} });
        }
        
        public void UpdateAccount(Account a)
        {
            ExecuteNonQuery("UPDATE Account SET AccountDate=@d, AccountType=@ty, AccountName=@n, AccountNumber=@no, AccountTitle=@ti, BankName=@bn, BankBranch=@bb, OpeningBalance=@ob, CurrentBalance=@cb WHERE RowId=@id",
                new Dictionary<string, object> { {"@id", a.RowId}, {"@d", a.AccountDate}, {"@ty", a.AccountType}, {"@n", a.AccountName}, {"@no", a.AccountNumber}, {"@ti", a.AccountTitle}, {"@bn", a.BankName}, {"@bb", a.BankBranch}, {"@ob", a.OpeningBalance}, {"@cb", a.CurrentBalance} });
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

        public void AdjustStockDuty(string chassis, double delta)
        {
            ExecuteNonQuery("UPDATE Stock SET Duty = Duty + @delta WHERE Chassis = @chassis",
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
                    Status = GetValue<string>(r, "Status"), PaidYen = GetValue<double>(r, "PaidYen"), PaidAmount = GetValue<double>(r, "PaidAmount"), Comments = GetValue<string>(r, "Comments")
                });
            }
            return res;
        }
        public void AddStock(Stock s)
        {
            ExecuteNonQuery("INSERT INTO Stock (Date, Chassis, Model, Color, PriceYen, Rate, PricePkr, Duty, MiscExpense, Cost, Status, PaidYen, PaidAmount, Comments) VALUES (@d, @c, @m, @co, @py, @r, @pp, @du, @me, @cost, @st, @pdy, @pda, @comm)",
                new Dictionary<string, object> { {"@d", s.Date}, {"@c", s.Chassis}, {"@m", s.Model}, {"@co", s.Color}, {"@py", s.PriceYen}, {"@r", s.Rate}, {"@pp", s.PricePkr}, {"@du", s.Duty}, {"@me", s.MiscExpense}, {"@cost", s.Cost}, {"@st", s.Status}, {"@pdy", s.PaidYen}, {"@pda", s.PaidAmount}, {"@comm", s.Comments} });
        }
        public void UpdateStock(Stock s)
        {
            ExecuteNonQuery("UPDATE Stock SET Date=@d, Chassis=@c, Model=@m, Color=@co, PriceYen=@py, Rate=@r, PricePkr=@pp, Duty=@du, MiscExpense=@me, Cost=@cost, Status=@st, PaidYen=@pdy, PaidAmount=@pda, Comments=@comm WHERE RowId=@id",
                new Dictionary<string, object> { {"@id", s.RowId}, {"@d", s.Date}, {"@c", s.Chassis}, {"@m", s.Model}, {"@co", s.Color}, {"@py", s.PriceYen}, {"@r", s.Rate}, {"@pp", s.PricePkr}, {"@du", s.Duty}, {"@me", s.MiscExpense}, {"@cost", s.Cost}, {"@st", s.Status}, {"@pdy", s.PaidYen}, {"@pda", s.PaidAmount}, {"@comm", s.Comments} });
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
                SaleAmountReceived = GetValue<double>(r, "SaleAmountReceived"), PaymentReceivedIn = GetValue<string>(r, "PaymentReceivedIn")
            });
            return res;
        }
        public void AddSale(Sale s)
        {
            ExecuteNonQuery("INSERT INTO Sale (SaleDate, SaleChassis, SaleCustomer, SalePrice, SaleAmountReceived, PaymentReceivedIn) VALUES (@d, @c, @cust, @p, @a, @pri)",
                new Dictionary<string, object> { {"@d", s.SaleDate}, {"@c", s.SaleChassis}, {"@cust", s.SaleCustomer}, {"@p", s.SalePrice}, {"@a", s.SaleAmountReceived}, {"@pri", s.PaymentReceivedIn} });
        }

        // --- DASHBOARD/REPORTS ---
        public double GetTotalProfit()
        {
            var dt = ExecuteQuery(
                "SELECT SUM(sa.SalePrice - st.Cost) as Total " +
                "FROM Stock st " +
                "INNER JOIN Sale sa ON sa.SaleChassis = st.Chassis " +
                "WHERE st.Status = 'Sold'");
            if (dt.Rows.Count > 0 && dt.Rows[0]["Total"] != DBNull.Value)
                return Convert.ToDouble(dt.Rows[0]["Total"]);
            return 0;
        }
        public double GetTotalYenPayable()
        {
            var dt = ExecuteQuery("SELECT SUM(PriceYen - PaidYen) as Total FROM Stock");
            if (dt.Rows.Count > 0 && dt.Rows[0]["Total"] != DBNull.Value)
                return Convert.ToDouble(dt.Rows[0]["Total"]);
            return 0;
        }

        public DataTable GetReportData(string reportType, DateTime fromDate, DateTime toDate)
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
                    break;
                case "Stocks":
                    query = "SELECT Date, Chassis, Model, Color, PricePkr as [Price PKR], Duty, MiscExpense as [Misc Exp], Cost, Status FROM Stock WHERE Date >= @from AND Date <= @to";
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
                case "Duty Expenses":
                    query = "SELECT Chassis, DutyExpDate as Date, DutyExpAmount as Amount, DutyExpDetail as Detail, DutyExpAgent as Agent, DutyExpPaidBy as [Paid By] FROM DutyExp WHERE DutyExpDate >= @from AND DutyExpDate <= @to";
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
            ExecuteNonQuery("UPDATE Account SET CurrentBalance = CurrentBalance - @amount WHERE AccountName = @acc",
                new Dictionary<string, object> { { "@amount", amount }, { "@acc", accountName } });
            ExecuteNonQuery("INSERT INTO OfficeAccount (Date, Amount, Detail, CreditFrom, DebitTo) VALUES (@date, @amount, @detail, @acc, 'Profit')",
                new Dictionary<string, object> { { "@date", DateTime.Today.ToString("yyyy-MM-dd") }, { "@amount", amount }, { "@detail", detail }, { "@acc", accountName } });
        }
    }
}
