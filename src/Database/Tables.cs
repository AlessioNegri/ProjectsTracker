using Newtonsoft.Json;

namespace ProjectsTracker.src
{
    /// <summary> Database "projects" table structure </summary>
    struct ROW_PROJECT
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

        /// <summary> LEFT JOIN solutions column 1 </summary>
        [JsonProperty(nameof(SolutionName))]
        public string SolutionName { get; set; }
    }

    /// <summary> Database "solutions" table structure </summary>
    struct ROW_SOLUTION
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

    /// <summary> Database "project_{id}" table structure </summary>
    struct ROW_TABLE
    {
        /// <summary> Column 1 </summary>
        [JsonProperty(nameof(ID))]
        public int ID { get; set; }

        /// <summary> Column 2 </summary>
        [JsonProperty(nameof(CreationDate))]
        public string CreationDate { get; set; }

        /// <summary> Column 3 </summary>
        [JsonProperty(nameof(UpdateDate))]
        public string UpdateDate { get; set; }

        /// <summary> Column 4 </summary>
        [JsonProperty(nameof(ClosureDate))]
        public string ClosureDate { get; set; }

        /// <summary> Column 5 </summary>
        [JsonProperty(nameof(Version))]
        public string Version { get; set; }

        /// <summary> Column 6 </summary>
        [JsonProperty(nameof(PatchVersion))]
        public string PatchVersion { get; set; }

        /// <summary> Column 7 </summary>
        [JsonProperty(nameof(ReferenceVersion))]
        public string ReferenceVersion { get; set; }

        /// <summary> Column 8 </summary>
        [JsonProperty(nameof(Type))]
        public int Type { get; set; }

        /// <summary> Column 9 </summary>
        [JsonProperty(nameof(Number))]
        public int Number { get; set; }

        /// <summary> Column 10 </summary>
        [JsonProperty(nameof(Status))]
        public int Status { get; set; }

        /// <summary> Column 11 </summary>
        [JsonProperty(nameof(Priority))]
        public int Priority { get; set; }

        /// <summary> Column 12 </summary>
        [JsonProperty(nameof(Title))]
        public string Title { get; set; }

        /// <summary> Column 13 </summary>
        [JsonProperty(nameof(Description))]
        public string Description { get; set; }

        /// <summary> Column 14 </summary>
        [JsonProperty(nameof(Note))]
        public string Note { get; set; }
    }
}