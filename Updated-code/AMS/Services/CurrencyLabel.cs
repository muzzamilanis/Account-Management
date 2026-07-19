namespace AMS.Services
{
    // Single source of truth for the base/reporting currency label wherever it can't be a XAML
    // binding (code-behind strings, SQL column aliases in DatabaseService). Defaults to "PKR" via
    // CompanySettings, so every call site renders exactly as it did before this existed until the
    // client explicitly changes it in Settings.
    public static class CurrencyLabel
    {
        public static string Symbol => SettingsService.Instance.Settings.BaseCurrencySymbol;
        public static string Code => SettingsService.Instance.Settings.BaseCurrencyCode;
    }
}
