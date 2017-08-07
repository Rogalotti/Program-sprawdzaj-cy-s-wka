using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static WordsFromZ3R0.Utils.NonRegisterUserUtils;
using static WordsFromZ3R0.Consts.NonRegisteredUserConsts;

namespace WordsFromZ3R0.Pages
{
    /// <summary>
    /// Interaction logic for NonRegisterPage.xaml
    /// </summary>
    public partial class NonRegisterPage : Page
    {
        public NonRegisterPage()
        {
            InitializeComponent();
            RefreshPage();
        }

        private void RefreshPage()
        {
            countWords.Text = Convert.ToString(GetNonRegisterUserWordsCount());
            dataGridWords.ItemsSource = GetWordsForSelectedSet(NON_REGISTER_USER_WORDS_SET_ID);
        }

        private void AddWordsButton_Click(object sender, RoutedEventArgs e)
        {    
            if (String.IsNullOrEmpty(addWord1TextBox.Text) || String.IsNullOrEmpty(addWord2TextBox.Text))
            {
                MessageBox.Show("Uzupełnij pola słówek");
            }
            else
            {
                if (String.IsNullOrEmpty(idSetTexBox.Text))
                {
                    MessageBox.Show("Wybierz zestaw");
                }
                else
                {
                    AddNewWordWithTranslationIfPossible(addWord1TextBox.Text, addWord2TextBox.Text);
                    RefreshPage();
                }
            }
        }

        private void StartGameAdminButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame(NON_REGISTER_USER_ID, NON_REGISTER_USER_LOGIN, NON_REGISTER_USER_ROLE, NON_REGISTER_USER_WORDS_SET_ID);
        }

        private void DataGridWords_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid gd = (DataGrid) sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                deleteWordsTextBox.Text = row_selected["IdWords"].ToString();
            }
        }

        private void LogOutAdminButton_Click(object sender, RoutedEventArgs e)
        {
           
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Content = new MainPage();
        }

        private void DeleteWordsButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(deleteWordsTextBox.Text))
            {
                MessageBox.Show("Wybierz słówka do usunięcia");
            }
            else
            {
                DeleteWordWithTranslation(deleteWordsTextBox.Text);
                deleteWordsTextBox.Clear();
                RefreshPage();
            }
        }
    }
}
