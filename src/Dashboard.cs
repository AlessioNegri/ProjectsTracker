using ProjectsTracker.src.Database;
using ProjectsTracker.src.Models;
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
        //private Database database;

        /// <summary> Vector containing the list of projects </summary>
        private List<Project> projects;

        /// <summary> Vector containing the list of solutions </summary>
        private List<Solution> solutions;

        public List<Project> Projects { get { return projects; } init { } }

        public List<Solution> Solutions { get { return solutions; } init { } }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        public Dashboard()
        {
            //database    = new Database();
            projects    = new List<Project>();
            solutions   = new List<Solution>();

            Init();
        }

        public bool UpdateProject(int id, string name, int? solutionId = null)
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
            //database.Init();

            string projects = string.Empty;

            LoadProjects();
            LoadSolutions();
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Loads the list of projects </summary>
        private void LoadProjects()
        {
            var t_projects = new List<ROW_PROJECT>();

            ProjectsManager.Instance.SelectProjects(out t_projects);

            foreach (var row in t_projects)
            {
                var project = new Project();

                project.Id          = row.ProjectID;
                project.Name        = row.Name;
                project.SolutionId  = row.SolutionID;

                projects.Add(project);
            }
        }

        /// <summary> Loads the list of solutions </summary>
        private void LoadSolutions()
        {
            var t_solutions = new List<ROW_SOLUTION>();

            //database.SelectSolutions(out t_solutions);

            foreach (var row in t_solutions)
            {
                var solution = new Solution();

                solution.Id     = row.SolutionID;
                solution.Name   = row.Name;

                var t_projects = new List<ROW_PROJECT>();

                //database.SelectProjects(row.SolutionID, out t_projects);

                foreach (var item in t_projects)
                {
                    var project = new Project();

                    project.Id          = item.ProjectID;
                    project.Name        = item.Name;
                    project.SolutionId  = item.SolutionID;

                    solution.AddSubProject(project);
                }

                solutions.Add(solution);
            }
        }

        #endregion
    }
}
