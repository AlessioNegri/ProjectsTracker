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

        private int sub_projects = 0;

        /// <summary> Unique identifier of the solution </summary>
        public int Id { get => id; set { if (value != NullSolutionId) id = value; } }

        /// <summary> Name of the solution </summary>
        public string Name { get => name; set { if (!string.IsNullOrEmpty(value)) name = value; } }

        /// <summary> Number of sub-projects associated with it </summary>
        public int SubProjects { get => sub_projects; set => sub_projects = value; }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Checks if the solution identifier is null </summary>
        /// <returns>Result of check</returns>
        public bool IsNull() => Id == NullSolutionId;

        #endregion
    }
}
