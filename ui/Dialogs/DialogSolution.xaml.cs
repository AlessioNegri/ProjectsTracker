using ProjectsTracker.src;
using ProjectsTracker.src.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectsTracker.ui.Dialogs
{
    /// <summary>
    /// Logica di interazione per DialogSolution.xaml
    /// </summary>
    public partial class DialogSolution : Window, INotifyPropertyChanged
    {
        #region INTERFACES

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region MEMBERS

        private bool edit = false;

        private string solution_header = String.Empty;

        private int solution_id = 0;

        private string solution_name = String.Empty;

        private bool solition_extract = false;

        private string error = String.Empty;

        private Dictionary<string, string> errors = new Dictionary<string, string>();

        private bool success = false;

        #endregion

        #region BINDINGS

        public bool Edit { get => edit; set { edit = value; cbSolutionExtract.Visibility = edit ? Visibility.Visible : Visibility.Hidden; } }

        public string SolutionHeader { get => solution_header; set { solution_header = value; OnPropertyChanged(); } }

        public int SolutionId { get => solution_id; set => solution_id = value; }

        public string SolutionName { get => solution_name; set { solution_name = ctbSolutionName.Text = value; Validate(); OnPropertyChanged(); } }

        public bool SolutionExtract { get => solition_extract; set { solition_extract = value; OnPropertyChanged(); } }

        public string Error { get => error; set { error = value; OnPropertyChanged(); } }

        public bool Success { get => success; set => success = value; }

        #endregion

        #region METHODS - PUBLIC

        public DialogSolution(Window? parent = null)
        {
            Owner = parent;

            DataContext = this;

            InitializeComponent();

            cbSolutionExtract.Visibility = Visibility.Hidden;
        }

        #endregion

        #region METHODS - PRIVATE

        private void Validate([CallerMemberName] string? propertyName = null)
        {
            string message = String.Empty;

            if (propertyName == "SolutionName")
            {
                if (string.IsNullOrEmpty(SolutionName) || string.IsNullOrWhiteSpace(SolutionName))
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
            Error           = String.Empty;
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

        private bool InsertSolution()
        {
            ROW_SOLUTION row = new ROW_SOLUTION();

            row.Name = SolutionName;

            if (!SolutionsManager.Instance.InsertSolution(row)) return false;

            return true;
        }

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
