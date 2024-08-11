using MaterialDesignThemes.Wpf;
using ProjectsTracker.src.Database;
using ProjectsTracker.src.Models;
using ProjectsTracker.src.MVVM;
using ProjectsTracker.src.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UC = ProjectsTracker.ui.UserControls;

namespace ProjectsTracker.src.ViewModels
{
    /// <summary> PageProject View Model </summary>
    internal class PageProjectViewModel : ViewModelBase
    {
        #region READONLY

        public readonly KeyValuePair<int, string> TypeAll       = new KeyValuePair<int, string>(-1, "All");
        public readonly KeyValuePair<int, string> TypeECR       = new KeyValuePair<int, string>(0, "ECR");
        public readonly KeyValuePair<int, string> TypePR        = new KeyValuePair<int, string>(1, "PR");
        public readonly KeyValuePair<int, string> TypeRELEASE   = new KeyValuePair<int, string>(2, "RELEASE");
        public readonly KeyValuePair<int, string> TypePATCH     = new KeyValuePair<int, string>(3, "PATCH");

        public readonly KeyValuePair<int, string> StatusAll         = new KeyValuePair<int, string>(-1, "All");
        public readonly KeyValuePair<int, string> StatusAssigned    = new KeyValuePair<int, string>(0, "Assigned");
        public readonly KeyValuePair<int, string> StatusInProgress  = new KeyValuePair<int, string>(1, "In Progress");
        public readonly KeyValuePair<int, string> StatusDone        = new KeyValuePair<int, string>(2, "Done");

        public readonly KeyValuePair<int, string> PriorityAll   = new KeyValuePair<int, string>(-1, "All");
        public readonly KeyValuePair<int, string> Priority1     = new KeyValuePair<int, string>(1, "1 (Low)");
        public readonly KeyValuePair<int, string> Priority2     = new KeyValuePair<int, string>(2, "2");
        public readonly KeyValuePair<int, string> Priority3     = new KeyValuePair<int, string>(3, "3");
        public readonly KeyValuePair<int, string> Priority4     = new KeyValuePair<int, string>(4, "4");
        public readonly KeyValuePair<int, string> Priority5     = new KeyValuePair<int, string>(5, "5 (High)");

        #endregion

        #region MEMBERS

        private INavigationService navigation;

        private int project_id = UC.CardProject.NullProjectId;

        private int solution_id = UC.CardProject.NullSolutionId;

        private string solution_name = string.Empty;

        private KeyValuePair<string, string> version = new KeyValuePair<string, string>();

        private KeyValuePair<int, string> type = new KeyValuePair<int, string>();

        private KeyValuePair<int, string> status = new KeyValuePair<int, string>();

        private KeyValuePair<int, string> priority = new KeyValuePair<int, string>();

        #endregion

        #region BINDINGS

        /// <summary> Navigation command </summary>
        public INavigationService Navigation { get => navigation; set { navigation = value; OnPropertyChanged(); } }

        /// <summary> Project id </summary>
        public int ProjectId { get => project_id; set => project_id = value; }

        /// <summary> Solution id </summary>
        public int SolutionId { get => solution_id; set => solution_id = value; }

        /// <summary> Solution name </summary>
        public string SolutionName { get => solution_name; set => solution_name = value; }

        /// <summary> Collection of ProjectRow </summary>
        public ObservableCollection<ProjectRow> ProjectRows { get; private set; }

        /// <summary> Collection for version combo box </summary>
        public ObservableCollection<KeyValuePair<string, string>> VersionItems { get; set; }

        /// <summary> Collection for type combo box </summary>
        public ObservableCollection<KeyValuePair<int, string>> TypeItems { get; set; }

        /// <summary> Collection for status combo box </summary>
        public ObservableCollection<KeyValuePair<int, string>> StatusItems { get; set; }

        /// <summary> Collection for priority combo box </summary>
        public ObservableCollection<KeyValuePair<int, string>> PriorityItems { get; set; }

        /// <summary> Version </summary>
        public KeyValuePair<string, string> Version { get => version; set { version = value; OnPropertyChanged(); LoadProjectRows(); } }

        /// <summary> Status </summary>
        public KeyValuePair<int, string> Type { get => type; set { type = value; OnPropertyChanged(); LoadProjectRows(); } }

        /// <summary> Status </summary>
        public KeyValuePair<int, string> Status { get => status; set { status = value; OnPropertyChanged(); LoadProjectRows(); } }

        /// <summary> Importance </summary>
        public KeyValuePair<int, string> Priority { get => priority; set { priority = value; OnPropertyChanged(); LoadProjectRows(); } }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        /// <param name="service"> Navigation service </param>
        public PageProjectViewModel(INavigationService service)
        {
            Navigation = service;

            project_id = Int32.Parse(((NavigationService)service).Parameters!["ProjectId"]);

            solution_id = Int32.Parse(((NavigationService)service).Parameters!["SolutionId"]);

            solution_name = ((NavigationService)service).Parameters!["SolutionName"];

            ProjectRows = new ObservableCollection<ProjectRow>();

            VersionItems = new ObservableCollection<KeyValuePair<string, string>>();

            TypeItems = new ObservableCollection<KeyValuePair<int, string>>();

            StatusItems = new ObservableCollection<KeyValuePair<int, string>>();

            PriorityItems = new ObservableCollection<KeyValuePair<int, string>>();

            LoadComboBoxes();
        }

