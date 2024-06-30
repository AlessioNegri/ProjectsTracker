using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ProjectsTracker.src
{
    internal class Dashboard
    {
        #region MEMBERS

        /// <summary> Database Management System </summary>
        private Database database;

        /// <summary> Vector containing the list of projects </summary>
        private List<Project> projects;

        /// <summary> Vector containing the list of solutions </summary>
        private List<Solution> solutions;

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        public Dashboard()
        {
            database    = new Database();
            projects    = new List<Project>();
            solutions   = new List<Solution>();

            Init();
        }

        public bool InsertProject(string name, int solutionId)
        {
            Project project = new Project();

            project.Name        = name;
            project.SolutionId  = solutionId;

            projects.Add(project);

            return true;
        }

        public bool UpdateProject(int id, string name, int solutionId)
        {
            Project? project = projects.Find(e => e.Id == id);

            if (project == null) return false;

            project.Name        = name;
            project.SolutionId  = solutionId;

            return true;
        }

        public bool DeleteProject(int id)
        {
            int index = projects.FindIndex(e => e.Id == id);

            if (index == -1) return false;

            projects.RemoveAt(index);

            return true;
        }

        public bool InsertSolution(string name)
        {
            Solution solution = new Solution();

            solution.Name = name;

            solutions.Add(solution);

            return true;
        }

        public bool UpdateSolution(int id, string name)
        {
            Solution? solution = solutions.Find(e => e.Id == id);

            if (solution == null) return false;

            solution.Name = name;

            return true;
        }

        public bool DeleteSolution(int id)
        {
            int index = solutions.FindIndex(e => e.Id == id);

            if (index == -1) return false;

            solutions.RemoveAt(index);

            return true;
        }

        #endregion

        #region METHODS - PROTECTED

        /// <summary> Initializes the structures </summary>
        protected void Init()
        {
            database.Init();
        }

        #endregion
    }
}
