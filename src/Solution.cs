using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsTracker.src
{
    internal class Solution
    {
        #region CONST

        /// <summary> Null project unique identifier </summary>
        const int NullProjectId = 0;

        /// <summary> Null solution unique identifier </summary>
        const int NullSolutionId = 0;

        #endregion

        #region MEMBERS

        private int id = NullSolutionId;

        private string name = "";

        /// <summary> Vector containing the list of sub-projects associated with it </summary>
        private Vector<Project> subProjects;

        /// <summary> Unique identifier of the solution </summary>
        public int Id { get => id; set { if (value != NullSolutionId) id = value; } }

        /// <summary> Name of the solution </summary>
        public string Name { get => name; set { if (String.IsNullOrEmpty(value)) name = value; } }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Checks if the solution identifier is null </summary>
        /// <returns>Result of check</returns>
        public bool IsNull() { return Id == NullSolutionId; }

        #endregion
    }
}
