using LaboratoryBook.UserClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;

namespace LaboratoryBook.LoginWindow
{
    internal static class LoginHelper 
    {
        
        public static void WriteLoginToFile(string name)
        {
            try
            {
                var loginStringToBytes = Encoding.ASCII.GetBytes(name);
                File.WriteAllBytes("userdata.tmp", loginStringToBytes);
            }

            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    "Write name error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }

        public static string ReadLoginFromFile()
        {
            try
            {
                if (File.Exists("userdata.tmp"))
                {
                    var loginBytes = File.ReadAllBytes("userdata.tmp");
                    return Encoding.ASCII.GetString(loginBytes);
                }
            }
            catch(Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    "Load name error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
           
            return string.Empty;
        }

        //Generate hash from salt and password using SHA256 algorythm, returns hash in hex string format

        public static string GenerateHash(string salt, string password)
        {
            using (var SHA256hash = SHA256.Create())
            {
                string stringToHash = salt + password;
                var bytes = SHA256hash.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));

                var builder = new StringBuilder();
                for (var i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //Load a list of laboratory books which are accessable for current user
        public async static Task<ObservableCollection<string>> GetAvailableLaboratoryBooksAsync (string userName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);
            var sqlCommand       = new MySqlCommand
            {
                CommandText =  "SELECT db_name FROM users "+
                               $"JOIN db_users ON(select users.user_id from users where user_name = '{userName}') = db_users.user_id "+
                               "JOIN db_list ON db_users.db_id = db_list.db_id "+
                               $"WHERE user_name = '{userName}';",
                Connection = connection
            };

            var dbList = new DataTable();

            await connection.OpenAsync();                      
            dbList.Load(await sqlCommand.ExecuteReaderAsync());
            await connection.CloseAsync();           

            var result = new ObservableCollection<string>();
            
            foreach (DataRow row in dbList.Rows)
            {
                result.Add(row[0].ToString());               
            }
            
            return result;            
           
        }

        public  static ObservableCollection<string> GetAvailableLaboratoryBooks(string userName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);
            var sqlCommand = new MySqlCommand
            {
                CommandText = "SELECT db_name FROM users " +
                               $"JOIN db_users ON(select users.user_id from users where user_name = '{userName}') = db_users.user_id " +
                               "JOIN db_list ON db_users.db_id = db_list.db_id " +
                               $"WHERE user_name = '{userName}';",
                Connection = connection
            };

            var dbList = new DataTable();

            connection.Open();
            dbList.Load(sqlCommand.ExecuteReader());
            connection.Close();

            var result = new ObservableCollection<string>();

            foreach (DataRow row in dbList.Rows)
            {
                result.Add(row[0].ToString());
            }

            return result;

        }

        public async static Task<User> GetUserByNameAsync (string userName) 
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);
            var sqlCommand       = new MySqlCommand
            {
                CommandText = $"SELECT `status_id` FROM users WHERE user_name = '{userName}'; ",
                Connection  = connection
            };

            try
            {
                await connection.OpenAsync();
                var status = (sbyte)(await sqlCommand.ExecuteScalarAsync());

                sqlCommand.CommandText = $"SELECT `user_id` FROM users WHERE user_name = '{userName}'; ";
                var userID = (int)(await sqlCommand.ExecuteScalarAsync());

                switch (status)
                {
                    case 1: return new Guest(userName, userID);
                    case 2: return new Laborant(userName, userID);
                    case 3: return new Moderator(userName, userID);
                    case 4: return new Administer(userName, userID);
                    default: return null;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "Create user error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error
                            );
            }
            finally
            {
                await connection.CloseAsync();
                sqlCommand?.Dispose();
            }
            return null;
        }

        public static User GetUserByName(string userName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);
            var sqlCommand = new MySqlCommand
            {
                CommandText = $"SELECT `status_id` FROM users WHERE user_name = '{userName}';",
                Connection  = connection
            };

            try
            {
                connection.Open();
                var status = (sbyte)(sqlCommand.ExecuteScalar());

                sqlCommand.CommandText = $"SELECT `user_id` FROM users WHERE user_name = '{userName}'; ";
                var userID = (int)(sqlCommand.ExecuteScalar());

                switch (status)
                {
                    case 1: return new Guest(userName, userID);
                    case 2: return new Laborant(userName, userID);
                    case 3: return new Moderator(userName, userID);
                    case 4: return new Administer(userName, userID);
                    default: return null;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "Create user error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error
                            );
            }
            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }
            return null;
        }
    }
}
