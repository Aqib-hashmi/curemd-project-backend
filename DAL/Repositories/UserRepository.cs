using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackOverFlowReplica.Context;
using StackOverFlowReplica.StackOverFlowReplica.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace StackOverFlowReplica.DAL.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public int RegisterUser(User user)
        {
            int userId = 0;

            var connection = _db.Database.GetDbConnection();
            try
            {

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "RegisterUser";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Name", user.Name));
                    command.Parameters.Add(new SqlParameter("@Email", user.Email));
                    command.Parameters.Add(new SqlParameter("@Password", user.Password));
                    command.Parameters.Add(new SqlParameter("@Bio", user.Bio));
                    command.Parameters.Add(new SqlParameter("RoleId", 1));
                    command.Parameters.Add(new SqlParameter("isActive", 1));
                    command.Parameters.Add(new SqlParameter("isActiveBy", 1));




                    if (connection.State == ConnectionState.Closed)
                    {

                        connection.Open();
                    }

                    var result = command.ExecuteScalar();

                    if (result != null)
                    {

                        userId = Convert.ToInt32(result);
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); // 👈 important
            }

            return userId;
        }


        public User? GetUserByEmail(string email)
        {
            var connection = _db.Database.GetDbConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "LoginUserByEmail";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Email", email));

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            Name = reader["Name"].ToString() ?? "",
                            Password = reader["Password"].ToString() ?? "",
                            Email = reader["Email"].ToString() ?? "",
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            Bio = reader["Bio"].ToString() ?? "",
                            isActive = Convert.ToBoolean(reader["isActive"])
                        };
                    }
                }

                return null; // Email not found
            }
        }
        public User? GetUserById(int userId)
        {
            var connection = _db.Database.GetDbConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetUserProfile"; // SP in DB
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@UserId", userId));

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : 0,
                            Name = reader["Name"].ToString() ?? "",
                            Password = reader["Password"].ToString() ?? "",
                            Email = reader["Email"].ToString() ?? "",
                            RoleId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["RoleId"]) : 0,
                            Bio = reader["Bio"].ToString() ?? "",
                            isActive = reader["isActive"] != DBNull.Value ? Convert.ToBoolean(reader["isActive"]) : false,
                            isActiveBy = reader["isActiveBy"] != DBNull.Value ? Convert.ToInt32(reader["isActiveBy"]) : 0,
                            CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue,
                            UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedDate"]) : DateTime.MinValue
                        };
                    }
                }
                return null;

            }
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            var connection = _db.Database.GetDbConnection();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "GetAllUsers"; // SP in DB
                    command.CommandType = CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : 0,
                                Name = reader["Name"].ToString() ?? "",
                                Email = reader["Email"].ToString() ?? "",
                                Password = reader["Password"].ToString() ?? "",
                                RoleId = reader["RoleId"] != DBNull.Value ? Convert.ToInt32(reader["RoleId"]) : 0,
                                Bio = reader["Bio"].ToString() ?? "",
                                isActive = reader["isActive"] != DBNull.Value ? Convert.ToBoolean(reader["isActive"]) : false,
                                isActiveBy = reader["isActiveBy"] != DBNull.Value ? Convert.ToInt32(reader["isActiveBy"]) : 0,
                            });
                        }
                    }
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return users;
        }

        // DAL/Repositories/UserRepository.cs
        public bool UpdateUser(User user)
        {
            var connection = _db.Database.GetDbConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UpdateUser"; // SP name
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@UserId", user.UserId));
                command.Parameters.Add(new SqlParameter("@Name", user.Name ?? ""));
                command.Parameters.Add(new SqlParameter("@Email", user.Email ?? ""));
                command.Parameters.Add(new SqlParameter("@Bio", user.Bio ?? ""));
                command.Parameters.Add(new SqlParameter("@isActive", user.isActive));
                command.Parameters.Add(new SqlParameter("@isActiveBy", user.isActiveBy));

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected > 0;
            }
        }


        // DAL/Repositories/UserRepository.cs
        public bool ChangeUserStatus(int adminId, int targetUserId, bool isActive)
        {
            var connection = _db.Database.GetDbConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "sp_ChangeUserStatus"; // Stored Procedure name
                command.CommandType = CommandType.StoredProcedure;

                // Parameters
                command.Parameters.Add(new SqlParameter("@AdminId", adminId));
                command.Parameters.Add(new SqlParameter("@TargetUserId", targetUserId));
                command.Parameters.Add(new SqlParameter("@IsActive", isActive));

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                // If 0 rows affected, either target user is admin or invalid IDs
                return rowsAffected > 0;
            }
        }
    }
}