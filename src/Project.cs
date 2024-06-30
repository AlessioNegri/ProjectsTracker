using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsTracker.src
{
    internal class Project
    {
        #region CONST

        /// <summary> Null project unique identifier </summary>
        public const int NullProjectId = 0;

        /// <summary> Null solution unique identifier </summary>
        public const int NullSolutionId = 0;

        #endregion

        #region MEMBERS

        private int id = NullProjectId;

        private string name = "";

        private int solutionId = NullSolutionId;

        /// <summary> Unique identifier of the project </summary>
        public int Id { get => id; set { if (value != NullProjectId) id = value; } }

        /// <summary> Name of the project </summary>
        public string Name { get => name; set { if (String.IsNullOrEmpty(value)) name = value; } }

        /// <summary> Unique identifier of the connected solution (it is a sub-project) </summary>
        public int SolutionId { get => solutionId; set { if (value != NullSolutionId) solutionId = value; } }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Checks if the project identifier is null </summary>
        /// <returns>Result of check</returns>
        public bool IsNull() { return Id == NullProjectId; }

        /// <summary> Checks if the project is a sub-project </summary>
        /// <returns>Result of check</returns>
        public bool IsSubProject() { return SolutionId == NullSolutionId; }

        #endregion
    }
}
