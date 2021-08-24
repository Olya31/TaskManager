using BL.Managers.Interfaces;
using Entities.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Managers
{
    public sealed class TaskManager : ITaskManager
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _context;
        private string connectionDb;

        public TaskManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _context = _configuration.GetSection("ConnectionStrings");
            connectionDb = _context.GetSection("sqlConnection").Value;
        }

        public async Task<int> AddTaskAsync(TaskModel task, CancellationToken token)
        {
            var sqlExpression = String.Format(
                "INSERT INTO Tasks " +
                "(Name, Description, Url, CronFormat, Email, Header)" +
                " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", 
                task.Name, 
                task.Description, 
                task.Url,
                task.CronFormat,
                task.Email,
                task.Header);

            return await GetConnectionAsync(sqlExpression, token);
        }

        public async Task<int> DeleteTaskAsync(int id, CancellationToken token)
        {
            var sqlExpression = String.Format("Delete FROM Tasks WHERE Id='{0}'", id);

            return await GetConnectionAsync(sqlExpression, token);
        }

        public async Task<int> UpdateAsync(TaskModel task, CancellationToken token)
        {
            var sqlExpression = String.Format(
                "UPDATE Tasks SET Name='{0}'," +
                " Description ='{1}'," +
                " Url='{2}'," +
                " CronFormat='{3}'," +
                " Email='{4}'," +
                " Header='{5}'" +
                " WHERE Id='{6}'", 
                task.Name, 
                task.Description, 
                task.Url,
                task.CronFormat,
                task.Email,
                task.Header,
                task.Id);

            return await GetConnectionAsync(sqlExpression, token);
        }

        public async Task<TaskModel> GetTaskByIdAsync(int id, CancellationToken token)
        {
            var sqlExpression = String.Format("SELECT * FROM Tasks WHERE Id='{0}'", id);
            var task = new TaskModel(0, "", "", "", "", "", "");
            using (var connection = new SqliteConnection(connectionDb))
            {
                await connection.OpenAsync(token);
                var command = new SqliteCommand(sqlExpression, connection);
                try
                {
                    using (SqliteDataReader reader = await command.ExecuteReaderAsync(token))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    var Id = reader.GetInt32(0);
                                    var Name = reader.GetString(1);
                                    var Description = reader.GetString(2);
                                    var Url = reader.GetString(3);
                                    var CronFormat = reader.GetString(4);
                                    var Email = reader.GetString(5);
                                    var Header = reader.GetString(6);
                                    var taskItem = new TaskModel(Id, Name, Description, Url, CronFormat, Email, Header);
                                    task = taskItem;
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }

            }

            return task;
        }

        public async Task<List<TaskModel>> GetAllTaskAsync(CancellationToken token)
        {
            string sqlExpression = "SELECT * FROM Tasks";
            List<TaskModel> tasks = new List<TaskModel>();

            using (var connection = new SqliteConnection(connectionDb))
            {
                await connection.OpenAsync(token);
                var command = new SqliteCommand(sqlExpression, connection);

                using (SqliteDataReader reader = await command.ExecuteReaderAsync(token))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var Id = reader.GetInt32(0);
                                var Name = reader.GetString(1);
                                var Description = reader.GetString(2);
                                var Url = reader.GetString(3);
                                var CronFormat = reader.GetString(4);
                                var Email = reader.GetString(5);
                                var Header = reader.GetString(6);
                                var task = new TaskModel(Id, Name, Description, Url, CronFormat, Email, Header);
                                tasks.Add(task);
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                    }
                }
            }

            return tasks;
        }

        private async Task<int> GetConnectionAsync(string sqlExpression, CancellationToken token)
        {
            try
            {
                using (var connection = new SqliteConnection(connectionDb))
                {
                    await connection.OpenAsync(token);
                    var command = new SqliteCommand(sqlExpression, connection);
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }
    }
}
