using System;
using MySql.Data.MySqlClient;

namespace Adv.Server.Util.Database
{
    class DatabaseConnection : IDatabaseConnection
    {
        private string connectionString;

        private readonly MySqlConnection connection;

        public DatabaseConnection(string connectionString)
        {
            this.connectionString = connectionString;
            connection = new MySqlConnection(connectionString);
            connection.Open();

            Init();
        }

        public void Init()
        {
            ExecuteNonQuery(@"CREATE DATABASE IF NOT EXISTS `Adventure`;");

            ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS Adventure.Characters (
                                id INT AUTO_INCREMENT PRIMARY KEY,
                                name VARCHAR(255) NOT NULL,
                                location INT NOT NULL,
                                avatar TINYINT NOT NULL,
                                colorA INT NOT NULL,
                                colorB INT NOT NULL,
                                colorC INT NOT NULL,
                                colorD INT NOT NULL,
                                flags INT NOT NULL,
                                isAdmin BOOLEAN NOT NULL,
                                user INT NOT NULL
                                );");

            ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS Adventure.Teams (
                                id INT AUTO_INCREMENT PRIMARY KEY,
                                name VARCHAR(255) NOT NULL,
                                secretName VARCHAR(255) NOT NULL
                                );");

            ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS Adventure.Users (
                                id INT AUTO_INCREMENT PRIMARY KEY,
                                username VARCHAR(255) NOT NULL,
                                password VARCHAR(255) NOT NULL,
                                isAdmin BOOLEAN NOT NULL,
                                team INT,
                                FOREIGN KEY (team) REFERENCES Adventure.Teams(id)
                                );");

            ExecuteNonQuery(@"ALTER TABLE Adventure.Characters ADD FOREIGN KEY (user) REFERENCES Adventure.Users(id);");
        }

        public void ExecuteNonQuery(string commandText, params Tuple<string, object>[] parameters)
        {
            var command = new MySqlCommand(commandText, connection);
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
            }
            command.ExecuteNonQuery();
        }

        public MySqlDataReader ExecuteQuery(string commandText, params Tuple<string, object>[] parameters)
        {
            var command = new MySqlCommand(commandText, connection);
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
            }
            return command.ExecuteReader();
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}