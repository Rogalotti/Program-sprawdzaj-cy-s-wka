using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SQLite;
using Microsoft.Win32;
using System.Data;
using System.IO;
using static WordsFromZ3R0.Utils.AdminUtils;
using static WordsFromZ3R0.Consts.RegisteredUserConsts;
using static WordsFromZ3R0.Consts.AdminConsts;
using static WordsFromZ3R0.Utils.RegisterUserUtils;
using static WordsFromZ3R0.Utils.NonRegisterUserUtils;
using static WordsFromZ3R0.Utils.DatabaseUtils;

namespace WordsFromZ3R0.Pages
{
    /// <summary>
    /// Interaction logic for AdministratorPage.xaml
    /// </summary>
    public partial class AdministratorPage : Page
    {
        private int idAccount;
        private string nameUser;
        private string accountRole;

        public AdministratorPage(int idAccount, string nameUser, string accountRole)
        {
            InitializeComponent();
            this.nameUser = nameUser;
            this.accountRole = accountRole;
            this.idAccount = idAccount;
            FillDataGrid();
            dataGridCategory.ItemsSource = GetCategories();
            DisableAdministratorFunctionalitiesIfRegularUser();
        }

        private void DisableAdministratorFunctionalitiesIfRegularUser()
        {
            if (accountRole.Equals(REGISTERED_USER_ROLE))
            {
                addCategoryButton.IsEnabled = false;
                editCategoryButton.IsEnabled = false;
                deleteCategoryButton.IsEnabled = false;
                addCategoryTextBox.IsEnabled = false;
                editCategoryTextBox.IsEnabled = false;
                deleteCategoryTextBox.IsEnabled = false;
                comboBoxRole.IsEnabled = false;
                insertButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
            }
        }

        private void FillDataGrid()
        {
                if (accountRole.Equals(ADMIN_ROLE))
                {
                dataGrid.ItemsSource = GetAccountsData();
                }
                else
                {
                dataGrid.ItemsSource = GetAccountData(nameUser);
                }
        }

