using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Adv.Server.Util.Database
{
    class DatabaseConnection : IDisposable
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
            using var createDBCommand = connection.CreateCommand();
            createDBCommand.CommandText = "CREATE DATABASE IF NOT EXISTS `Adventure`;";
            createDBCommand.ExecuteNonQuery();

            using var createCharactersTableCommand = connection.CreateCommand();
            createCharactersTableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS Adventure.Characters (
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
                                );";
            createCharactersTableCommand.ExecuteNonQuery();
            
            using var createTeamTableCommand = connection.CreateCommand();
            createTeamTableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS Adventure.Teams (
                                id INT AUTO_INCREMENT PRIMARY KEY,
                                name VARCHAR(255) NOT NULL,
                                secretName VARCHAR(255) NOT NULL
                                );";
            createTeamTableCommand.ExecuteNonQuery();
            
            using var createUserTableCommand = connection.CreateCommand();
            createUserTableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS Adventure.Users (
                                id INT AUTO_INCREMENT PRIMARY KEY,
                                username VARCHAR(255) NOT NULL,
                                password VARCHAR(255) NOT NULL,
                                isAdmin BOOLEAN NOT NULL,
                                team INT,
                                FOREIGN KEY (team) REFERENCES Adventure.Teams(id)
                                );";
            createUserTableCommand.ExecuteNonQuery();

            using var addUserForeignKeyToCharacterCommand = connection.CreateCommand();
            addUserForeignKeyToCharacterCommand.CommandText = @"ALTER TABLE Adventure.Characters ADD FOREIGN KEY (user) REFERENCES Adventure.Users(id);";
            addUserForeignKeyToCharacterCommand.ExecuteNonQuery();
        }

        public void ExecuteQuery(string commandText)
        {
            var command = new MySqlCommand(commandText, connection);
            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}
