using Newtonsoft.Json;
using System.Xml.Linq;

namespace ProjectsTracker.src.Database
{
    internal sealed class ProjectsManager
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

            string query = $"SELECT * FROM projects WHERE SolutionID = {solutionId};";

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
            string query = "INSERT INTO projects (Name, SolutionID) VALUES (@name, @solutionid);";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@name", $"{project.Name}");
            parameters.Add("@solutionid", project.SolutionID is null ? DBNull.Value : project.SolutionID);

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

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
            string query = $"DELETE FROM projects WHERE ProjectID = {project_id};";

            if (!DBMS.Instance.ExecuteQuery(query)) return false;

            return true;
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Constructor </summary>
        public ProjectsManager() { }

        #endregion
    }
}
