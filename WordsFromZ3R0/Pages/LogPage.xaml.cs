using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using static WordsFromZ3R0.Utils.DatabaseUtils;
using static WordsFromZ3R0.Consts.AdminConsts;
using static WordsFromZ3R0.Consts.RegisteredUserConsts;

namespace WordsFromZ3R0.Pages
{
    /// <summary>
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
        public string YourLogin;

        public LogPage()
        {
            InitializeComponent();
        }

        private void LoginLogButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DoesUserAccountExists(loginLogBox.Text))
                {
                    sql = string.Format("select idAccount, Login, AccountRole from account where Login = @login and Password = @password");
                    Command = new SQLiteCommand(sql, connection);
                    Command.Parameters.Add(new SQLiteParameter("@login", loginLogBox.Text));
                    Command.Parameters.Add(new SQLiteParameter("@password", HashPasswordIntoMd5(loginPasswordBox.Password)));
                    SQLiteDataReader sqReader = Command.ExecuteReader();
                    sqReader.Read();
                    if(sqReader.GetString(2).Equals(ADMIN_ROLE))
                    {
                        int idAccount = sqReader.GetInt32(0);
                        var mainWindow = Application.Current.MainWindow as MainWindow;
                        mainWindow.Content = new AdministratorPage(idAccount, loginLogBox.Text, ADMIN_ROLE);
                        sqReader.Close();
                    }
                    else
                    {
                        int idAccount = sqReader.GetInt32(0);
                        var mainWindow = Application.Current.MainWindow as MainWindow;
                        mainWindow.Content = new AdministratorPage(idAccount, loginLogBox.Text, REGISTERED_USER_ROLE);
                        sqReader.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Błędne dane logowania");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelLogButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Content = new MainPage();
        }
    }
}
