using MaterialDesignThemes.Wpf;
using ProjectsTracker.src.Database;
using ProjectsTracker.src.Models;
using ProjectsTracker.src.MVVM;
using ProjectsTracker.src.Services;
using System.Collections.ObjectModel;
using UC = ProjectsTracker.ui.UserControls;

namespace ProjectsTracker.src.ViewModels
{
    /// <summary> PageProject View Model </summary>
    internal class PageProjectViewModel : ViewModelBase
    {
        #region MEMBERS

        private INavigationService navigation;

        private int project_id = UC.CardProject.NullProjectId;

        private int solution_id = UC.CardProject.NullSolutionId;

        private string solution_name = string.Empty;

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

            LoadProjectRows();
        }

        /// <summary> Loads the project rows </summary>
        public void LoadProjectRows()
        {
            ProjectRows.Clear();

            // Query DB

            var rows = new List<ROW_TABLE>();

            if (!TablesManager.Instance.SelectRows(project_id, out rows)) return;

            foreach (var row in rows)
            {
                ProjectRow project_row = new ProjectRow();

                project_row.RowId               = row.ID;
                project_row.CreationDate        = (row.CreationDate == "0000-00-00") ? string.Empty : row.CreationDate;
                project_row.UpdateDate          = row.UpdateDate;
                project_row.ClosureDate         = (row.ClosureDate == "0000-00-00") ? string.Empty : row.ClosureDate;
                project_row.Version             = row.Version;
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