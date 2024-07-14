namespace ProjectsTracker.src.Models
{
    internal class Solution
    {
        #region CONST

        /// <summary> Null project unique identifier </summary>
        public const int NullProjectId = 0;

        /// <summary> Null solution unique identifier </summary>
        public const int NullSolutionId = 0;

        #endregion

        #region MEMBERS

        private int id = NullSolutionId;

        private string name = "";

        /// <summary> List containing the list of sub-projects associated with it </summary>
        private List<Project> sub_projects = new List<Project>();

        /// <summary> Unique identifier of the solution </summary>
        public int Id { get => id; set { if (value != NullSolutionId) id = value; } }

        /// <summary> Name of the solution </summary>
        public string Name { get => name; set { if (!string.IsNullOrEmpty(value)) name = value; } }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Checks if the solution identifier is null </summary>
        /// <returns>Result of check</returns>
        public bool IsNull() => Id == NullSolutionId;

        /// <summary> Add a project to the list of sub projects </summary>
        /// <param name="project"></param>
        public void AddSubProject(Project project) => sub_projects.Add(project);

        #endregion
    }
}
