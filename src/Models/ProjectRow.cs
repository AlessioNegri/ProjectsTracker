using MaterialDesignThemes.Wpf;

namespace ProjectsTracker.src.Models
{
    /// <summary> Project table row model </summary>
    public class ProjectRow
    {
        #region BINDINGS

        /// <summary> ID of the table row </summary>
        public int RowId { get; set; }

        /// <summary> Creation date </summary>
        public string CreationDate { get; set; }

        /// <summary> Update date/time </summary>
        public string UpdateDate { get; set; }

        /// <summary> Closure date </summary>
        public string ClosureDate { get; set; }

        /// <summary> Version </summary>
        public string Version { get; set; }

        /// <summary> Patch version </summary>
        public string PatchVersion { get; set; }

        /// <summary> Reference version </summary>
        public string ReferenceVersion { get; set; }

        /// <summary> Row type </summary>
        public int Type { get; set; }

        /// <summary> Type number </summary>
        public int Number { get; set; }

        /// <summary> Type id </summary>
        public string Id { get; set; }

        /// <summary> Status </summary>
        public int Status { get; set; }

        /// <summary> Status icon </summary>
        public PackIconKind StatusIcon { get; set; }

        /// <summary> Priority </summary>
        public int Priority { get; set; }

        /// <summary> Priority icon (icon 1) </summary>
        public PackIconKind PriorityIcon1 { get; set; }

        /// <summary> Priority icon (icon 2) </summary>
        public PackIconKind PriorityIcon2 { get; set; }

        /// <summary> Priority icon (icon 3) </summary>
        public PackIconKind PriorityIcon3 { get; set; }

        /// <summary> Priority icon (icon 4) </summary>
        public PackIconKind PriorityIcon4 { get; set; }

        /// <summary> Priority icon (icon 5) </summary>
        public PackIconKind PriorityIcon5 { get; set; }

        /// <summary> Title </summary>
        public string Title { get; set; }

        /// <summary> Description </summary>
        public string Description { get; set; }

        /// <summary> Note </summary>
        public string Note { get; set; }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        public ProjectRow()
        {
            RowId               = 0;
            CreationDate        = string.Empty;
            UpdateDate          = string.Empty;
            ClosureDate         = string.Empty;
            Version             = string.Empty;
            PatchVersion        = string.Empty;
            ReferenceVersion    = string.Empty;
            Type                = 0;
            Number              = 0;
            Id                  = string.Empty;
            Status              = 0;
            StatusIcon          = PackIconKind.Assignment;
            Priority            = 0;
            PriorityIcon1       = PackIconKind.StarBorder;
            PriorityIcon2       = PackIconKind.StarBorder;
            PriorityIcon3       = PackIconKind.StarBorder;
            PriorityIcon4       = PackIconKind.StarBorder;
            PriorityIcon5       = PackIconKind.StarBorder;
            Title               = string.Empty;
            Description         = string.Empty;
            Note                = string.Empty;
        }

        #endregion
    }
}