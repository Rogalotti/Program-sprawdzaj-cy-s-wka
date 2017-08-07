using System;
using System.Windows;
using System.Windows.Controls;
using static WordsFromZ3R0.Utils.DatabaseUtils;
using static WordsFromZ3R0.Consts.RegisteredUserConsts;

namespace WordsFromZ3R0.Pages
{
    /// <summary>
    /// Interaction logic for RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void CancelRegButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Content = new MainPage();
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(regLoginBox.Text) || String.IsNullOrEmpty(regPasswordBox.Password))
                {
                    MessageBox.Show("Uzupełnij wszystkie pola");
                }
                else
                {
                    if (regPasswordBox.Password.Equals(regPasswordBox2.Password))
                    {
                        CreateUserAccountIfNotExists(regLoginBox.Text, regPasswordBox.Password, REGISTERED_USER_ROLE);
                        MessageBox.Show("Konto zostało utworzone");
                        var mainWindow = Application.Current.MainWindow as MainWindow;
                        mainWindow.Content = new MainPage();
                    }
                    else
                    {
                        MessageBox.Show("Hasłą nie są takie same");
                        regPasswordBox.Clear();
                        regPasswordBox2.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
