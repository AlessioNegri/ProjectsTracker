﻿using Newtonsoft.Json;

namespace ProjectsTracker.src.Database
{
    /// <summary> Class to manage "projects" SQL requests </summary>
    class ProjectsManager
    {
        #region MEMBERS

        /// <summary> Singleton instance </summary>
        private static ProjectsManager? instance = null;

        /// <summary> Thread lock </summary>
        private static readonly object padlock = new object();

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Retrieves the class instance </summary>
        public static ProjectsManager Instance
        {
            get { lock (padlock) { if (instance is null) instance = new ProjectsManager(); return instance; } }
        }

        /// <summary> Executes a SELECT query on "projects" table </summary>
        /// <param name="elements"> List of projects </param>
        /// <returns> Success of the operation </returns>
        public bool SelectProjects(out List<ROW_PROJECT> elements)
        {
            elements = new List<ROW_PROJECT>();

            string query = "SELECT * FROM projects WHERE SolutionID IS NULL;";

            string json = string.Empty;

            if (!DBMS.Instance.ExecuteReader(query, out json)) return false;

            List<ROW_PROJECT>? jsonArr = JsonConvert.DeserializeObject<List<ROW_PROJECT>>(json);

            if (jsonArr is null) return false;

            foreach (var obj in jsonArr)
            {
                ROW_PROJECT row = new ROW_PROJECT();

                row.ProjectID   = obj.ProjectID;
                row.Name        = obj.Name;
                row.SolutionID  = obj.SolutionID;

                elements.Add(row);
            }

            return true;
        }

        /// <summary> Executes a SELECT query on "projects" table </summary>
        /// <param name="elements"> List of sub-projects </param>
        /// <param name="solutionId"> Solution ID </param>
        /// <returns> Success of the operation </returns>
        public bool SelectSubProjects(out List<ROW_PROJECT> elements, int solutionId)
        {
            elements = new List<ROW_PROJECT>();

            string query = $"" +
                $"SELECT projects.*, solutions.Name AS SolutionName FROM projects " +
                $"LEFT JOIN solutions " +
                $"ON projects.SolutionID = solutions.SolutionID " +
                $"WHERE projects.SolutionID = {solutionId};";

            string json = string.Empty;

            if (!DBMS.Instance.ExecuteReader(query, out json)) return false;

            List<ROW_PROJECT>? jsonArr = JsonConvert.DeserializeObject<List<ROW_PROJECT>>(json);

            if (jsonArr is null) return false;

            foreach (var obj in jsonArr)
            {
                ROW_PROJECT row = new ROW_PROJECT();

                row.ProjectID       = obj.ProjectID;
                row.Name            = obj.Name;
                row.SolutionID      = obj.SolutionID;
                row.SolutionName    = obj.SolutionName;

                elements.Add(row);
            }

            return true;
        }

        /// <summary> Executes a SELECT query on "projects" table for a specific project </summary>
        /// <param name="project_id"> Project ID </param>
        /// <param name="element"> Project </param>
        /// <returns> Success of the operation </returns>
        public bool SelectProjectById(in int project_id, out ROW_PROJECT element)
        {
            element = new ROW_PROJECT();

            string query = $"SELECT * FROM projects WHERE ProjectID = {project_id};";

            string json = string.Empty;

            if (!DBMS.Instance.ExecuteReader(query, out json)) return false;

            List<ROW_PROJECT>? jsonArr = JsonConvert.DeserializeObject<List<ROW_PROJECT>>(json);

            if (jsonArr is null) return false;

            if (jsonArr.Count == 0) return false;

            ROW_PROJECT jsonObj = jsonArr.ElementAt(0);

            element.ProjectID   = jsonObj.ProjectID;
            element.Name        = jsonObj.Name;
            element.SolutionID  = jsonObj.SolutionID;

            return true;
        }

