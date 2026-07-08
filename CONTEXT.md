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
