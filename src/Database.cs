using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace ProjectsTracker.src
{
    internal class Database
    {
        SqliteConnection connection;
        SqliteCommand command;
        SqliteDataReader reader;

        public bool Init()
        {
            // Open connection

            string assembly_path = System.Reflection.Assembly.GetExecutingAssembly().Location;

            string database_name = assembly_path.Split("\\")[^1].Split(".")[0];

            string database_path = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("dll", "db");

            string connection_string = $"Data Source={database_path};Mode=ReadWriteCreate";

            var connection = new SqliteConnection(connection_string);

            connection.Open();

            // Create database

            string sql_solutions =
                $"CREATE TABLE IF NOT EXISTS solutions" +
                $"(" +
                $"`SolutionID` INTEGER NOT NULL, " +
                $"`Name` TEXT NOT NULL, " +
                $"PRIMARY KEY (`SolutionID` AUTOINCREMENT)" +
                $");";

            string sql_projects =
                $"CREATE TABLE IF NOT EXISTS projects" +
                $"(" +
                $"`ProjectID` INTEGER NOT NULL, " +
                $"`Name` TEXT NOT NULL, " +
                $"`SolutionID` INTEGER NULL, " +
                $"FOREIGN KEY (`SolutionID`) REFERENCES `solutions`(`SolutionID`) ON UPDATE RESTRICT ON DELETE RESTRICT, " +
                $"PRIMARY KEY (`ProjectID` AUTOINCREMENT)" +
                $");";

            command = new SqliteCommand(sql_solutions, connection);

            command.ExecuteNonQuery();

            command = new SqliteCommand(sql_projects, connection);

            command.ExecuteNonQuery();

            // Close connection

            connection.Close();

            return true;
        }
    }
}
