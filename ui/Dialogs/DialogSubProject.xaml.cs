﻿using ProjectsTracker.src;
using ProjectsTracker.src.Database;
using ProjectsTracker.ui.UserControls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProjectsTracker.ui.Dialogs
{
    /// <summary> Logica di interazione per DialogSubProject.xaml </summary>
    public partial class DialogSubProject : Window, INotifyPropertyChanged
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

        private ObservableCollection<KeyValuePair<int, string>> solution_items = new ObservableCollection<KeyValuePair<int, string>>();

        private bool edit = false;

        private int project_id = CardProject.NullProjectId;

        private int solution_id = CardSolution.NullSolutionId;

        private string project_header = string.Empty;

        private string project_name = string.Empty;

        private bool project_choice_leave_in_solution = true;

        private bool project_choice_extract_from_solution = false;

        private bool project_choice_add_to_solution = false;

        private KeyValuePair<int, string> project_solution = new KeyValuePair<int, string>();

        private string error = string.Empty;

        /// <summary> Dictionary to keep track of errors </summary>
        private Dictionary<string, string> errors = new Dictionary<string, string>();

        private bool success = false;

        #endregion

        #region BINDINGS

        /// <summary> Collection for solution combo box </summary>
        public ObservableCollection<KeyValuePair<int, string>> SolutionItems { get; set; }

        /// <summary> True for edit dialog </summary>
        public bool Edit { get => edit; set { edit = value; } }

        /// <summary> Project id </summary>
        public int ProjectId { get => project_id; set => project_id = value; }

        /// <summary> Solution id </summary>
        public int SolutionId { get => solution_id; set { solution_id = value; } }

        /// <summary> Project header </summary>
        public string ProjectHeader { get => project_header; set { project_header = value; OnPropertyChanged(); } }

        /// <summary> Project name </summary>
        public string ProjectName { get => project_name; set { project_name = ctbProjectName.Text = value; Validate(); OnPropertyChanged(); } }

        /// <summary> Project radio button (Leave In Solution) </summary>
        public bool ProjectChoiceLeaveInSolution { get => project_choice_leave_in_solution; set { project_choice_leave_in_solution = value; OnPropertyChanged(); } }

        /// <summary> Project radio button (Extract From Solution) </summary>
        public bool ProjectChoiceExtractFromSolution { get => project_choice_extract_from_solution; set { project_choice_extract_from_solution = value; OnPropertyChanged(); } }

        /// <summary> Project radio button (Add To Solution) </summary>
        public bool ProjectChoiceAddToSolution { get => project_choice_add_to_solution; set { project_choice_add_to_solution = value; OnPropertyChanged(); } }

        /// <summary> Project solution </summary>
        public KeyValuePair<int, string> ProjectSolution { get => project_solution; set { project_solution = value; OnPropertyChanged(); } }

        /// <summary> Error message </summary>
        public string Error { get => error; set { error = value; OnPropertyChanged(); } }

        /// <summary> Success of form </summary>
        public bool Success { get => success; set => success = value; }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        /// <param name="parent"> Parent Window </param>
        public DialogSubProject(Window? parent = null)
        {
            Owner = parent;

            DataContext = this;

            SolutionItems = new ObservableCollection<KeyValuePair<int, string>>();

            InitializeComponent();

            LoadSolutions();
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Validates the form fields </summary>
        /// <param name="propertyName"></param>
        private void Validate([CallerMemberName] string? propertyName = null)
        {
            string message = string.Empty;

            // Check errors

            if (propertyName == "ProjectName")
            {
                if (string.IsNullOrEmpty(ProjectName) || string.IsNullOrWhiteSpace(ProjectName))
                {
                    message = "Project Name is empty!";
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
            Error       = string.Empty;
            ProjectName = ctbProjectName.Text;

            if (ProjectChoiceAddToSolution && SolutionItems.Count() == 0)
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

        /// <summary> Load the list of solutions </summary>
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

        /// <summary> Inserts a new project </summary>
        /// <returns> Success of the operations </returns>
        private bool InsertProject()
        {
            ROW_PROJECT row = new ROW_PROJECT();

            row.Name = ProjectName;

            if (ProjectChoiceLeaveInSolution)
            {
                row.SolutionID = SolutionId;
            }
            else if (ProjectChoiceExtractFromSolution)
            {
                row.SolutionID = null;
            }
            else if (ProjectChoiceAddToSolution)
            {
                row.SolutionID = ProjectSolution.Key;
            }

            ProjectsManager.Instance.InsertProject(row);

            return true;
        }

        /// <summary> Edits an existing project </summary>
        /// <returns> Success of the operations </returns>
        private bool EditProject()
        {
            ROW_PROJECT row = new ROW_PROJECT();

            row.ProjectID   = ProjectId;
            row.Name        = ProjectName;

            if (ProjectChoiceLeaveInSolution)
            {
                row.SolutionID = SolutionId;
            }
            else if (ProjectChoiceExtractFromSolution)
            {
                row.SolutionID = null;
            }
            else if (ProjectChoiceAddToSolution)
            {
                row.SolutionID = ProjectSolution.Key;
            }

            ProjectsManager.Instance.UpdateProject(row);

            return true;
        }

        #endregion
    }
}