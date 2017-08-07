using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using static WordsFromZ3R0.Utils.DatabaseUtils;
using static WordsFromZ3R0.Consts.NonRegisteredUserConsts;
using WordsFromZ3R0.Pages;

namespace WordsFromZ3R0.Utils
{
    public class NonRegisterUserUtils
    {
        public static void AddNewWordWithTranslationIfPossible(string word, string translation)
        {
            if (GetNonRegisterUserWordsCount() < NON_REGISTER_USER_WORDS_LIMIT)
            {
                AddNewWordWithTranslation(word, translation, NON_REGISTER_USER_WORDS_SET_ID);
            }
            else
            {
                MessageBox.Show("Maksymalny limit słówek dla niezarejestrowanego użytkownika wynosi 30");
            }
        }

        public static int GetNonRegisterUserWordsCount()
        {
            SQLiteDataReader reader = null;
            try
            {
                sql = string.Format("SELECT count(IdSet) FROM words where IdSet = @IdSet");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@IdSet", NON_REGISTER_USER_WORDS_SET_ID));
                reader = Command.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return reader.RecordsAffected;
        }

        public static void AddNewWordWithTranslation(string word, string translation, int wordsSetId)
        {
            try
            {
                sql = string.Format("INSERT INTO words(IdSet, word1, word2) VALUES (@IdSet, @Word1, @Word2)");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@IdSet", wordsSetId));
                Command.Parameters.Add(new SQLiteParameter("@Word1", word));
                Command.Parameters.Add(new SQLiteParameter("@Word2", translation));
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void DeleteWordWithTranslation(string wordId)
        {
            try
            {
                sql = string.Format("DELETE FROM words WHERE IdWords = @IdWords");
                Command = new SQLiteCommand(sql, connection);
                Command.Parameters.Add(new SQLiteParameter("@IdWords", wordId));
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static DataView GetWordsForSelectedSet(int wordsSetId)
        {
            DataView result = null;
            try
            {
                SDA = new SQLiteDataAdapter("select IdWords, word1, word2 from words where IdSet =" + wordsSetId + "", connection);
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

        public static void StartGame(int accountId, string login, string role, int wordsSetId)
        {
            try
            {
                sql = string.Format("select IdWords, word1, word2 from words where IdSet =" + wordsSetId + "");
                Command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = Command.ExecuteReader();
                if (!reader.HasRows)
                {
                    MessageBox.Show("Zestaw nie zawiera słówek");
                }
                else
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow.Content = new StartWordsPage(accountId, login, role, wordsSetId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}