        /// <summary> Inserts a new project </summary>
        /// <param name="project"> Project </param>
        /// <returns> Success of the operation </returns>
        public bool InsertProject(ROW_PROJECT project)
        {
            // Insert Project

            string query = "INSERT INTO projects (Name, SolutionID) VALUES (@name, @solutionid);";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@name", $"{project.Name}");
            parameters.Add("@solutionid", project.SolutionID is null ? DBNull.Value : project.SolutionID);

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            long project_id = 0;

            if (!DBMS.Instance.LastInsertRowId(out project_id)) return false;

            // Create project table

            query =
                $"CREATE TABLE `project_{project_id}` (" +
                $"`ID` INTEGER NOT NULL, " +
                $"`CreationDate` TEXT NOT NULL DEFAULT '0000-00-00', " +
                $"`UpdateDate` TEXT NOT NULL DEFAULT '0000-00-00 00.00.00', " +
                $"`ClosureDate` TEXT NOT NULL DEFAULT '0000-00-00', " +
                $"`Version` TEXT NOT NULL DEFAULT '00000.00000', " +
                $"`PatchVersion` TEXT NOT NULL DEFAULT '00000.00000', " +
                $"`ReferenceVersion` TEXT NOT NULL DEFAULT '00000.00000', " +
                $"`Type` INTEGER NOT NULL DEFAULT 0, " +
                $"`Number` INTEGER NOT NULL DEFAULT 0, " +
                $"`Status` INTEGER NOT NULL DEFAULT 0, " +
                $"`Priority` INTEGER NOT NULL DEFAULT 1, " +
                $"`Title` TEXT NOT NULL DEFAULT '', " +
                $"`Description` TEXT NOT NULL DEFAULT '', " +
                $"`Note` TEXT NOT NULL DEFAULT '', " +
                $"PRIMARY KEY(`ID` AUTOINCREMENT) " +
                $");";

            if (!DBMS.Instance.ExecuteQuery(query)) return false;

            return true;
        }

        /// <summary> Updates an existing project </summary>
        /// <param name="project"> Project </param>
        /// <returns> Success of the operation </returns>
        public bool UpdateProject(ROW_PROJECT project)
        {
            string query = $"UPDATE projects SET Name = @name, SolutionID = @solutionid WHERE ProjectID = {project.ProjectID};";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@name", $"{project.Name}");
            parameters.Add("@solutionid", project.SolutionID is null ? DBNull.Value : project.SolutionID);

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            return true;
        }

        /// <summary> Deletes an existing project </summary>
        /// <param name="project_id"> Project ID </param>
        /// <returns> Success of the operation </returns>
        public bool DeleteProject(int project_id)
        {
            // Delete Project

            string query = $"DELETE FROM projects WHERE ProjectID = {project_id};";

            if (!DBMS.Instance.ExecuteQuery(query)) return false;

            // Drop project table

            query = $"DROP TABLE project_{project_id};";

            if (!DBMS.Instance.ExecuteQuery(query)) return false;

            return true;
        }

        /// <summary> Moves a project inside a solution </summary>
        /// <param name="project_id"> Project ID </param>
        /// <param name="solution_id"> Solution ID </param>
        /// <returns> Success of the operation </returns>
        public bool MoveProjectInSolution(int project_id, int solution_id)
        {
            string query = $"UPDATE projects SET SolutionID = @solution_id WHERE ProjectID = @project_id;";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@solution_id", solution_id);
            parameters.Add("@project_id", project_id);

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            return true;
        }

        /// <summary> Moves a project outside a solution </summary>
        /// <param name="project_id"> Project ID </param>
        /// <returns> Success of the operation </returns>
        public bool MoveProjectOutSolution(int project_id)
        {
            string query = $"UPDATE projects SET SolutionID = NULL WHERE ProjectID = @project_id;";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@project_id", project_id);

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            return true;
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Constructor </summary>
        private ProjectsManager() { }

        #endregion
    }
}