        /// <summary> Loads the combo box items </summary>
        void LoadComboBoxes()
        {
            // Init Version Combo Box

            var versions = new List<string>();

            if (!TablesManager.Instance.SelectVersions(ProjectId, out versions)) return;

            VersionItems.Add(new KeyValuePair<string, string>("All", "All"));

            foreach (var version in versions)
            {
                if (String.IsNullOrEmpty(version))
                {
                    VersionItems.Add(new KeyValuePair<string, string>("", "No Version"));
                }
                else
                {
                    VersionItems.Add(new KeyValuePair<string, string>(version, version));
                }
            }

            Version = VersionItems.Last();

            // Init Type Combo Box

            TypeItems.Clear();

            TypeItems.Add(TypeAll);
            TypeItems.Add(TypeECR);
            TypeItems.Add(TypePR);
            TypeItems.Add(TypeRELEASE);
            TypeItems.Add(TypePATCH);

            TypeItems.ElementAt(0);

            Type = TypeItems.ElementAt(0);

            // Init Status Combo Box

            StatusItems.Clear();

            StatusItems.Add(StatusAll);
            StatusItems.Add(StatusAssigned);
            StatusItems.Add(StatusInProgress);
            StatusItems.Add(StatusDone);

            StatusItems.ElementAt(0);

            Status = TypeItems.ElementAt(0);

            // Init Priority Combo Box

            PriorityItems.Clear();

            PriorityItems.Add(PriorityAll);
            PriorityItems.Add(Priority1);
            PriorityItems.Add(Priority2);
            PriorityItems.Add(Priority3);
            PriorityItems.Add(Priority4);
            PriorityItems.Add(Priority5);

            PriorityItems.ElementAt(0);

            Priority = PriorityItems.ElementAt(0);

            // Load project rows

            LoadProjectRows();
        }

        /// <summary> Loads the project rows </summary>
        public void LoadProjectRows()
        {
            ProjectRows.Clear();

            // Query DB

            var rows = new List<ROW_TABLE>();

            if (!TablesManager.Instance.SelectRows(project_id, Version.Key, Type.Key, Status.Key, Priority.Key, out rows)) return;

            rows.Sort((a, b) => string.Compare(a.Ordering, b.Ordering) );

            foreach (var row in rows)
            {
                ProjectRow project_row = new ProjectRow();

                project_row.RowId               = row.ID;
                project_row.CreationDate        = (row.CreationDate == "0000-00-00") ? string.Empty : row.CreationDate;
                project_row.UpdateDate          = row.UpdateDate;
                project_row.ClosureDate         = (row.ClosureDate == "0000-00-00") ? string.Empty : row.ClosureDate;
                project_row.Version             = (row.Type == ((int)RowType.PR)) ? (row.Version + " - [" + row.ReferenceVersion + "]") : row.Version;
                project_row.PatchVersion        = (row.Type == ((int)RowType.RELEASE)) ? string.Empty : row.PatchVersion;
                project_row.ReferenceVersion    = row.ReferenceVersion;
                project_row.Type                = row.Type;
                project_row.Status              = row.Status;
                project_row.Priority            = row.Priority;
                project_row.Title               = row.Title;
                project_row.Description         = row.Description;
                project_row.Note                = row.Note;

                // Id Label

                switch (row.Type)
                {
                    case 0: project_row.Id = "ECR - " + row.Number.ToString(); break;
                    case 1: project_row.Id = "PR - " + row.Number.ToString(); break;
                    case 2: project_row.Id = "RELEASE"; break;
                    case 3: project_row.Id = "PATCH"; break;
                }

                // Status Icons

                switch (row.Status)
                {
                    case 0: project_row.StatusIcon = PackIconKind.AlphaACircle; break;
                    case 1: project_row.StatusIcon = PackIconKind.TimerSandFull; break;
                    case 2: project_row.StatusIcon = PackIconKind.CheckCircle; break;
                }

                // Priority Icons

                if (row.Type == ((int)RowType.RELEASE) || row.Type == ((int)RowType.PATCH))
                {
                    project_row.PriorityIcon1 = PackIconKind.None;
                    project_row.PriorityIcon2 = PackIconKind.None;
                    project_row.PriorityIcon3 = PackIconKind.None;
                    project_row.PriorityIcon4 = PackIconKind.None;
                    project_row.PriorityIcon5 = PackIconKind.None;
                }
                else
                {
                    project_row.PriorityIcon1 = (row.Priority >= 1) ? PackIconKind.Star : PackIconKind.StarBorder;
                    project_row.PriorityIcon2 = (row.Priority >= 2) ? PackIconKind.Star : PackIconKind.StarBorder;
                    project_row.PriorityIcon3 = (row.Priority >= 3) ? PackIconKind.Star : PackIconKind.StarBorder;
                    project_row.PriorityIcon4 = (row.Priority >= 4) ? PackIconKind.Star : PackIconKind.StarBorder;
                    project_row.PriorityIcon5 = (row.Priority >= 5) ? PackIconKind.Star : PackIconKind.StarBorder;
                }

                ProjectRows.Add(project_row);
            }
        }

        #endregion
    }
}