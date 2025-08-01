﻿// Repositories/UserRepository.cs
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using TekstilScada.Core;
using TekstilScada.Models;
using TekstilScada.Core; // Bu satırı ekleyin
namespace TekstilScada.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString = AppConfig.ConnectionString;

        public User GetUserByUsername(string username)
        {
            User user = null;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Username, FullName, IsActive FROM users WHERE Username = @Username;";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = reader.GetInt32("Id"),
                            Username = reader.GetString("Username"),
                            FullName = reader.GetString("FullName"),
                            IsActive = reader.GetBoolean("IsActive")
                        };
                    }
                }

                if (user != null)
                {
                    user.Roles = GetUserRoles(user.Id);
                }
            }
            return user;
        }

        public List<Role> GetUserRoles(int userId)
        {
            var roles = new List<Role>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT r.Id, r.RoleName FROM roles r INNER JOIN user_roles ur ON r.Id = ur.RoleId WHERE ur.UserId = @UserId;";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role { Id = reader.GetInt32("Id"), RoleName = reader.GetString("RoleName") });
                    }
                }
            }
            return roles;
        }

        public bool ValidateUser(string username, string password)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT PasswordHash FROM users WHERE Username = @Username AND IsActive = TRUE;";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                var storedHash = cmd.ExecuteScalar() as string;
                if (storedHash == null) return false;
                return PasswordHasher.VerifyPassword(password, storedHash);
            }
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Username, FullName, IsActive FROM users ORDER BY Username;";
                var cmd = new MySqlCommand(query, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32("Id"),
                            Username = reader.GetString("Username"),
                            FullName = reader.GetString("FullName"),
                            IsActive = reader.GetBoolean("IsActive")
                        });
                    }
                }
            }
            return users;
        }

        public List<Role> GetAllRoles()
        {
            var roles = new List<Role>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, RoleName FROM roles ORDER BY RoleName;";
                var cmd = new MySqlCommand(query, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role { Id = reader.GetInt32("Id"), RoleName = reader.GetString("RoleName") });
                    }
                }
            }
            return roles;
        }

        public void AddUser(User user, string password, List<int> roleIds)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "INSERT INTO users (Username, FullName, PasswordHash, IsActive) VALUES (@Username, @FullName, @PasswordHash, @IsActive); SELECT LAST_INSERT_ID();";
                        var cmd = new MySqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@FullName", user.FullName);
                        cmd.Parameters.AddWithValue("@PasswordHash", PasswordHasher.HashPassword(password));
                        cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                        user.Id = Convert.ToInt32(cmd.ExecuteScalar());

                        UpdateUserRoles(user.Id, roleIds, connection, transaction);

                        transaction.Commit();
                    }
                    catch (Exception) { transaction.Rollback(); throw; }
                }
            }
        }

        public void UpdateUser(User user, List<int> roleIds, string newPassword = null)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "UPDATE users SET Username = @Username, FullName = @FullName, IsActive = @IsActive " +
                                       (!string.IsNullOrEmpty(newPassword) ? ", PasswordHash = @PasswordHash " : "") +
                                       "WHERE Id = @Id;";

                        var cmd = new MySqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@Id", user.Id);
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@FullName", user.FullName);
                        cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                        if (!string.IsNullOrEmpty(newPassword))
                        {
                            cmd.Parameters.AddWithValue("@PasswordHash", PasswordHasher.HashPassword(newPassword));
                        }
                        cmd.ExecuteNonQuery();

                        UpdateUserRoles(user.Id, roleIds, connection, transaction);

                        transaction.Commit();
                    }
                    catch (Exception) { transaction.Rollback(); throw; }
                }
            }
        }

        public void UpdateUserRoles(int userId, List<int> roleIds, MySqlConnection connection, MySqlTransaction transaction)
        {
            // Önce mevcut rolleri sil
            var deleteCmd = new MySqlCommand("DELETE FROM user_roles WHERE UserId = @UserId;", connection, transaction);
            deleteCmd.Parameters.AddWithValue("@UserId", userId);
            deleteCmd.ExecuteNonQuery();

            // Sonra yeni rolleri ekle
            if (roleIds != null && roleIds.Any())
            {
                foreach (var roleId in roleIds)
                {
                    var insertCmd = new MySqlCommand("INSERT INTO user_roles (UserId, RoleId) VALUES (@UserId, @RoleId);", connection, transaction);
                    insertCmd.Parameters.AddWithValue("@UserId", userId);
                    insertCmd.Parameters.AddWithValue("@RoleId", roleId);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int userId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM users WHERE Id = @Id;";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", userId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
