using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System.Data;

namespace ProjectsTracker.src.Database
{
    internal sealed class DBMS
    {
        #region MEMBERS

        /// <summary> Singleton instance </summary>
        private static DBMS? instance = null;

        /// <summary> Thread lock </summary>
        private static readonly object padlock = new object();

        /// <summary> Connection string </summary>
        private string connection_string = "";

        /// <summary> SQLite connection </summary>
        private SqliteConnection? connection = null;

        /// <summary> SQLite command </summary>
        private SqliteCommand? command = null;

        /// <summary> SQLite data reader </summary>
        private SqliteDataReader? reader = null;

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Retrieves the class instance </summary>
        public static DBMS Instance
        {
            get { lock (padlock) { if (instance is null) instance = new DBMS(); return instance; } }
        }

        /// <summary> Initializes the SQLite database </summary>
        /// <returns> Success of the operations </returns>
        public bool Init()
        {
            // Open connection

            string assembly_path = System.Reflection.Assembly.GetExecutingAssembly().Location;

            string database_name = assembly_path.Split("\\")[^1].Split(".")[0];

            string database_path = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("dll", "db");

            connection_string = $"Data Source={database_path};Mode=ReadWriteCreate";

            connection = new SqliteConnection(connection_string);

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

        /// <summary> Executes a SQL query retrieving the json content </summary>
        /// <param name="query"> SQL query </param>
        /// <param name="json"> JSON data read </param>
        /// <returns> Success of the operations </returns>
        public bool ExecuteReader(string query, out string json)
        {
            connection!.Open();

            command = new SqliteCommand(query, connection);

            reader = command.ExecuteReader();

            var dataTable = new DataTable();

            dataTable.Load(reader);

            json = string.Empty;

            json = JsonConvert.SerializeObject(dataTable);

            connection!.Close();

            return true;
        }

        /// <summary> Executes a SQL query </summary>
        /// <param name="query"> SQL query </param>
        /// <returns> Success of the operations </returns>
        public bool ExecuteQuery(string query, Dictionary<String, Object>? parameters = null)
        {
            connection!.Open();

            command = new SqliteCommand(query, connection);

            if (parameters is not null)
            {
                foreach (var item in parameters)
                {
                    command.Parameters.AddWithValue(item.Key, item.Value);
                }
            }

            int result = command.ExecuteNonQuery();

            connection!.Close();

            return true;
        }

        /// <summary> Retrieves the last inserted table id </summary>
        /// <param name="id"> ID </param>
        /// <returns> Success of the operations </returns>
        public bool LastInsertRowId(out long id)
        {
            string query = @"select last_insert_rowid()";

            connection!.Open();

            command = new SqliteCommand(query, connection);

            id = (long?)command.ExecuteScalar() ?? 0;

            connection!.Close();

            return true;
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Constructor </summary>
        private DBMS() { }

        #endregion
    }
}
