# AMS WPF Application — Modern UI Upgrade Plan

## Background

**Project**: Jaini Auto Accounts Manager (`Autos_Accounts`)  
**Type**: WPF Desktop Application — **.NET Framework 4.8**, x86  
**Current Tech Stack**:
- Target Framework: `.NET Framework 4.8`
- UI: WPF / XAML (hand-coded, no MVVM)
- Database: SQLite via `System.Data.SQLite.dll` (local DLL reference)
- 3rd Party: `Xceed.Wpf.Toolkit.dll`, `CodeReason.Reports.dll`, `AES256.dll`
- Architecture: Code-behind heavy, monolithic `MainWindow.xaml` (2001 lines XAML, 371KB CS)

**Screens / Windows**:
- `UserLogin` — password prompt
- `MainWindow` — main shell with left-sidebar TabControl + nested tabs (Accounts, A/c-to-A/c, Misc Expense, Duty Payments, Office Expense, etc.)
- `AddAccount`, `AddAgentForm`, `AddCustomer`, `AccounttoAccount`
- `PurchaseAuto`, `SaleAuto`
- `DutyExpEntry`, `MiscExpEntry`, `OfficeExpEntry`, `PaymentAgentEntry`, `PaymentPkrEntry`, `ReceiptEntry`

---

## Upgrade Strategy

> [!IMPORTANT]
> The upgrade is **purely a UI/styling modernization** on a **new git branch**. All existing business logic (C# code-behind), data bindings, SQLite queries, and control names are **preserved as-is**. No functionality is broken.

### What will change
1. **New Git branch** — `ui-upgrade` branched from `main`
2. **Migrate project to SDK-style `.csproj`** targeting **.NET 8.0** (WPF is fully supported). This brings modern tooling, NuGet, and faster builds.
3. **Replace 3rd-party DLL references** with NuGet equivalents:
   - `System.Data.SQLite` → `System.Data.SQLite.Core` (NuGet)
   - `Xceed.Wpf.Toolkit` → `Extended.Wpf.Toolkit` (NuGet, community fork, same namespace)
   - `CodeReason.Reports` — keep local DLL reference (no NuGet equivalent)
   - `AES256` — keep local DLL reference
4. **Complete visual redesign** via `dictionary1.xaml` (the global ResourceDictionary):
   - Modern dark-mode color palette (deep navy/slate with vibrant accents)
   - Rounded `CornerRadius` on buttons, inputs, cards
   - Smooth hover/press animations using `Storyboard` + `BeginStoryboard`
   - Modern `Button` style with subtle glow on hover
   - Better `DataGrid` styling: alternating row colors, styled headers
   - Modern `TextBox` / `ComboBox` / `DatePicker` styles
   - Modern `TabItem` sidebar with icon-friendly layout
   - Google Font embedding (Inter/Segoe UI Variable) or system font upgrade
5. **`UserLogin.xaml`** — modern centered card-style login dialog (frosted glass aesthetic)
6. **`MainWindow.xaml` header** — modernize app title, subtitle, layout spacing
7. **Dialog windows** (`AddAccount`, `AddCustomer`, `AddAgentForm`, etc.) — modernize form layout spacing and appearance

### What will NOT change
- All `Name` attributes (control names used in code-behind)
- All data bindings (`{Binding ...}`)
- All event handler references
- `MainWindow.xaml.cs` business logic (371KB — not touched)
- Database schema / SQLite
- Application settings (`MySettings`)
- All existing helper classes

---

## Open Questions

> [!IMPORTANT]
> **Framework target**: Upgrading to .NET 8 offers the best long-term support and modern tooling. However it requires some migration testing. Would you prefer:
> - (A) **Stay on .NET Framework 4.8** — zero risk, just style upgrade
> - (B) **Upgrade to .NET 8.0 WPF** — modern, but needs testing with local DLLs

> [!NOTE]
> The `CodeReason.Reports.dll` and `AES256.dll` are referenced as local files from `..\..\`. These may need to be re-tested under .NET 8. If you prefer option A (.NET 4.8), there is zero risk of compatibility issues.

---

## Proposed Changes

### Branch & Git

#### Create `ui-upgrade` branch from current `main`
- `git checkout -b ui-upgrade`

---

### Project File

#### [MODIFY] [Autos_Accounts.csproj](file:///D:/Muzzamil/Personal/AMS/Account-Management/Code/Autos_Accounts/Autos_Accounts.csproj)
- Optionally convert to SDK-style project format (only if .NET 8 is chosen)
- Or stay on Framework 4.8 and add NuGet package restore for SQLite and Xceed

---

### Visual Redesign (Core)

#### [MODIFY] [dictionary1.xaml](file:///D:/Muzzamil/Personal/AMS/Account-Management/Code/Autos_Accounts/dictionary1.xaml)
Complete rewrite of all styles:
- **Color palette**: Deep navy `#0F172A` background, `#1E293B` surface, `#38BDF8` accent (sky blue), `#F1F5F9` text
- **Button**: Rounded corners (`CornerRadius="6"`), smooth color transition on hover via `Storyboard`, accent background, disabled state
- **TextBox / ComboBox / DatePicker**: Rounded border, focus glow effect, consistent height
- **DataGrid**: Clean alternating rows, bold styled headers with gradient, selection highlight
- **TabItem (sidebar)**: Active indicator bar on left, icon spacing, hover state
- **TabControl**: Modernize sidebar panel color
- **ListViewItem**: Hover highlight, modern padding
- **Label / TextBlock**: Consistent typography, `Segoe UI Variable` or `Segoe UI`

---

### Login Screen

#### [MODIFY] [UserLogin.xaml](file:///D:/Muzzamil/Personal/AMS/Account-Management/Code/Autos_Accounts/UserLogin.xaml)
- Increase window size to 400×280
- Card-style centered form with dark background
- App logo/icon area at top
- Password field with modern rounded styling
- Modern OK/Cancel buttons with accent color

---

### Main Window Header

#### [MODIFY] [MainWindow.xaml](file:///D:/Muzzamil/Personal/AMS/Account-Management/Code/Autos_Accounts/MainWindow.xaml)
- Modernize header row (app title, exchange rate display)
- Update footer (developer credit)
- Keep all control names and bindings intact

---

### Dialog Windows (Form Windows)

These are secondary windows opened from code-behind. Light modernization:
- **[MODIFY]** `AddAccount.xaml`
- **[MODIFY]** `AddAgentForm.xaml`
- **[MODIFY]** `AddCustomer.xaml`
- **[MODIFY]** `AccounttoAccount.xaml`
- **[MODIFY]** `PurchaseAuto.xaml`
- **[MODIFY]** `SaleAuto.xaml`

Changes: Consistent padding, rounded corners, title bar styling, form layout improvements.

---

## Verification Plan

### Manual Verification
1. Build solution in Visual Studio (`Ctrl+Shift+B`) — should build with **0 errors**
2. Launch app → Login screen appears modernized
3. Navigate all sidebar tabs — UI renders correctly, no missing styles
4. Open all dialogs (Add Account, Add Customer, Purchase, Sale, etc.)
5. All data entry operations work (enter data, save, edit)
6. Reports generate correctly
7. Verify on 1080p and 1440p screens (DPI-aware layout)

### Build Validation
```
cd "D:\Muzzamil\Personal\AMS\Account-Management\Code"
msbuild Autos_Accounts.sln /p:Configuration=Debug
```
