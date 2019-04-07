using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
    public class UserManagers
    {
        private string _connectionString;

        public UserManagers(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = hash;
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Users (Name, Email, PasswordHash) " +
                                  "VALUES (@name, @email, @hash)";
                cmd.Parameters.AddWithValue("@name", user.Name.ToString());
                cmd.Parameters.AddWithValue("@email", user.Email.ToString());
                cmd.Parameters.AddWithValue("@hash", user.PasswordHash.ToString());
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValid)
            {
                return null;
            }
            return user;
        }

        public User GetByEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT TOP 1 * FROM Users WHERE Email = @email";
                cmd.Parameters.AddWithValue("@email", email);
                connection.Open();
                var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }
                return new User
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    PasswordHash = (string)reader["PasswordHash"],
                    Email = (string)reader["Email"]
                };
            }
        }
    }
}

