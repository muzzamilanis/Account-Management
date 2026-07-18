# AMS / Autos Accounts — Project Context

Handoff notes for the WPF UI restyle + functional bugfix pass on `Updated-code/AMS`.
Read this before starting new work here — it explains which codebase is authoritative,
where the ground-truth references live, and what's already been fixed vs. still open.

## Which codebase is which

This repo contains **four** things that look similar but serve different purposes:

| Path | What it is | Use for |
|---|---|---|
| `Updated-code/AMS/` | **The active project.** WPF MVVM rewrite (net48, SDK-style csproj). This is what gets built and shipped. | All new work goes here. |
| `app/Accounts/Autos_Accounts.exe` | The original compiled legacy app. Still runnable. | Visual/behavioral ground truth — run it directly to compare. |
| `Code/Autos_Accounts/` | A **decompiled** copy of the legacy app's real source (ILSpy/dnSpy output, VB `My` namespace leftovers, ~2000-line monolithic `MainWindow.xaml.cs`). Its `dictionary1.xaml` was reskinned to a modern navy/sky-blue palette by an unrelated prior session — **do not use its colors as reference**, but its field labels, control layout, validation messages, and (critically) its exact SQL/business logic in `MainWindow.xaml.cs` are the authoritative record of what the original app actually does. | Grep this when you need to know "what does the legacy app actually do here" — search for the relevant `btnXxx_Click` handler. |
| `winforms/` | A barely-started WinForms skeleton (3 near-empty projects, doesn't compile — references a `DashboardForm` that doesn't exist). | Not a serious candidate. Ignore unless explicitly asked to revisit it. |
| `Updated-code/AMS/Document/Running application screenshots.docx` (extracted to `.../Document/screenshots_extracted/word/media/image1.png`–`image21.png`) | 21 reference screenshots of the real legacy app in use. | Visual ground truth for exact colors/layout/labels. |

## Phase 1 — Visual restyle (teal "Jaini Auto Accounts Manager" look)

Goal: make `Updated-code/AMS` an exact visual replica of the screenshots (flat dark-teal
header/sidebar, vertical secondary nav, teal-header data grids, classic rectangular dialogs).

What changed:
- **`Themes/LegacyTeal.xaml`** (new) — single consolidated resource dictionary replacing 7
  separate/orphaned theme files (`BaseDark/Light.xaml`, `ThemeOriginal/Classic/ModernBlue.xaml`,
  `OriginalTealTheme.xaml`, `Resources/Themes/DarkTheme.xaml` — all deleted). Defines both the
  `bg1/bg2/bg3/bg4/TextPrimary/...` key family (used by dialogs) and the
  `BaseBg/PrimaryBg/AccentBrush/...` key family (used by MainWindow/LoginWindow) pointing at the
  same teal palette, plus full implicit control styles (Button, TextBox, ComboBox, DataGrid,
  ListBox, TabControl) and the new `SecondaryNavItemStyle`.
- **Found and fixed a real pre-existing bug while doing this**: the 13 dialog windows under
  `Views/Dialogs/*.xaml` referenced `{StaticResource bg1/bg2/AccentBrush}` etc., but nothing in
  the app ever merged a dictionary defining those keys — every dialog was throwing a
  `XamlParseException` on open, silently swallowed by `MainWindow.xaml.cs`'s
  `catch (Exception ex) { MessageBox.Show(ex.Message); }` pattern. Fixed by the consolidation above.
- **Theme switcher removed** — the app now has one fixed skin, no runtime switching.
  `Services/ThemeManager.cs` deleted.
- **`MainWindow.xaml`**: Accounts sub-nav restructured from a horizontal top strip to a vertical
  left column (matches legacy layout); header now has the big centered bold title; window title
  changed to "Autos Accounts".
- **`LoginWindow.xaml`**: converted from custom rounded/transparent chrome to plain rectangular
  `Window`, retitled "UserLogin", added a "Register Now" button (placeholder only — shows an info
  `MessageBox`, no real registration logic, per explicit user decision).
- **`AddAccountDialog`**: added conditional visibility for Bank-only fields (Account Number/Title/
  Bank Name/Branch) based on Account Type, via `AddAccountViewModel.IsBankVisible`.

## Phase 2 — Functional/data-sync bugfixes

While testing the restyle, the user found the app wasn't actually moving money correctly between
accounts/agents/customers/stock when transactions were saved. This turned into a much bigger
bugfix pass, verified against `Code/Autos_Accounts/MainWindow.xaml.cs`'s decompiled SQL.

