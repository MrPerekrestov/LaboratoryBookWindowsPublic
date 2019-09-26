using LaboratoryBook.ModifyUsersWindow;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LaboratoryBook.UserClass
{
    public class Moderator : User, IAdvancedUser
    {
        public Moderator(string userName, int userID)
        {
            this.UserName = userName;
            this.AccessID = 3;
            this.UserID = userID;
        }

        public bool ChangeUserStatus(ModifyUserModel changedUser)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);
            var commandString = $"UPDATE `users` SET `status_id` = '{changedUser.UserStatusId}'" +
                                $" WHERE (`user_id` = '{changedUser.UserId}');";

            var sqlCommand = new MySqlCommand(commandString, connection);
            try
            {
                connection.Open();
                var result = (int)sqlCommand.ExecuteNonQuery();
                if (result > 0) return true;
                return false;
            }
            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }
            
        }

        public int CreateUser(string userName, string password, int accessId)
        {
            var salt = Password.GenerateSalt();
            var hash = Password.GenerateHash(salt, password);

            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var commandString = "INSERT INTO `users` (`user_name`, `salt`, `password_hash`, `status_id`, `creator_id`) " +
                               $"VALUES ('{userName}', '{salt}', '{hash}', '{accessId}', '{this.GetUserID()}');";

            var sqlCommand = new MySqlCommand(commandString, connection);

            connection.Open();

            var result = sqlCommand.ExecuteNonQuery();

            connection.Close();
            sqlCommand?.Dispose();

            return result;
        }

        public ObservableCollection<AccessStatusModel> GetAccessStatusList()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var commandString = "SELECT * FROM access_status ORDER BY access_status_id; ";
            var sqlCommand = new MySqlCommand(commandString, connection);

            var result = new ObservableCollection<AccessStatusModel>();
            var usersTable = new DataTable();

            connection.Open();

            var dbReader = sqlCommand.ExecuteReader();
            usersTable.Load(dbReader);

            connection.Close();
            sqlCommand?.Dispose();

            foreach (DataRow row in usersTable.Rows)
            {
                var accessStatusId = (int)((sbyte)row[0]);

                var accessStatusName = (string)row[1];

                var accessStatusModel = new AccessStatusModel(accessStatusId, accessStatusName);
                result.Add(accessStatusModel);
            }

            return result;

        }

        public ObservableCollection<ModifyUserModel> GetAvailableUsers()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var commandString = "SELECT user_id, user_name, status_id FROM users " +
                                $"WHERE status_id < '{AccessID}'; ";
            var sqlCommand = new MySqlCommand(commandString, connection);

            var result = new ObservableCollection<ModifyUserModel>();
            var usersTable = new DataTable();

            connection.Open();

            var dbReader = sqlCommand.ExecuteReader();
            usersTable.Load(dbReader);

            connection.Close();
            sqlCommand?.Dispose();

            foreach (DataRow row in usersTable.Rows)
            {
                var userId = (int)row[0];
                var userName = (string)row[1];
                var statusId = (int)((sbyte)row[2]);

                var modifyUserModel = new ModifyUserModel(userId, userName, statusId);
                result.Add(modifyUserModel);
            }

            return result;
        }

        public bool RemoveUser(int userId)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var commandString = $"DELETE FROM `users` WHERE (`user_id` = '{userId}');";

            var sqlCommand = new MySqlCommand(commandString, connection);

            try
            {

                connection.Open();

                var result = sqlCommand.ExecuteNonQuery();

                return result > 0;
            }

            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }

        }
    }
}
