using AMS.Services;
using System.Windows;
using System.Windows.Input;

namespace AMS.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            TxtCompany.Text = SettingsService.Instance.CompanyName;
            TxtPass.Focus();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e) => TryLogin();
        private void BtnCancel_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
        private void TxtPass_KeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Return) TryLogin(); }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
            => MessageBox.Show("Please contact the vendor to register this software.", "Register",
                MessageBoxButton.OK, MessageBoxImage.Information);

        private void TryLogin()
        {
            string expected = SettingsService.Instance.Settings.LoginPassword;
            if (TxtPass.Password == expected)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                TxtError.Text = "Incorrect password. Please try again.";
                TxtError.Visibility = Visibility.Visible;
                TxtPass.Clear();
                TxtPass.Focus();
            }
        }
    }
}