**Core pattern**: `Services/DatabaseService.cs` has `DebitAccount`/`CreditAccount` (cash/bank),
`AdjustAgentPayable`/`RecordAgentPayment` (agent ledger), `RecordCustomerPayment`/
`RecordCustomerReceipt`/`RecordCustomerSale` (customer ledger), `AdjustStockDuty`/`MarkStockSold`
(stock). Every dialog's `Save()` in `ViewModels/Dialogs/*.cs` is supposed to call the right
combination of these after inserting its own record. Bugs found and fixed, roughly in the order
discovered:

1. **`AddAccountViewModel`**: new accounts never initialized `CurrentBalance` from
   `OpeningBalance` — always saved as 0.
2. **`AccountTransferViewModel`**: recorded the transfer row but never actually debited/credited
   either account.
3. **Misc/Office Expense, Receipts, Yen/Party Payments, Agent Payments, Sale**: none of these
   touched account balances at all originally — wired up `DebitAccount`/`CreditAccount` calls to
   all of them.
4. **`DutyExpViewModel`** — first wired incorrectly (debited a cash/bank account), then corrected:
   the legacy app does **not** touch any account for standalone Duty Expense entries. It decrements
   the selected Agent's `PaymentReceivable` and adds to the Stock's `Duty` cost instead. Verified
   directly against `btnNewDutyExpEntry_Click` in the decompiled source.
5. **`PurchaseAutoDialog.xaml`**: the "Amount Paid" field was bound to `Stock.PaidYen`, but
   `PurchaseAutoViewModel.Save()` checked `Stock.PaidAmount` — a different property that was never
   populated — to decide whether to debit the account. So purchasing a car never moved money,
   ever. Fixed by computing `PaidAmount = PaidYen * rate` before the debit check.
6. **`PurchaseAutoViewModel`**: the Agent combo in the Purchase dialog was purely cosmetic — never
   persisted. Legacy auto-creates a Duty Expense entry (tagged "Expense at purchase: ...") against
   the selected Agent when Duty > 0, and a Misc Expense entry against the account when
   MiscExpense > 0, and the account debit should include `PaidAmount + MiscExpense` (not just
   `PaidAmount`). All wired up to match. **Edit mode (existing stock) does not run any of this** —
   scoped to the Add-new-purchase path only; editing a stock's paid/duty/misc amounts still
   doesn't reconcile balances (known gap, not yet fixed).
7. **Agent Payment / Party Payment**: debited the account correctly but never updated the Agent's/
   Customer's `PaymentPaid`/`PaymentReceivable` ledger fields at all. Legacy does
   `PaymentPaid += amount, PaymentReceivable += amount` for both (this is an advance/settlement
   ledger, not a simple "they owe us" number — see caption note below). Wired up via
   `RecordAgentPayment`/`RecordCustomerPayment`.
8. **Sale**: never marked the sold Stock's `Status = 'Sold'` (so sold cars kept appearing as
   available inventory forever), and never updated the Customer's ledger. Fixed via
   `MarkStockSold` + `RecordCustomerSale` (`PaymentReceived += amountReceived`,
   `PaymentReceivable += (SalePrice - amountReceived)`).
