using System;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using static WordsFromZ3R0.Consts.AdminConsts;

namespace WordsFromZ3R0.Utils
{
    public class DatabaseUtils
    {
        public static SQLiteCommand Command;
        SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
        public static string sql = "";
        public static SQLiteDataReader Reader;
        public static SQLiteDataAdapter SDA;
        public static SQLiteConnection connection = new SQLiteConnection();

        public static void InitializeDatabase()
        {
            CreateDatabaseFileIfNotExists();
            CreateAccountsTableIfNotExists();
            CreateWordsTableIfNotExists();
            CreateWordsSetsTableIfNotExists();
            CreateCategoriesTableIfNotExists();
            CreateUserAccountIfNotExists(ADMIN_LOGIN_ADN_PASSWORD, ADMIN_LOGIN_ADN_PASSWORD, ADMIN_ROLE);
        }

        private static void CreateDatabaseFileIfNotExists()
        {
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WordsBase.db")))
                connection = new SQLiteConnection(string.Format("Data Source={0}", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WordsBase.db")));   
            connection.Open();
        }

        private static void CreateAccountsTableIfNotExists()
        {
            try
            {
                sql = string.Format("create table if not exists account(IdAccount integer primary key autoincrement, AccountRole varchar(30) , Login varchar(30), Password varchar(35))");
                Command = new SQLiteCommand(sql, connection);
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void CreateWordsTableIfNotExists()
        {
            try
            {
                sql = string.Format("create table if not exists words(IdWords integer primary key autoincrement, IdSet integer, word1 varchar(30), word2 varchar(35))");
                Command = new SQLiteCommand(sql, connection);
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void CreateWordsSetsTableIfNotExists()
        {
            try
            {
                sql = string.Format("create table if not exists wordset(IdSet integer primary key autoincrement, IdAccount integer, IdCategory integer, setName varchar(30))");
                Command = new SQLiteCommand(sql, connection);
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void CreateCategoriesTableIfNotExists()
        {
            try
            {
                sql = string.Format("create table if not exists category(IdCategory integer primary key autoincrement, categoryName varchar(30))");
                Command = new SQLiteCommand(sql, connection);
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void CreateUserAccountIfNotExists(string login, string password, string role)
        {
            if (!DoesUserAccountExists(login))
                CreateUserAccount(login, password, role);
        }

        public static bool DoesUserAccountExists(string login)
        {
            sql = string.Format("select Login, AccountRole from account where Login = @login");
            Command = new SQLiteCommand(sql, connection);
            Command.Parameters.Add(new SQLiteParameter("@login", login));
            return Command.ExecuteReader().HasRows;
        }

        private static void CreateUserAccount(string login, string password, string role)
        {
            try
            {
                sql = string.Format("INSERT INTO account(AccountRole, Login, Password) VALUES (@role, @login, @password)");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@role", role));
                Command.Parameters.Add(new SQLiteParameter("@login", login));
                Command.Parameters.Add(new SQLiteParameter("@password", HashPasswordIntoMd5(password)));
                Command.ExecuteNonQuery();
                MessageBox.Show("Konto zostało utworzone");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void CloseConnection()
        {
            connection.Close();
        }

        public static string HashPasswordIntoMd5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
    }
}
