using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using static WordsFromZ3R0.Utils.DatabaseUtils;

namespace WordsFromZ3R0.Utils
{
    public class RegisterUserUtils : NonRegisterUserUtils
    {
        public static DataView GetCategories()
        {
            DataView result = null;
            try
            {
                SDA = new SQLiteDataAdapter("select IdCategory, categoryName from category", connection);
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

        public static DataView GetAccountData(string login)
        {
            DataView result = null;
            try
            {
                SDA = new SQLiteDataAdapter("select * from account where Login = '" + login + "'", connection);
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

        public static void ChangePassword(string login, string newPassword)
        {
            try
            {
                sql = string.Format("UPDATE account SET (Password) = (@password) WHERE Login = '" + login + "'");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@password", HashPasswordIntoMd5(newPassword)));
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