9. **Customer name-matching bug**: `GetCustomerNames()` returns `Title || ' ' || Name` (e.g. "Mr.
   Umer Nagda") for dropdowns, but `RecordCustomerSale`/`RecordCustomerReceipt`/
   `RecordCustomerPayment` were matching `WHERE Name = @name` against the bare `Name` column — so
   the UPDATE silently matched zero rows for every customer, for all three operations. Fixed by
   matching `WHERE (Title || ' ' || Name) = @name` instead. **This was broken for a while before
   being caught — if testing shows a customer balance not moving, check this class of bug first.**
10. **Edit-mode field-wipe bug (serious, systemic)**: `AddAccountViewModel`, `AddAgentViewModel`,
    `AddCustomerViewModel`, and `PurchaseAutoViewModel` all build a fresh copy of the record being
    edited, but only copied the *editable* fields — not the *computed/running* ones
    (`CurrentBalance`, `PaymentReceivable`, `PaymentPaid`, `PaymentReceived`, `Status`). Since those
    default to 0 on the new object, simply editing an Account/Agent/Customer/Stock's basic info
    (name, phone, etc.) and hitting Save would silently **zero out its running balance** via the
    UPDATE statement. Fixed by copying those fields through in all four constructors. **This bug
    caused real data loss during testing** (an agent's payable balance was wiped mid-session) —
    there's no undo; if you hit this again, the value has to be manually reconstructed from
    transaction history.
11. **Missing `InvBoolToVis` converter** — `PaymentPkrDialog.xaml` referenced it but it was never
    registered in `App.xaml`, so the dialog would throw when opened in Party Payment mode. Added.
12. **`ReportService.cs`**: was calling `double.TryParse()` on every cell's raw string regardless
    of actual column type, so numeric-looking text (e.g. a Chassis number) got mangled into a fake
    currency-formatted number, and `DateTime` columns printed their full timestamp. Fixed to check
    `DataColumn.DataType` instead — only real `double`/`decimal`/`float` columns get `N2`
    formatting, `DateTime` columns get `dd MMM yyyy`, everything else prints as-is.
13. **Visual**: `StringFormat='PKR N2'` bindings (missing the `{0}` placeholder — WPF just prints
    the literal string when that happens) — fixed to `'PKR {0:N2}'`. The shared `OutlineButton`
    style had dark navy text/border, invisible against the dark teal dialog footers every "Cancel"
    button sits on — changed to the accent teal color, which reads on both light and dark
    backgrounds. Agent detail panel caption "Receivable:" changed to "Payment Payable:" to match
    legacy (the underlying DB column is still literally named `PaymentReceivable` — don't rename
    that without a migration, just the on-screen caption was wrong).
14. **UX**: added a Yes/No confirmation prompt (`Window_Closing` in `MainWindow.xaml.cs`) before
    the app actually exits — previously the X button closed immediately with no confirmation.

## Phase 3 — Account Statement report

The user asked for a per-account statement (date, debit, credit, running balance) and asked
whether the legacy app already has this. It does — confirmed in
`Code/Autos_Accounts/MainWindow.xaml.cs`'s `Accounts_Report()` method, backed by a `LedgerTable`
that every money-moving handler inserts a row into. Our rewrite never had an equivalent — every
`DebitAccount`/`CreditAccount` call just updated the running total with no transaction history —
so this had to be built from scratch, not just exposed.

What was added:
- **`Ledger` table** (`Date`, `Amount`, `Detail`, `Account`) in `DatabaseService.CreateTables()`.
  Also made `OpenDatabase()` call `CreateTables()` (it previously only ran on
  `CreateDatabase()`), so **existing** `.bndb` files pick up new/missing tables automatically —
  this was itself a latent gap.
- **`AddLedgerEntry` / `DebitAccountWithLedger` / `CreditAccountWithLedger`** in
  `DatabaseService.cs` — the ledger-aware wrappers now used at all 9 money-movement call sites
  (Account Transfer ×2, Misc Expense, Office Expense, Receipt, Yen Payment, Party Payment, Agent
  Payment, Purchase Auto, Sale, Withdraw Profit) instead of the raw `DebitAccount`/`CreditAccount`.
  **Sign convention matches legacy exactly**: a debit (money leaving the account) is logged as a
  *negative* ledger amount and displayed in the statement's **Credit** column; a credit (money
  entering) is logged *positive* and shown in the **Debit** column. This looks backwards to a
  non-accountant but is the standard convention for an asset/cash account (Dr increases it, Cr
  decreases it) — it's not a bug, it's what the legacy report does too.
- **`GetAccountStatement(accountName, from, to)`** in `DatabaseService.cs` — computes an opening
  balance row (`Account.OpeningBalance` + sum of all Ledger entries before `from`), then one row
  per Ledger entry in range with a running `Balance` column.
- **UI**: new "Account Statement" report type in `ReportViewModel.ReportTypes`, with an
  account-selector `ComboBox` (`ComboReportAccount` in `MainWindow.xaml`) that only appears when
  that report type is selected (`ComboReport_SelectionChanged` in `MainWindow.xaml.cs`).
- **`ReportService.cs`**: suppressed the totals-row logic for this report specifically (it
  auto-sums any column named "Balance", which is meaningless for a *running* balance — the last
  row already shows the ending balance).

**Forward-only, same caveat as everything else in this pass**: the Ledger table starts empty for
an existing database. Historical transactions made before this feature existed will not appear in
any Account Statement — only transactions recorded going forward will show up.

## Phase 4 — Credit-days payment reminders (new feature, not in legacy)

The user asked for installment-sale tracking: a customer can buy a car on credit terms (e.g. pay
over 3 months), and the app should surface a dashboard notification when the remaining balance's
due date is approaching. Checked first — **this does not exist anywhere in the legacy app**
(confirmed via grep, no "Credit"/"Reminder"/"Due" concept in `Code/Autos_Accounts`), so this was
designed fresh rather than replicated. Two design decisions were confirmed with the user before
building:
- **Credit Days lives on the Sale, not the Customer profile** — one customer can buy multiple cars
  with different terms, and the reminder is tied to a specific chassis's remaining balance.
- **Single due date, not an installment schedule** — `DueDate = SaleDate + CreditDays`. One
  reminder counts down to that one date for whatever balance is still outstanding; it is *not* a
  month-by-month installment schedule with separate due dates (that was explicitly scoped out as
  more than needed).

What was added:
- **`Sale` model** (`Models/Sale.cs`): `CreditDays`, `ReminderDaysBefore` (both default 0 = cash
  sale, no tracking), plus computed `DueDate`, `DaysUntilDue`, `HasActiveReminder` (true when
  `CreditDays > 0 && SaleBalance > 0 && DaysUntilDue <= ReminderDaysBefore` — this stays true even
  after the due date passes, i.e. overdue sales keep showing up), and `ReminderText` for display
  (e.g. "Umer Nagda — Chassis 0199991 — due in 10 days (18 Jul 2026)" or "OVERDUE by 3 days").
- **Schema migration pattern established here for the first time**: `DatabaseService.EnsureColumn`
  checks `PRAGMA table_info(table)` and runs `ALTER TABLE ... ADD COLUMN` only if missing. This was
  needed because `CREATE TABLE IF NOT EXISTS` (used for the Ledger table in Phase 3) only handles
  *new tables* — it does nothing for *new columns on a table that already exists* in someone's
  existing `.bndb` file, which is exactly the case here (`Sale` already existed). **Use
  `EnsureColumn` for any future column additions to existing tables** — don't just edit the
  `CREATE TABLE` string and assume it'll apply.
- **`SaleAutoDialog.xaml`**: two new fields, "Credit Days" and "Notify Me (Days Before Due)",
  right after the existing sale fields. Both optional/default 0.
- **UI on the Welcome page** (`MainWindow.xaml`): a "⚠ Payment Reminders" panel, hidden by default,
  shown at the top of the Welcome tab only when at least one sale has `HasActiveReminder == true`.
  Lists customer/chassis/due-status/balance for each. Populated in `RefreshAll()` (already called
  on database open/create and after any sale), so it updates automatically — no separate polling or
  background timer.

**Forward-only, same as every other feature added this session**: this only applies to sales
recorded with the new fields present. Existing sales in the database default to `CreditDays = 0`
(cash sale, no reminder) since that information was never captured before.

## Phase 5 — Credit-sales master toggle, profit-withdrawal bug, settings persistence bugs

- **`CompanySettings.EnableCreditSales`** (bool, default `true`): a Settings checkbox
  ("Enable Customer Credit Sale") that hides — but never deletes — everything credit-related when
  off: the Credit Days/Notify Me fields in `SaleAutoDialog`, the Welcome-page reminders panel, the
  "Active Credit Sales" report type, and the Sold Cars report's Credit/Cash filter. Existing credit
  sale rows stay in the database untouched; toggling back on makes them visible again.
- **Two new report additions**: "Active Credit Sales" (new report type — every sale with an
  outstanding balance and `CreditDays > 0`, showing due date and days remaining/overdue, via
  `DatabaseService.GetActiveCreditSales()`) and a Sale Type filter (All/Credit/Cash) on the existing
  "Sold Cars" report (`GetReportData` gained an optional `saleTypeFilter` param). Both only appear
  in the UI when `EnableCreditSales` is on.
- **`GetTotalProfit()` bug**: it computed gross lifetime profit from sold cars but never subtracted
  amounts already taken out via Withdraw Profit — so "Total Profit Available" never went down after
  a withdrawal, no matter how many times you withdrew. Fixed to subtract
  `SUM(Amount) FROM OfficeAccount WHERE DebitTo = 'Profit'` (the marker `WithdrawProfit()` already
  writes) from the gross figure.
- **`SettingsService.LastDatabasePath` was never actually persisted** — it was a bare in-memory
  property on `SettingsService`, not part of the `CompanySettings` object that gets XML-serialized
  to `%AppData%\AMS\settings.xml`. So the "auto-load last database" feature added in Phase 3 only
  ever worked within a single running session; every real restart lost it, which is why the app
  kept opening blank. Fixed by moving `LastDatabasePath` onto `CompanySettings` itself and proxying
  the `SettingsService` property through it (get/set + `Save()`, same pattern as `ExchangeRate`).
- **Found the same "edit-copy drops fields" bug class a third time**, this time in
  `CompanySettingsViewModel`'s constructor: it copies `CompanyName`/`CompanyAddress`/etc. into a
  fresh `CompanySettings` object but was missing `DatabasePassword` and (once added) would have
  been missing `LastDatabasePath` and `EnableCreditSales` too. Since `Save()` replaces the *entire*
  settings object, simply opening and saving Company Settings would have silently reset the
  database password and wiped the just-fixed `LastDatabasePath` back to null. Fixed by copying all
  fields through. **If you add a new field to `CompanySettings`, always check
  `CompanySettingsViewModel`'s constructor copies it too** — this is now the third time this exact
  shape of bug has appeared (see Phase 2 item 10 for the first two).
- **Multiple `.bndb` files exist side by side** in `Database/` (`accounts.bndb`,
  `accounts-new.bndb`, `StagingDB.bndb`) from different points in testing — they are *not* the same
  data. When debugging "why doesn't X show up", check which file is actually open
  (`SettingsService.Instance.LastDatabasePath` / the status bar text) before assuming a code bug.
  Direct-query approach that resolved a "wrong number" report in this session: load
  `System.Data.SQLite.dll` from the build output via
  `Add-Type -Path bin\Debug\net48\System.Data.SQLite.dll` in PowerShell and query the `.bndb` file
  directly — faster than guessing from report screenshots, and it's how the "SalePrice became 10x"
  report turned out to be a data-entry issue, not a code bug (the DB held the value the user
  actually typed).

## Phase 6 — Real installment schedules (replaces the Phase 4 single-due-date model)

After talking to the actual client, the credit-sale requirement turned out to be a real monthly
installment plan, not a single lump due date: customer pays an advance, the remaining balance is
split evenly across N months, each with its own due date, and each gets paid/marked off
individually as the customer pays month by month. This **replaces** the Phase 4 design (which had
`Sale.CreditDays` as a single "days until the whole balance is due") — that field is left orphaned
in the DB schema (harmless, just unused) rather than migrated, since only test data existed.

Two mechanics were confirmed with the user/client before building:
- Monthly payments are **not** recorded as a new Sale each month — that would mean re-entering a
  full sale every month, which the client correctly rejected. Instead there's a lightweight
  **"Pay Installment"** action on the Sale Autos screen that marks the next unpaid installment paid.
- Editing the plan (change number of remaining months / reminder days) at any time is supported.
  Already-paid installments are left untouched; only the still-unpaid remainder gets reshaped —
  **split evenly across the new month count, starting from today** (not from the original sale
  date), per explicit client confirmation.

What was built:
- **New `Installment` table** (`RowId, SaleRowId, InstallmentNumber, DueDate, Amount, IsPaid,
  PaidDate, PaidAmount, PaidInAccount`) and `Models/Installment.cs`. `Sale.CreditDays` (Phase 4) is
  replaced by `Sale.InstallmentMonths` — 0 means cash sale, same "0 = disabled" convention as
  before.
- **`DatabaseService.AddSale` now returns the new row's `RowId`** (via `SELECT last_insert_rowid()`
  right after the insert) — needed so the caller can attach an installment plan to the sale it just
  created. This is a breaking signature change from `void` — check for other callers before
  changing `AddSale` again.