        private void DataGrid_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                idLabel.Content = row_selected["IdAccount"].ToString();
                comboBoxRole.Text = row_selected["AccountRole"].ToString();
                loginAddtextBox.Text = row_selected["Login"].ToString();
                passwordAddBox.Password = row_selected["Password"].ToString();
                passwordBox1.Password = row_selected["Password"].ToString();
            }

        }

        private void insertButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(loginAddtextBox.Text) || String.IsNullOrEmpty(passwordAddBox.Password))
            {
                MessageBox.Show("Nie wszystkie pola są wypełnione");
            }
            else
            {
                CreateUserAccountIfNotExists(loginAddtextBox.Text, passwordAddBox.Password, comboBoxRole.Text);
                FillDataGrid();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(loginAddtextBox.Text) || String.IsNullOrEmpty(passwordAddBox.Password))
            {
                MessageBox.Show("Nie wszystkie pola są wypełnione");
            }
            else
            {
                if (accountRole.Equals(ADMIN_ROLE))
                {
                    if (passwordAddBox.Password == passwordBox1.Password)
                    {
                        UpdateUserRole(loginAddtextBox.Text, comboBoxRole.Text);
                        MessageBox.Show("Konto zostalo zmienione");
                        FillDataGrid();
                    }
                    else
                    {
                        UpdateUserPasswordAndRole(loginAddtextBox.Text, comboBoxRole.Text, passwordAddBox.Password);
                        MessageBox.Show("Konto zostalo zmienione");
                        FillDataGrid();
                    }
                }
                else
                {
                    if(!passwordAddBox.Password.Equals(passwordBox1.Password))
                    {
                        ChangePassword(loginAddtextBox.Text, passwordAddBox.Password);
                        MessageBox.Show("Konto zostalo zmienione");
                        FillDataGrid();
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(loginAddtextBox.Text) || String.IsNullOrEmpty(passwordAddBox.Password))
            {
                MessageBox.Show("Nie wszystkie pola są wypełnione");
            }
            else
            {
                if (DoesUserAccountExists(loginAddtextBox.Text))
                {
                    MessageBox.Show("Użytkownik o podanej nazwie nie istnieje");
                    loginAddtextBox.Clear();
                }
                else
                {
                    if (nameUser == loginAddtextBox.Text)
                    {
                        MessageBox.Show("Nie możesz usunąć samego siebie");
                    }
                    else
                    {
                        DeleteUserAccount(loginAddtextBox.Text);
                        MessageBox.Show("Konto zostalo usunięte");
                        FillDataGrid();
                    }
                }
            }
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(addCategoryTextBox.Text))
            {
                MessageBox.Show("Podaj nazwę kategori");
            }
            else
            {
                if (DoesCategoryExists(addCategoryTextBox.Text))
                {
                    MessageBox.Show("Kategoria o podanej nazwie już istnieje");
                    addCategoryTextBox.Clear();
                }
                else
                {
                    AddNewCategory(addCategoryTextBox.Text);
                    MessageBox.Show("Kategoria została utworzona");
                    dataGridCategory.ItemsSource = GetCategories();
                    dataGridSet.ItemsSource = null;
                    dataGridWords.ItemsSource = null;
                }
            }
            addCategoryTextBox.Clear();
            editDisableCategoryTextBox.Clear();
            deleteCategoryTextBox.Clear();
        }

        private void FillDataGridSet(int id, int idAccount)
        {
            try
            {
                if (accountRole.Equals(ADMIN_ROLE))
                {
                    SDA = new SQLiteDataAdapter("select idSet, SetName from wordset where IdCategory= " + id + "", connection);
                    DataTable DATA = new DataTable();
                    SDA.Fill(DATA);
                    dataGridSet.ItemsSource = DATA.DefaultView;
                }
                else
                {
                    SDA = new SQLiteDataAdapter("select idSet, SetName from wordset where IdAccount= " + idAccount + " and IdCategory= "+ id +"", connection);
                    DataTable DATA = new DataTable();
                    SDA.Fill(DATA);
                    dataGridSet.ItemsSource = DATA.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillDataGridWords(int id)
        {
            GetWordsForSelectedSet(id);
        }

        private void DataGridCategory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                editDisableCategoryTextBox.Text = row_selected["categoryName"].ToString();
                deleteCategoryTextBox.Text = row_selected["categoryName"].ToString();
                idCategoryTextBox.Text = row_selected["IdCategory"].ToString();
                idSetTexBox.Clear();
                deleteWordsTextBox.Clear();
                int idCat = 0;
                Int32.TryParse(idCategoryTextBox.Text, out idCat);
                FillDataGridSet(idCat, idAccount);
                dataGridWords.ItemsSource = null;
            }
        }

        private void DataGridSet_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                editDisableSetTextBox.Text = row_selected["setName"].ToString();
                deleteSetTextBox.Text = row_selected["setName"].ToString();
                idSetTexBox.Text = row_selected["IdSet"].ToString();
                deleteWordsTextBox.Clear();
                int idSet = 0;
                Int32.TryParse(idSetTexBox.Text, out idSet);
                FillDataGridWords(idSet);
            }
        }

        private void AddSetButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(addSetTextBox.Text))
            {
                MessageBox.Show("Podaj nazwę zestawu");
            }
            else
            {
                if (DoesWordsSetExists(addSetTextBox.Text, idAccount))
                {
                    MessageBox.Show("Zestaw o podanej nazwie już istnieje");
                    addSetTextBox.Clear();
                }
                else
                {
                    if (String.IsNullOrEmpty(editDisableCategoryTextBox.Text))
                    {
                        MessageBox.Show("Wybierz kategorie");
                    }
                    else
                    {
                        int idCat = 0;
                        int.TryParse(idCategoryTextBox.Text, out idCat);
                        AddNewWordsSet(idAccount, idCat, addSetTextBox.Text);
                        MessageBox.Show("Zestaw został utworzony");
                        FillDataGridSet(idCat, idAccount);
                        dataGridWords.ItemsSource = null;
                    }
                }
            }
            addSetTextBox.Clear();
            editDisableSetTextBox.Clear();
            deleteSetTextBox.Clear();
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
                    int idSet = 0;
                    int.TryParse(idSetTexBox.Text, out idSet);
                    AddNewWordWithTranslation(addWord1TextBox.Text, addWord2TextBox.Text, idSet);
                    addWord1TextBox.Clear();
                    addWord2TextBox.Clear();
                }
            }
        }

        private void EditCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(editCategoryTextBox.Text))
            {
                MessageBox.Show("Uzupełnij pole");
            }
            else
            {
                if (DoesCategoryExists(editCategoryTextBox.Text))
                {
                    MessageBox.Show("Kategoria o podanej nazwie już istnieje");
                    editCategoryTextBox.Clear();
                }
                else
                {
                    EditCategory(editDisableCategoryTextBox.Text, editCategoryTextBox.Text);
                    MessageBox.Show("Kategoria została zmieniona");
                    dataGridCategory.ItemsSource = GetCategories();
                    dataGridSet.ItemsSource = null;
                    dataGridWords.ItemsSource = null;
                    editCategoryTextBox.Clear();
                    editDisableCategoryTextBox.Clear();
                    deleteCategoryTextBox.Clear();
                }
            }
        }

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(deleteCategoryTextBox.Text))
            {
                MessageBox.Show("Uzupełnij pole");
            }
            else
            {
                if (DoesCategoryExists(deleteCategoryTextBox.Text))
                {
                    MessageBox.Show("Kategoria o podanej nazwie nie istnieje");
                    deleteCategoryTextBox.Clear();
                }
                else
                {
                    DeleteCategory(deleteCategoryTextBox.Text);
                    MessageBox.Show("Kategoria została usunięta");
                    dataGridCategory.ItemsSource = GetCategories();
                    dataGridSet.ItemsSource = null;
                    dataGridWords.ItemsSource = null;
                    deleteCategoryTextBox.Clear();
                    editDisableCategoryTextBox.Clear();
                }
            }
        }

        private void EditSetButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(editSetTextBox.Text))
            {
                MessageBox.Show("Uzupełnij pole");
            }
            else
            {
                if (DoesWordsSetExists(editSetTextBox.Text, idAccount))
                {
                    MessageBox.Show("Zestaw o podanej nazwie już istnieje");
                    editSetTextBox.Clear();
                }
                else
                {
                    EditWordsSet(editDisableSetTextBox.Text, editSetTextBox.Text, idAccount);
                    MessageBox.Show("Zestaw został zmieniony");
                    editSetTextBox.Clear();
                    editDisableSetTextBox.Clear();
                    deleteSetTextBox.Clear();
                    int idCat = 0;
                    Int32.TryParse(idCategoryTextBox.Text, out idCat);
                    FillDataGridSet(idCat, idAccount);
                }
            }
        }

        private void DeleteSetButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(deleteSetTextBox.Text))
            {
                MessageBox.Show("Uzupełnij pole");
            }
            else
            {
                if (DoesWordsSetExists(deleteSetTextBox.Text, idAccount))
                {
                    MessageBox.Show("Zestaw o podanej nazwie nie istnieje");
                    deleteSetTextBox.Clear();
                }
                else
                {
                    int idCat = 0;
                    int.TryParse(idCategoryTextBox.Text, out idCat);
                    DeleteWordsSet(deleteSetTextBox.Text, idAccount);
                    MessageBox.Show("Zestaw został usunięty");
                    dataGridCategory.ItemsSource = GetCategories();
                    FillDataGridSet(idCat, idAccount);
                    dataGridWords.ItemsSource = null;
                    deleteSetTextBox.Clear();
                    editDisableSetTextBox.Clear();
                }
            }
        }

        private void DataGridWords_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                deleteWordsTextBox.Text = row_selected["IdWords"].ToString();
            }
        }

        private void DeleteWordsButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(deleteWordsTextBox.Text))
            {
                MessageBox.Show("Wybierz słówka do usunięcia");
            }
            else
            {
                int idSet = 0;
                Int32.TryParse(idSetTexBox.Text.ToString(), out idSet);
                DeleteWordWithTranslation(deleteWordsTextBox.Text);
                FillDataGridWords(idSet);
                deleteWordsTextBox.Clear();
            }
        }

        private void LogOutAdminButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Content = new MainPage();
        }

        private void StartGameAdminButton_Click(object sender, RoutedEventArgs e)
        {
            int idSet = 0;
            Int32.TryParse(idSetTexBox.Text.ToString(), out idSet);
                StartGame(idAccount, nameUser, accountRole, idSet);
        }

        private void AddFileWordsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "Pliki textowe (txt)|*.txt";
            string sFilenames = "";
            Nullable<bool> dialogOK = fileDialog.ShowDialog();
            if (dialogOK == true)
            {

                foreach (string sFilename in fileDialog.FileNames)
                {
                    sFilenames += ";" + sFilename;
                }
                sFilenames = sFilenames.Substring(1);   // scieżka pliku
            }
            List<string> words = new List<string>();
            words = File.ReadAllLines(sFilenames).ToList();        //tworzymy tablicę i wczytujemy do niej linia po lini
            if (String.IsNullOrEmpty(idSetTexBox.Text))
            {
                MessageBox.Show("Wybierz zestaw");
            }
            else
            {
                int licznik1 = 0;
                int licznik2 = 1;
                int idSet = 0;
                int.TryParse(idSetTexBox.Text, out idSet);
                for (int i = 1; i <= words.Count; i = i + 2)
                {
                    sql = string.Format("INSERT INTO words(IdSet, word1, word2) VALUES (@IdSet, @Word1, @Word2)");
                    Command = new SQLiteCommand(sql, connection);                                // dodajemy słówka z pliku tekstowego do bazy
                    Command.Parameters.Add(new SQLiteParameter("@IdSet", idSet));
                    Command.Parameters.Add(new SQLiteParameter("@Word1", words[licznik1]));
                    Command.Parameters.Add(new SQLiteParameter("@Word2", words[licznik2]));
                    Command.ExecuteNonQuery();
                    licznik1 = licznik1 + 2;
                    licznik2 = licznik2 + 2;
                }
                FillDataGridWords(idSet);
                addWord1TextBox.Clear();
                addWord2TextBox.Clear();
            }
        }
    }
}
