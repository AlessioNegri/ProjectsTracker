using ProjectsTracker.src;
using ProjectsTracker.src.Database;
using ProjectsTracker.src.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProjectsTracker.ui.Dialogs
{
    /// <summary>
    /// Logica di interazione per DialogProject.xaml
    /// </summary>
    public partial class DialogProject : Window, INotifyPropertyChanged
    {
        #region INTERFACES

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region MEMBERS

        private ObservableCollection<KeyValuePair<int, string>> solution_items = new ObservableCollection<KeyValuePair<int, string>>();

        private bool edit = false;

        private string project_header = String.Empty;

        private int project_id = 0;

        private string project_name = String.Empty;

        private bool project_choice_no_solution = true;

        private bool project_choice_new_solution = false;

        private bool project_choice_add_to_solution = false;

        private string project_solution_name = String.Empty;

        private KeyValuePair<int, string> project_solution = new KeyValuePair<int, string>();

        private string error = String.Empty;

        private Dictionary<string, string> errors = new Dictionary<string, string>();

        private bool success = false;

        #endregion

        #region BINDINGS

        public ObservableCollection<KeyValuePair<int, string>> SolutionItems { get; set; }

        public bool Edit { get => edit; set { edit = value; } }

        public string ProjectHeader { get => project_header; set { project_header = value; OnPropertyChanged(); } }

        public int ProjectId { get => project_id; set => project_id = value; }

        public string ProjectName { get => project_name; set { project_name = ctbProjectName.Text = value; Validate(); OnPropertyChanged(); } }

        public bool ProjectChoiceNoSolution { get => project_choice_no_solution; set { project_choice_no_solution = value; OnPropertyChanged(); } }

        public bool ProjectChoiceNewSolution { get => project_choice_new_solution; set { project_choice_new_solution = value; OnPropertyChanged(); } }

        public bool ProjectChoiceAddToSolution { get => project_choice_add_to_solution; set { project_choice_add_to_solution = value; OnPropertyChanged(); } }

        public string ProjectSolutionName { get => project_solution_name; set { project_solution_name = value; Validate(); OnPropertyChanged(); } }

        public KeyValuePair<int, string> ProjectSolution { get => project_solution; set { project_solution = value; OnPropertyChanged(); } }

        public string Error { get => error; set { error = value; OnPropertyChanged(); } }

        public bool Success { get => success; set => success = value; }

        #endregion

        #region METHODS - PUBLIC

        public DialogProject(Window? parent = null)
        {
            Owner = parent;

            DataContext = this;

            SolutionItems = new ObservableCollection<KeyValuePair<int, string>>();

            InitializeComponent();

            LoadSolutions();
        }

        #endregion

        #region METHODS - PRIVATE

        private void Validate([CallerMemberName] string? propertyName = null)
        {
            string message = String.Empty;

            if (propertyName == "ProjectName")
            {
                if (string.IsNullOrEmpty(ProjectName) || string.IsNullOrWhiteSpace(ProjectName))
                {
                    message = "Project Name is empty!";
                }
            }
            else if (propertyName == "ProjectSolutionName")
            {
                if (ProjectChoiceNewSolution && string.IsNullOrEmpty(ProjectSolutionName))
                {
                    message = "Solution Name is empty!";
                }
            }

            if (errors.ContainsKey(propertyName))
            {
                errors.Remove(propertyName);

                if (message != String.Empty) errors.Add(propertyName, message);
            }
            else if (!errors.ContainsKey(propertyName) && message != String.Empty)
            {
                errors.Add(propertyName, message);
            }
        }

        private void Cancel(object sender, RoutedEventArgs e) { Success = false; Close(); }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            Error               = String.Empty;
            ProjectName         = ctbProjectName.Text;
            ProjectSolutionName = ctbSolutionName.Text;

            if (SolutionItems.Count() == 0)
            {
                Error = "Select a Solution!";

                return;
            }

            if (errors.Count() > 0)
            {
                Error = errors.First().Value;

                return;
            }

            if (edit == true)
            {
                EditProject();
            }
            else
            {
                InsertProject();
            }

            Success = true;

            Close();
        }

        private void LoadSolutions()
        {
            List<ROW_SOLUTION> t_solutions = new List<ROW_SOLUTION>();

            if (!SolutionsManager.Instance.SelectSolutions(out t_solutions)) return;

            SolutionItems.Clear();

            foreach (var solution in t_solutions)
            {
                SolutionItems.Add(new KeyValuePair<int, string>(solution.SolutionID, solution.Name));
            }

            ProjectSolution = SolutionItems.ElementAt(0);
        }

        private bool InsertProject()
        {
            ROW_PROJECT row = new ROW_PROJECT();

            row.Name = ProjectName;

            if (ProjectChoiceNoSolution)
            {
                row.SolutionID = null;
            }
            else if (ProjectChoiceNewSolution)
            {
                long solution_id = Solution.NullSolutionId;

                if (!InsertSolution(out solution_id)) return false;

                row.SolutionID = (int)solution_id;
            }
            else if (ProjectChoiceAddToSolution)
            {
                row.SolutionID = ProjectSolution.Key;
            }

            ProjectsManager.Instance.InsertProject(row);

            return true;
        }

        private bool EditProject()
        {
            ROW_PROJECT row = new ROW_PROJECT();

            row.ProjectID   = ProjectId;
            row.Name        = ProjectName;

            if (ProjectChoiceNoSolution)
            {
                row.SolutionID = null;
            }
            else if (ProjectChoiceNewSolution)
            {
                long solution_id = Solution.NullSolutionId;

                if (!InsertSolution(out solution_id)) return false;

                row.SolutionID = (int)solution_id;
            }
            else if (ProjectChoiceAddToSolution)
            {
                row.SolutionID = ProjectSolution.Key;
            }

            ProjectsManager.Instance.UpdateProject(row);

            return true;
        }

        private bool InsertSolution(out long solution_id)
        {
            solution_id = 0;

            ROW_SOLUTION row = new ROW_SOLUTION();

            row.Name = ProjectSolutionName;

            if (!SolutionsManager.Instance.InsertSolution(row)) return false;

            if (!DBMS.Instance.LastInsertRowId(out solution_id)) return false;

            return true;
        }

        #endregion
    }
}