- **`AddInstallmentPlan(saleRowId, startDate, totalBalance, months, startingInstallmentNumber=1)`**
  — splits `totalBalance` evenly across `months`, due dates at `startDate.AddMonths(1)`,
  `AddMonths(2)`, etc. The *last* installment absorbs any rounding remainder so the sum matches
  exactly (equal division of e.g. 100 across 3 months doesn't divide evenly).
- **`ReplanInstallments(saleRowId, newMonths, newReminderDaysBefore)`** — the "Edit Plan" logic:
  deletes unpaid installments for that sale, recomputes remaining balance as
  `SaleBalance - sum(paid installment PaidAmounts)`, regenerates `newMonths` fresh installments
  from today, continuing the `InstallmentNumber` sequence after the last paid one.
- **`PayInstallment(installment, paidDate, paidAmount, account)`** — marks it paid, and per explicit
  user direction ("single point of payment receiving record instead of consolidating here there")
  also writes a normal `Receipt` row (`AddReceipt`) and updates the customer's ledger
  (`RecordCustomerReceipt`), in addition to crediting the account (`CreditAccountWithLedger`) — so
  Receipts stays the one place that shows every payment ever received from a customer, whether it
  came in as a one-off receipt or as an installment payment. Takes the full `Installment` object
  (not just its RowId) because it needs `SaleCustomer`/`SaleChassis`/`InstallmentNumber` from the
  Sale join to build the Receipt detail text — those aren't columns on `Installment` itself.
- **UI**: `SaleAutoDialog` gained a "This is an installment / credit sale" checkbox (only shown when
  `EnableCreditSales` is on) that reveals Number of Months + reminder-days fields. Sale Autos screen
  gained "Pay Installment" and "Edit Plan" buttons, enabled only when the selected sale has an
  active plan (`InstallmentMonths > 0`) — `Views/Dialogs/PayInstallmentDialog` and
  `EditInstallmentPlanDialog` + matching ViewModels.
- **Welcome-page reminders and the "Active Credit Sales" report are now per-installment**, not
  per-sale: `DatabaseService.GetUnpaidInstallments()` joins Installment→Sale for customer/chassis/
  reminder-threshold, and each overdue/upcoming installment shows as its own line (e.g.
  "Umer Nagda — Chassis 0199991 — Installment 2/5 — due in 8 days").

**Same forward-only caveat as everything else**: existing test Sale rows from before this feature
have no Installment rows at all (nothing to display/pay against) since the schedule is generated
once at sale time. Only new installment sales recorded going forward will show up in reminders,
the Active Credit Sales report, or the Pay Installment/Edit Plan flow.

### Understanding the Agent/Customer "Payment Payable/Receivable" ledger

These are **not** simple "who owes whom" numbers — they're advance/settlement running balances:
- Paying an agent/customer money (Agent Payment / Party Payment) **increases** their
  `PaymentReceivable` — they're now holding an advance they still need to account for.
- Recording work against them (Duty Expense for agents, Receipt for customers, Sale for customers)
  **decreases/adjusts** it as that advance gets used/settled.
- If the balance goes **negative**, that's a genuine liability — money owed to them that hasn't
  been advanced yet.

## Known deferred / follow-up items (not yet done)

Explicitly deprioritized as "cosmetic, do later" per user direction:
- Button text wording doesn't match legacy exactly in several dialogs (legacy: "Save Entry"/"Save
  Payment"/"Save Account"; ours: "Enter Expense"/"Enter Payment"/"Save" — inconsistent, not fixed).
- Field order within several dialogs differs from legacy (Purchase Auto, Account Transfer, Payment
  Agent/Pkr, Sale Auto).
- No duplicate-name checks on Account/Agent/Customer creation, no CNIC format validation.
- `PurchaseAutoDialog` has no per-transaction Exchange Rate override field (uses the global
  `SettingsService.ExchangeRate` silently).
- Editing an existing Stock purchase doesn't reconcile Duty/Agent/Account balances against the old
  values before applying the new ones (would need a "reverse old, apply new" pattern to avoid
  double-counting on repeated edits — not attempted, flagged as a bigger piece of work).

## Build & run

```
cd Updated-code/AMS
dotnet build AMS.csproj -c Debug
```
Output: `bin/Debug/net48/AMS.exe`. Default login password (unless changed via Settings):
`karachi123` (see `Models/CompanySettings.cs`).

**Gotcha**: if the app is already running (e.g. launched from Visual Studio), the build will fail
with an MSB3027 file-lock error on `AMS.exe`. Close the running instance first.

## Distributable installer

`Updated-code/AMS/Installer/AMS.iss` is an Inno Setup script that packages a Release build into a
single `JainiAutoAccountsManager_Setup.exe` (Start Menu shortcut, optional desktop icon, bundled
uninstaller). To rebuild it:

```
dotnet build AMS.csproj -c Release
"C:\Users\<you>\AppData\Local\Programs\Inno Setup 6\ISCC.exe" Installer\AMS.iss
```

The build (`AMS.exe`) and the installer itself are both Authenticode-signed with a **self-signed**
"Jaini Motors" code-signing certificate (`JainiMotors_CodeSign.pfx`/`.cer`, generated locally,
**not committed to git** — `*.pfx` is gitignored on purpose since it contains the private key).
Signing command pattern:
```powershell
$cert = Get-ChildItem "Cert:\CurrentUser\My\8BACB5CBD27889842DA29DE0BFE293AEA8589585"
Set-AuthenticodeSignature -FilePath <path-to-exe> -Certificate $cert -HashAlgorithm SHA256
```
**Important limitation**: self-signing does *not* make Windows trust the binary automatically — the
client will still see an "Unknown Publisher" / SmartScreen warning unless they explicitly import
`JainiMotors_CodeSign.cer` into their Trusted Root Certification Authorities store first. Signing
only proves the file hasn't been tampered with after signing and gives a consistent publisher
identity; it is not a substitute for a CA-issued certificate if you need to fully suppress OS trust
warnings for the client.

## General debugging approach that worked well here

When something "isn't syncing" or "shows 0": check three things in order —
1. Is the `Save()` method actually calling the right `DatabaseService` method at all?
2. Does the identifier used in the `WHERE` clause (name/chassis/account) actually match what's
   stored in the DB column, or is there a Title-prefix / formatting mismatch like #9 above?
3. For edit flows specifically: does the ViewModel's "existing record" copy carry over *every*
   field the UPDATE statement writes, including computed ones? (#10 above)

When in doubt about what the legacy app is *supposed* to do, grep
`Code/Autos_Accounts/MainWindow.xaml.cs` for the relevant `btnXxx_Click`/`btnNewXxxEntry_Click`
handler — it has the exact SQL.

## Phase 7 — Installment shortfall carry-forward + cash-basis profit calculation

Two real bugs found after the client tested the Phase 6 installment feature live:

1. **Underpaying an installment silently "lost" the shortfall.** `PayInstallment` always marked
   an installment fully paid regardless of `paidAmount` vs. the scheduled `Amount`. Fixed in
   `DatabaseService.PayInstallment`: any shortfall (or overpayment) is now rolled onto the
   `Amount` of the *next* unpaid installment for that sale, so the schedule always re-balances to
   the true remaining balance. The **final** installment can no longer be marked paid for less
   than its full (carried-forward-inclusive) amount — `PayInstallment` throws
   `InvalidOperationException`, caught in `PayInstallmentViewModel.Save()` and shown as a
   validation message. `PayInstallmentViewModel.SummaryText` now also shows "Amount due" and flags
   when a payment is the final installment.

2. **"Total Profit Available" was accrual-basis, not cash-basis, and didn't distinguish accounts.**
   The old `GetTotalProfit()` summed `SalePrice - Cost` for every Sold car the moment it was
   marked sold — regardless of how much cash had actually been *collected* (a problem specifically
   for installment sales, where most of the price arrives over months). It also had no concept of
   some accounts (e.g. Petty Cash) not representing real sale proceeds.
   - Added `Account.IncludeInProfit` (bool, default `true`, migrated via
     `EnsureColumn("Account", "IncludeInProfit", "INTEGER DEFAULT 1")` — existing accounts stay
     profit-linked by default, matching prior behavior). Exposed as a checkbox in
     `AddAccountDialog.xaml` ("Include this account in profit calculation... uncheck for Petty
     Cash..."). Remember: `AddAccountViewModel`'s edit-copy constructor had to explicitly copy this
     field too — the same "edit-copy drops fields" bug class documented earlier in this file.
   - New `DatabaseService.GetProfitBreakdown()` is now the single source of truth: for every Sold
     car it sums cash actually received (initial `Sale.SaleAmountReceived` + paid
     `Installment.PaidAmount` rows) **but only when the receiving account has `IncludeInProfit =
     1`**, subtracts the car's full `Cost` (cost is a real cash outflow that already happened at
     purchase time, so it's still counted in full, not proportionally), then subtracts profit
     already withdrawn. `GetTotalProfit()` (used by the dashboard) now just reads the final "NET
     PROFIT AVAILABLE" row out of this same breakdown, so the dashboard figure and the report
     below always reconcile exactly.
   - New report type **"Profit Breakdown"** (`ReportViewModel.ReportTypes`, wired in
     `MainWindow.xaml.cs` `BtnGenerateReport_Click`): one row per sold car (Chassis, Model, Sale
     Price, Cash Collected, Cost, Profit Contribution) plus three summary rows (TOTAL, LESS:
     PROFIT ALREADY WITHDRAWN, NET PROFIT AVAILABLE). No date range applies (it's a lifetime
     figure) — `ComboReport_SelectionChanged` hides the From/To date pickers when this report is
     selected (`TxtReportFromLabel`/`DpReportFrom`/`TxtReportToLabel`/`DpReportTo`).
     `ReportService.BuildReport` skips its usual auto-computed TOTAL row for this report (flag
     `isProfitBreakdown`, same pattern as `isRunningStatement` for Account Statement) since the
     summary rows are already baked into the DataTable.

Root-caused via direct SQLite query against `StagingDB.bndb` (see debugging approach above): a test
sale (chassis `545613213521321`, cost 4,475,000, sold for 450,000) was overwhelmingly responsible
for the negative profit figure the client saw — almost certainly bad test data, not a calculation
bug. There is still no UI to edit/delete a `Sale`/`Stock` record to correct data like this — noted
as a gap, not yet requested/built.

## Phase 8 — Purchase form expense categories: Clearance (renamed Duty), Demurrage, No Plate, Commission, Tax

Client-requested, after accepting v1: the Purchase form only tracked "Duty" and "Misc Expense" as
per-car expense categories. Client wanted four more (Demurrage, No Plate, Commission, Tax), renamed
"Duty" to "Clearance" for display, and wanted every one of these to have full parity with the
existing Duty/Misc Expense features: its own field on the Purchase form, its own dedicated
entry dialog + Accounts sub-nav tab (for expenses recorded after purchase, not just at purchase
time), and its own report.

Two decisions confirmed with the client before building (see the two `AskUserQuestion` calls in
this session): (1) none of the 4 new fields are agent-linked like Duty — all four behave like Misc
Expense (a plain debit from a company Account, no agent involved); (2) all four, plus the renamed
Clearance, roll into `Stock.Cost` and therefore reduce profit, same as Duty/Misc Expense always
have.

- **"Duty" → "Clearance" is a display-only rename.** The underlying `DutyExp` table, `Stock.Duty`
  column, and all C# identifiers (`AddDutyExp`, `AdjustStockDuty`, etc.) are unchanged — only
  user-facing text changed (dialog titles, tab label, report name `"Clearance Expenses"`, Purchase
  form field label, Stock Details panel label). This avoids a schema migration and keeps the change
  low-risk; a client-visible "Duty" no longer appears anywhere in the app.
- New tables `DemurrageExp`, `NoPlateExp`, `CommissionExp`, `TaxExp` — identical shape to `MiscExp`
  (`Chassis, XDate, XAmount, XDetail, XPaidBy`, no agent column). New `Stock` columns `Demurrage`,
  `NoPlate`, `Commission`, `Tax` (migrated via `EnsureColumn`, default 0).
- **Found and fixed a pre-existing staleness bug while building this**: `AdjustStockDuty` (called
  when a *post-purchase* Duty entry is recorded via the dedicated dialog) only updated
  `Stock.Duty`, never `Stock.Cost` — so `Cost` (and therefore profit) silently went stale until the
  Stock record was next edited/saved. `MiscExp` had the same gap but worse: it never touched Stock
  at all when entered via the dedicated Misc Expense dialog, only when entered directly on the
  Purchase form. Fixed by having every `AdjustStockXxx` method (`AdjustStockDuty`,
  `AdjustStockMiscExpense` [new], `AdjustStockDemurrage`, `AdjustStockNoPlate`,
  `AdjustStockCommission`, `AdjustStockTax`) update both the category column and `Cost` in the same
  `UPDATE` statement (`Cost = Cost + @delta`), and wired `MiscExpViewModel.Save()` to call
  `AdjustStockMiscExpense` (it never did before). This matters more now than it used to: cash-basis
  profit (`GetProfitBreakdown`/`GetTotalProfit`, Phase 7) reads `Stock.Cost` directly, so a stale
  Cost silently corrupts "Total Profit Available."
- Purchase form (`PurchaseAutoDialog.xaml`/`PurchaseAutoViewModel.cs`): added Demurrage/No
  Plate/Commission/Tax fields; `Cost` formula now sums all 6 categories
  (`PricePkr + Duty + MiscExpense + Demurrage + NoPlate + Commission + Tax`); at purchase time each
  new field, if > 0, both adds into the combined account debit (same pattern Misc Expense already
  used) and creates its own `XxxExp` row tagged `"Expense at purchase: {chassis}"`. Also fixed
  another instance of the "edit-copy drops fields" bug class (documented earlier in this file) —
  `PurchaseAutoViewModel`'s edit-mode copy-constructor was missing the new fields.
- MainWindow: 4 new Accounts sub-nav tabs (Demurrage, No Plate, Commission, Tax) alongside the
  renamed "Clearance Payments" tab, each with its own Add-button + DataGrid, mirroring the
  Misc/Duty pattern exactly (`AccountsViewModel` gained matching `ObservableCollection`s,
  `LoadXxx`/`OpenXxx` methods). Stock Details panel extended with rows for all 4 new fields.
- 4 new report types (`Demurrage Expenses`, `No Plate Expenses`, `Commission Expenses`,
  `Tax Expenses`) plus the renamed `Clearance Expenses`; the `Stocks` report also now shows all 6
  expense columns.

## Phase 9 — Pay/Edit Plan buttons on the Welcome-page reminder rows

Client suggestion: the Welcome-page "Payment Reminders" panel (added in Phase 6) was read-only —
acting on a reminder meant going to Sale Autos, finding the matching sale, then clicking Pay
Installment/Edit Plan there. Added `Pay`/`Edit Plan` buttons directly on each reminder row
(`MainWindow.xaml` `LstReminders` item template), wired in `MainWindow.xaml.cs`
(`BtnPayReminder_Click`/`BtnEditReminderPlan_Click`) to open the exact same
`PayInstallmentDialog`/`EditInstallmentPlanDialog` the Sale Autos tab buttons use — the Installment
bound to the row already carries `SaleRowId`, so Edit Plan looks up the parent `Sale` via
`GetSales().FirstOrDefault(s => s.RowId == installment.SaleRowId)`. No new dialogs or DB logic;
pure UI convenience reusing Phase 6 plumbing.
