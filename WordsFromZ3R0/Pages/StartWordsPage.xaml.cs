using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using System.Windows.Threading;
using static WordsFromZ3R0.Utils.DatabaseUtils;
using static WordsFromZ3R0.Consts.NonRegisteredUserConsts;

namespace WordsFromZ3R0.Pages
{
    /// <summary>
    /// Interaction logic for StartWordsPage.xaml
    /// </summary>
    public partial class StartWordsPage : Page
    {
        Random rnd = new Random();
        int idSet;
        int idAccount = 0;
        int idWord = 0;
        int kolejnosc = 0;
        int counter = 0;
        int order = 1;
        int wrongAnswer = 0;
        int goodAnswer = 0;
        string name;
        string account;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        List<string> word1 = new List<string>();
        List<string> word2 = new List<string>();


        public StartWordsPage(int idAccount, string nameadmin, string accountrole, int idSet)
        {
            InitializeComponent();
            this.idSet = idSet;
            this.idAccount = idAccount;
            name = nameadmin;
            account = accountrole;
            Question(idSet);
            ProgressWords.Maximum = word1.Count();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private void Question(int idSet)
        {
            sql = string.Format("select IdWords, word1, word2 from words where IdSet =" + idSet + "");
            Command = new SQLiteCommand(sql, connection);
            var question = Command.ExecuteScalar();
            if (question != null)
            {
                SQLiteDataReader sqReader = Command.ExecuteReader();
                while (sqReader.Read())
                {
                    word1.Add(sqReader.GetString(1));
                    word2.Add(sqReader.GetString(2));
                    counter++;
                }
                sqReader.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (kolejnosc == 0)
            {

                if (textBox.Text == word2[idWord])
                {
                    goodAnswer++;
                    oklabel.Content = Convert.ToString(goodAnswer);
                    okclick.Opacity = 1;
                    dispatcherTimer.Start();
                    word1.RemoveAt(idWord);
                    word2.RemoveAt(idWord);
                    ProgressWords.Value = order;
                    order++;
                    counter = counter - 1;
                    if (ProgressWords.Value == ProgressWords.Maximum)
                    {
                        MessageBox.Show("Ukończyłeś wszystkie słówka");
                        if (name.Equals(NON_REGISTER_USER_LOGIN))
                        {
                            var mainWindow = Application.Current.MainWindow as MainWindow;
                            mainWindow.Content = new MainPage();
                        }
                        else
                        {
                            var mainWindow = Application.Current.MainWindow as MainWindow;
                            mainWindow.Content = new AdministratorPage(idAccount, name, account);
                        }
                    }
                    else
                    {

                        RandomWords(counter);
                    }
                }
                else
                {
                    wrongAnswer++;
                    xlabel.Content = Convert.ToString(wrongAnswer);
                    xclick.Opacity = 1;
                    dispatcherTimer.Start();
                    MessageBox.Show(word1[idWord] + " = " + word2[idWord], "Odpowiedź");
                    RandomWords(counter);
                }
            }
            else
            {
                if (textBox.Text == word1[idWord])
                {
                    goodAnswer++;
                    oklabel.Content = Convert.ToString(goodAnswer);
                    okclick.Opacity = 1;
                    dispatcherTimer.Start();
                    word1.RemoveAt(idWord);
                    word2.RemoveAt(idWord);
                    ProgressWords.Value = order;
                    order++;
                    counter = counter - 1;
                    if (ProgressWords.Value == ProgressWords.Maximum)
                    {
                        MessageBox.Show("Ukończyłeś wszystkie słówka");
                        if (name.Equals(NON_REGISTER_USER_LOGIN))
                        {
                            var mainWindow = Application.Current.MainWindow as MainWindow;
                            mainWindow.Content = new MainPage();
                        }
                        else
                        {
                            var mainWindow = Application.Current.MainWindow as MainWindow;
                            mainWindow.Content = new AdministratorPage(idAccount, name, account);
                        }
                        
                    }
                    else
                    {
                        RandomWords(counter);
                    }
                }
                else
                {
                    wrongAnswer++;
                    xlabel.Content = Convert.ToString(wrongAnswer);
                    xclick.Opacity = 1;
                    dispatcherTimer.Start();
                    MessageBox.Show(word2[idWord] + " = " + word1[idWord], "Odpowiedź");
                    RandomWords(counter);
                }
            }
            textBox.Clear();
        }

        private void startWordsbutton_Click(object sender, RoutedEventArgs e)
        {
            RandomWords(counter);
            startWordsbutton.IsEnabled = false;
            button.IsEnabled = true;
        }

        private void RandomWords(int ilosc)
        {
            idWord = rnd.Next(0, ilosc);
            kolejnosc = rnd.Next(0, 2);
            if (kolejnosc == 0)
            {
                textBox1.Text = word1[idWord];
            }
            else
            {
                textBox1.Text = word2[idWord];
            }
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            xclick.Opacity = 0;
            okclick.Opacity = 0;
        }

        private void cancelWordsbutton_Click(object sender, RoutedEventArgs e)
        {
            if (name.Equals(NON_REGISTER_USER_LOGIN))
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.Content = new NonRegisterPage();
            }
            else
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.Content = new AdministratorPage(idAccount,name, account);
            }
        }
    }
}