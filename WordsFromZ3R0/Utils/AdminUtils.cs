using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using static WordsFromZ3R0.Utils.DatabaseUtils;

namespace WordsFromZ3R0.Utils
{
    public class AdminUtils : RegisterUserUtils
    {
        public static DataView GetAccountsData()
        {
            DataView result = null;
            try
            {
                SDA = new SQLiteDataAdapter("select * from account", connection);
                DataTable table = new DataTable();
                SDA.Fill(table);
                result = table.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return result;
        }

        public static void UpdateUserRole(string login, string role)
        {
            try
            {
                sql = string.Format("UPDATE account SET (AccountRole) = (@role) WHERE Login = '" + login + "'");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@role", role));
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void UpdateUserPasswordAndRole(string login, string role, string password)
        {
            try
            {
                sql = string.Format("UPDATE account SET (AccountRole, Password) = (@role, @password) WHERE Login = '" + login + "'");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@role", role));
                Command.Parameters.Add(new SQLiteParameter("@password", HashPasswordIntoMd5(password)));
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void DeleteUserAccount(string login)
        {
            try
            {
                sql = string.Format("DELETE FROM account WHERE Login = @login");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@login", login));
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static bool DoesCategoryExists(string categoryName)
        {
            sql = string.Format("select categoryName from category where categoryName = @catName");
            Command = new SQLiteCommand(sql, connection);
            Command.Parameters.Add(new SQLiteParameter("@catName", categoryName));
            return Command.ExecuteReader().HasRows;
        }

        public static void AddNewCategory(string categoryName)
        {
            try
            {
                sql = string.Format("INSERT INTO category(categoryName) VALUES (@catName)");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@catName", categoryName));
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static bool DoesWordsSetExists(string wordsSetName, int accountId)
        {
            sql = string.Format("select setName from wordset where setName = @setName and IdAccount = @IdAccount");
            Command = new SQLiteCommand(sql, connection);
            Command.Parameters.Add(new SQLiteParameter("@setName", wordsSetName));
            Command.Parameters.Add(new SQLiteParameter("@IdAccount", accountId));
            return Command.ExecuteReader().HasRows;
        }

        public static void AddNewWordsSet(int accountId, int categoryId, string wordsSetName)
        {
            try
            {
                sql = string.Format("INSERT INTO wordset(IdAccount, IdCategory, setName) VALUES (@IdAccount, @IdCategory, @setName)");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@IdCategory", categoryId));
                Command.Parameters.Add(new SQLiteParameter("@setName", wordsSetName));
                Command.Parameters.Add(new SQLiteParameter("@IdAccount", accountId));
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void EditCategory(string existingCategoryName, string newCategoryName)
        {
            try
            {
                sql = string.Format("UPDATE category SET (categoryName) = (@category) WHERE categoryName = '" + existingCategoryName + "'");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@category", newCategoryName));
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void DeleteCategory(string categoryName)
        {
            try
            {
                sql = string.Format("DELETE FROM category WHERE categoryName = @category");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@category", categoryName));
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void EditWordsSet(string existingWordsSetName, string newWordsSetName, int accountId)
        {
            try
            {
                sql = string.Format("UPDATE wordset SET (setName) = (@set) WHERE setName = @disableSet and IdAccount = @IdAccount");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@set", newWordsSetName));
                Command.Parameters.Add(new SQLiteParameter("@disableSet", existingWordsSetName));
                Command.Parameters.Add(new SQLiteParameter("@IdAccount", accountId));
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void DeleteWordsSet(string wordsSetName, int accountId)
        {
            try
            {
                sql = string.Format("DELETE FROM wordset WHERE setName = @set and IdAccount = @IdAccount");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@set", wordsSetName));
                Command.Parameters.Add(new SQLiteParameter("@IdAccount", accountId));
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
