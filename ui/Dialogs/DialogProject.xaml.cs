using ProjectsTracker.src;
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

        #endregion

        #region MEMBERS

        private bool edit = false;

        private string project_header = String.Empty;

        private string project_name = String.Empty;

        private bool project_choice_no_solution = true;

        private bool project_choice_new_solution = false;

        private bool project_choice_add_to_solution = false;

        private string project_solution_name = String.Empty;

        private int project_solution = 0;

        private string error = String.Empty;

        private Dictionary<string, string> errors = new Dictionary<string, string>();

        #endregion

        #region BINDINGS

        public bool Edit { get { return edit; } set { edit = value; } }

        public string ProjectHeader { get { return project_header; } set { project_header = value; OnPropertyChanged(); } }

        public string ProjectName { get { return project_name; } set { project_name = ctbProjectName.Text = value; Validate(); OnPropertyChanged(); } }

        public bool ProjectChoiceNoSolution { get { return project_choice_no_solution; } set { project_choice_no_solution = value; OnPropertyChanged(); } }

        public bool ProjectChoiceNewSolution { get { return project_choice_new_solution; } set { project_choice_new_solution = value; OnPropertyChanged(); } }

        public bool ProjectChoiceAddToSolution { get { return project_choice_add_to_solution; } set { project_choice_add_to_solution = value; OnPropertyChanged(); } }

        public string ProjectSolutionName { get { return project_solution_name; } set { project_solution_name = value; Validate(); OnPropertyChanged(); } }

        public int ProjectSolution { get { return project_solution; } set { project_solution = value; OnPropertyChanged(); } }

        public string Error { get { return error; } set { error = value; OnPropertyChanged(); } }

        #endregion

        #region METHODS - PUBLIC

        public DialogProject(Window? parent = null)
        {
            Owner = parent;

            DataContext = this;

            InitializeComponent();
        }

        #endregion

        #region METHODS - PRIVATE

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            Error               = String.Empty;
            ProjectName         = ctbProjectName.Text;
            ProjectSolutionName = ctbSolutionName.Text;

            if (errors.Count() > 0)
            {
                Error = errors.First().Value;

                return;
            }

            if (edit == true) // EDIT
            {

            }
            else // NEW
            {
                //InsertProject();
            }

            Close();
        }

        private bool InsertProject()
        {
            ROW_PROJECT row = new ROW_PROJECT();

            //if (Proje)

            row.Name = ProjectName;

            if (ProjectChoiceNoSolution)
            {
                row.SolutionID = null;
            }
            else if (ProjectChoiceNewSolution)
            {

            }
            else if (ProjectChoiceAddToSolution)
            {

            }

            //ProjectsManager.Instance.InsertProject(row);

            return true;
        }

        private bool InsertSolution()
        {
            return true;
        }

        #endregion
    }
}
