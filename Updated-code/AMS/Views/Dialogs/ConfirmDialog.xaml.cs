using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class ConfirmDialog : Window
    {
        public ConfirmDialog(string message, string title = "Confirm")
        {
            InitializeComponent();
            TxtTitle.Text = title;
            TxtMessage.Text = message;
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e) { DialogResult = true; Close(); }
        private void BtnNo_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
    }
}
