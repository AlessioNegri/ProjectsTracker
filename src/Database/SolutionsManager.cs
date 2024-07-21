using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsTracker.src.Database
{
    internal sealed class SolutionsManager
    {
        #region MEMBERS

        /// <summary> Singleton instance </summary>
        private static SolutionsManager? instance = null;

        /// <summary> Thread lock </summary>
        private static readonly object padlock = new object();

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Retrieves the class instance </summary>
        public static SolutionsManager Instance
        {
            get { lock (padlock) { if (instance is null) instance = new SolutionsManager(); return instance; } }
        }

        /// <summary> Executes a SELECT query on "solutions" table </summary>
        /// <param name="elements"> List of solutions </param>
        /// <returns> Success of the operation </returns>
        public bool SelectSolutions(out List<ROW_SOLUTION> elements)
        {
            elements = new List<ROW_SOLUTION>();

            string query = "SELECT * FROM solutions;";

            string json = string.Empty;

            if (!DBMS.Instance.ExecuteReader(query, out json)) return false;

            List<ROW_SOLUTION>? jsonArr = JsonConvert.DeserializeObject<List<ROW_SOLUTION>>(json);

            if (jsonArr is null) return false;

            foreach (var obj in jsonArr)
            {
                // Solution

                ROW_SOLUTION row = new ROW_SOLUTION();

                row.Name        = obj.Name;
                row.SolutionID  = obj.SolutionID;

                // Number of sub-projects

                query = $"SELECT * FROM projects WHERE SolutionID = {obj.SolutionID}";

                json = string.Empty;

                if (!DBMS.Instance.ExecuteReader(query, out json)) return false;

                List<ROW_PROJECT>? jsonArrProjects = JsonConvert.DeserializeObject<List<ROW_PROJECT>>(json);

                if (jsonArrProjects is null) return false;

                row.SubProjects = jsonArrProjects.Count();

                // Add

                elements.Add(row);
            }

            return true;
        }

        /// <summary> Executes a SELECT query on "solutions" table for a specific solution </summary>
        /// <param name="solution_id"> Solution ID </param>
        /// <param name="element"> Solution </param>
        /// <returns> Success of the operation </returns>
        public bool SelectSolutionById(in int solution_id, out ROW_SOLUTION element)
        {
            element = new ROW_SOLUTION();

            string query = $"SELECT * FROM solutions WHERE SolutionID = {solution_id};";

            string json = string.Empty;

            if (!DBMS.Instance.ExecuteReader(query, out json)) return false;

            List<ROW_SOLUTION>? jsonArr = JsonConvert.DeserializeObject<List<ROW_SOLUTION>>(json);

            if (jsonArr is null) return false;

            if (jsonArr.Count == 0) return false;

            ROW_SOLUTION jsonObj = jsonArr.ElementAt(0);

            element.Name        = jsonObj.Name;
            element.SolutionID  = jsonObj.SolutionID;

            return true;
        }

        /// <summary> Inserts a new solution </summary>
        /// <param name="solution"> Solution </param>
        /// <returns> Success of the operation </returns>
        public bool InsertSolution(ROW_SOLUTION solution)
        {
            string query = "INSERT INTO solutions (Name) VALUES (@name);";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@name", $"{solution.Name}");

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            return true;
        }

        /// <summary> Updates an existing solution </summary>
        /// <param name="solution"> Solution </param>
        /// <param name="extract"> True for extracting all sub-projects </param>
        /// <returns> Success of the operation </returns>
        public bool UpdateSolution(ROW_SOLUTION solution, bool extract)
        {
            string query = $"UPDATE solutions SET Name = @name WHERE SolutionID = {solution.SolutionID};";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@name", $"{solution.Name}");

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            if (extract)
            {
                query = $"UPDATE projects SET SolutionID = NULL WHERE SolutionID = {solution.SolutionID};";


                if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;
            }

            return true;
        }

        /// <summary> Deletes an existing solution </summary>
        /// <param name="solution_id"> Solution ID </param>
        /// <returns> Success of the operation </returns>
        public bool DeleteSolution(int solution_id)
        {
            string query = $"DELETE FROM projects WHERE SolutionID = {solution_id};";

            if (!DBMS.Instance.ExecuteQuery(query)) return false;

            query = $"DELETE FROM solutions WHERE SolutionID = {solution_id};";

            if (!DBMS.Instance.ExecuteQuery(query)) return false;

            return true;
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Constructor </summary>
        public SolutionsManager() { }

        #endregion
    }
}
