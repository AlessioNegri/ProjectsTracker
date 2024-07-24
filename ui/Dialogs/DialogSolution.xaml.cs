using ProjectsTracker.src;
using ProjectsTracker.src.Database;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProjectsTracker.ui.Dialogs
{
    /// <summary> Logica di interazione per DialogSolution.xaml </summary>
    public partial class DialogSolution : Window, INotifyPropertyChanged
    {
        #region INTERFACE

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary> Invokes the propery change event </summary>
        /// <param name="propertyName"> Name of the property </param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region MEMBERS

        private bool edit = false;

        private string solution_header = string.Empty;

        private int solution_id = 0;

        private string solution_name = string.Empty;

        private bool solition_extract = false;

        private string error =  string.Empty;

        /// <summary> Dictionary to keep track of errors </summary>
        private Dictionary<string, string> errors = new Dictionary<string, string>();

        private bool success = false;

        #endregion

        #region BINDINGS

        /// <summary> True for edit dialog </summary>
        public bool Edit { get => edit; set { edit = value; cbSolutionExtract.Visibility = edit ? Visibility.Visible : Visibility.Hidden; } }

        /// <summary> Solution header </summary>
        public string SolutionHeader { get => solution_header; set { solution_header = value; OnPropertyChanged(); } }

        /// <summary> Solution id </summary>
        public int SolutionId { get => solution_id; set => solution_id = value; }

        /// <summary> Solution name </summary>
        public string SolutionName { get => solution_name; set { solution_name = ctbSolutionName.Text = value; Validate(); OnPropertyChanged(); } }

        /// <summary> Solution extract </summary>
        public bool SolutionExtract { get => solition_extract; set { solition_extract = value; OnPropertyChanged(); } }

        /// <summary> Error message </summary>
        public string Error { get => error; set { error = value; OnPropertyChanged(); } }

        /// <summary> Success of form </summary>
        public bool Success { get => success; set => success = value; }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        /// <param name="parent"> Parent Window </param>
        public DialogSolution(Window? parent = null)
        {
            Owner = parent;

            DataContext = this;

            InitializeComponent();

            cbSolutionExtract.Visibility = Visibility.Hidden;
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Validates the form fields </summary>
        /// <param name="propertyName"></param>
        private void Validate([CallerMemberName] string? propertyName = null)
        {
            string message = string.Empty;

            // Check errors

            if (propertyName == "SolutionName")
            {
                if (string.IsNullOrEmpty(SolutionName) || string.IsNullOrWhiteSpace(SolutionName))
                {
                    message = "Solution Name is empty!";
                }
            }

            // Update errors dictionary

            if (errors.ContainsKey(propertyName))
            {
                errors.Remove(propertyName);

                if (message != string.Empty) errors.Add(propertyName, message);
            }
            else if (!errors.ContainsKey(propertyName) && message != string.Empty)
            {
                errors.Add(propertyName, message);
            }
        }

        /// <summary> Cancel button action </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            Success = false;
            
            Close();
        }

        /// <summary> Confirm button action </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Confirm(object sender, RoutedEventArgs e)
        {
            Error           = string.Empty;
            SolutionName    = ctbSolutionName.Text;

            if (errors.Count() > 0)
            {
                Error = errors.First().Value;

                return;
            }

            if (edit == true)
            {
                EditSolution();
            }
            else
            {
                InsertSolution();
            }

            Success = true;

            Close();
        }

        /// <summary> Inserts a new solution </summary>
        /// <returns> Success of the operations </returns>
        private bool InsertSolution()
        {
            ROW_SOLUTION row = new ROW_SOLUTION();

            row.Name = SolutionName;

            if (!SolutionsManager.Instance.InsertSolution(row)) return false;

            return true;
        }

        /// <summary> Edits an existing solution </summary>
        /// <returns> Success of the operations </returns>
        private bool EditSolution()
        {
            ROW_SOLUTION row = new ROW_SOLUTION();

            row.SolutionID  = SolutionId;
            row.Name        = SolutionName;

            SolutionsManager.Instance.UpdateSolution(row, SolutionExtract);

            return true;
        }

        #endregion
    }
}