using Newtonsoft.Json;

namespace ProjectsTracker.src
{
    /// <summary> Database "projects" table structure </summary>
    internal struct ROW_PROJECT
    {
        /// <summary> Column 1 </summary>
        [JsonProperty(nameof(ProjectID))]
        public int ProjectID { get; set; }

        /// <summary> Column 2 </summary>
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        /// <summary> Column 3 </summary>
        [JsonProperty(nameof(SolutionID))]
        public int? SolutionID { get; set; }
    }

    /// <summary> Database "solutions" table structure </summary>
    internal struct ROW_SOLUTION
    {
        /// <summary> Column 1 </summary>
        [JsonProperty(nameof(SolutionID))]
        public int SolutionID { get; set; }

        /// <summary> Column 2 </summary>
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        /// <summary> Number of sub-projects </summary>
        [JsonProperty(nameof(SubProjects))]
        public int SubProjects { get; set; }
    }
}
