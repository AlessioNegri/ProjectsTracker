using ProjectsTracker.src;
using ProjectsTracker.src.Database;
using ProjectsTracker.src.Utility;
using ProjectsTracker.ui.UserControls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProjectsTracker.ui.Dialogs
{
    /// <summary> Logica di interazione per DialogPR.xaml </summary>
    public partial class DialogPR : Window, INotifyPropertyChanged
    {
        #region READONLY

        public readonly KeyValuePair<int, string> Priority1 = new KeyValuePair<int, string>(1, "1 (Low)");
        public readonly KeyValuePair<int, string> Priority2 = new KeyValuePair<int, string>(2, "2");
        public readonly KeyValuePair<int, string> Priority3 = new KeyValuePair<int, string>(3, "3");
        public readonly KeyValuePair<int, string> Priority4 = new KeyValuePair<int, string>(4, "4");
        public readonly KeyValuePair<int, string> Priority5 = new KeyValuePair<int, string>(5, "5 (High)");

        public readonly KeyValuePair<int, string> StatusAssigned    = new KeyValuePair<int, string>(0, "Assigned");
        public readonly KeyValuePair<int, string> StatusInProgress  = new KeyValuePair<int, string>(1, "In Progress");
        public readonly KeyValuePair<int, string> StatusDone        = new KeyValuePair<int, string>(2, "Done");

        #endregion

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

        private int project_id = CardProject.NullProjectId;

        private int id = 0;

        private bool success = false;

        private string error = string.Empty;

        /// <summary> Dictionary to keep track of errors </summary>
        private Dictionary<string, string> errors = new Dictionary<string, string>();

        private string header = string.Empty;

        private KeyValuePair<int, string> priority = new KeyValuePair<int, string>();

        private KeyValuePair<int, string> status = new KeyValuePair<int, string>();

        private string creation_date = "0000-00-00";

        private string closure_date = "0000-00-00";

        private string version = string.Empty;

        private string patch_version = string.Empty;

        private string discovery_version = string.Empty;

        private string title = string.Empty;

        private string description = string.Empty;

        private string note = string.Empty;

        #endregion

        #region BINDINGS

        /// <summary> Collection for priority combo box </summary>
        public ObservableCollection<KeyValuePair<int, string>> PriorityItems { get; set; }

        /// <summary> Collection for status combo box </summary>
        public ObservableCollection<KeyValuePair<int, string>> StatusItems { get; set; }

        /// <summary> True for edit dialog </summary>
        public bool Edit { get => edit; set { edit = value; } }

        /// <summary> Project id </summary>
        public int ProjectId { get => project_id; set => project_id = value; }

        /// <summary> ECR Id </summary>
        public int Id { get => id; set { id = value; } }

        /// <summary> Error message </summary>
        public string Error { get => error; set { error = value; OnPropertyChanged(); } }

        /// <summary> Dialog header </summary>
        public string Header { get => header; set { header = value; OnPropertyChanged(); } }

        /// <summary> Success of form </summary>
        public bool Success { get => success; set { success = value; OnPropertyChanged(); } }

        /// <summary> Priority </summary>
        public KeyValuePair<int, string> Priority { get => priority; set { priority = value; OnPropertyChanged(); } }

        /// <summary> Status </summary>
        public KeyValuePair<int, string> Status { get => status; set { status = value; OnPropertyChanged(); } }

        /// <summary> Creation Date </summary>
        public string CreationDate { get => creation_date; set { creation_date = value; OnPropertyChanged(); } }

        /// <summary> Closure Date </summary>
        public string ClosureDate { get => closure_date; set { closure_date = value; OnPropertyChanged(); } }

        /// <summary> Version </summary>
        public string Version { get => version; set { version = ctbVersion.Text = value; OnPropertyChanged(); } }

        /// <summary> Patch Version </summary>
        public string PatchVersion { get => patch_version; set { patch_version = ctbPatchVersion.Text = value; OnPropertyChanged(); } }

        /// <summary> Discovery Version </summary>
        public string DiscoveryVersion { get => discovery_version; set { discovery_version = ctbDiscoveryVersion.Text = value; Validate(); OnPropertyChanged(); } }

        /// <summary> Title </summary>
        public string PRTitle { get => title; set { title = ctbTitle.Text = value; Validate(); OnPropertyChanged(); } }

        /// <summary> Description </summary>
        public string Description { get => description; set { description = ctbDescription.Text = value; OnPropertyChanged(); } }

        /// <summary> Note </summary>
        public string Note { get => note; set { note = ctbNote.Text = value; OnPropertyChanged(); } }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        /// <param name="parent"> Parent Window </param>
        public DialogPR(Window? parent = null)
        {
            Owner = parent;

            DataContext = this;

            PriorityItems = new ObservableCollection<KeyValuePair<int, string>>();

            StatusItems = new ObservableCollection<KeyValuePair<int, string>>();

            InitializeComponent();

            // Init Priority Combo Box

            PriorityItems.Clear();

            PriorityItems.Add(Priority1);
            PriorityItems.Add(Priority2);
            PriorityItems.Add(Priority3);
            PriorityItems.Add(Priority4);
            PriorityItems.Add(Priority5);

            PriorityItems.ElementAt(0);

            // Init Status Combo Box

            StatusItems.Clear();

            StatusItems.Add(StatusAssigned);
            StatusItems.Add(StatusInProgress);
            StatusItems.Add(StatusDone);

            StatusItems.ElementAt(0);
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Validates the form fields </summary>
        /// <param name="propertyName"></param>
        private void Validate([CallerMemberName] string? propertyName = null)
        {
            string message = string.Empty;

            // Check errors

            if (propertyName == "PRTitle")
            {
                if (string.IsNullOrEmpty(PRTitle) || string.IsNullOrWhiteSpace(PRTitle))
                {
                    message = "Title is empty!";
                }
            }
            else if (propertyName == "DiscoveryVersion")
            {
                if (string.IsNullOrEmpty(DiscoveryVersion) || string.IsNullOrWhiteSpace(DiscoveryVersion))
                {
                    message = "Discovery Version is empty!";
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
            Error               = string.Empty;
            CreationDate        = Common.ParseDate(CreationDate);
            ClosureDate         = Common.ParseDate(ClosureDate);
            Version             = ctbVersion.Text;
            PatchVersion        = ctbPatchVersion.Text;
            DiscoveryVersion    = ctbDiscoveryVersion.Text;
            PRTitle             = ctbTitle.Text;
            Description         = ctbDescription.Text;
            Note                = ctbNote.Text;

            if (errors.Count() > 0)
            {
                Error = errors.First().Value;

                return;
            }

            if (edit == true)
            {
                EditPR();
            }
            else
            {
                InsertPR();
            }

            Success = true;

            Close();
        }

        /// <summary> Inserts a new PR </summary>
        /// <returns> Success of the operations </returns>
        private bool InsertPR()
        {
            var row = new ROW_TABLE();

            row.Priority            = Priority.Key;
            row.Status              = Status.Key;
            row.CreationDate        = CreationDate;
            row.ClosureDate         = ClosureDate;
            row.Version             = Version;
            row.PatchVersion        = PatchVersion;
            row.ReferenceVersion    = DiscoveryVersion;
            row.Title               = PRTitle;
            row.Description         = Description;
            row.Note                = Note;

            TablesManager.Instance.InsertPR(ProjectId, row);

            return true;
        }

        /// <summary> Edits an existing PR </summary>
        /// <returns> Success of the operations </returns>
        private bool EditPR()
        {
            var row = new ROW_TABLE();

            row.ID                  = Id;
            row.Priority            = Priority.Key;
            row.Status              = Status.Key;
            row.CreationDate        = CreationDate;
            row.ClosureDate         = ClosureDate;
            row.Version             = Version;
            row.PatchVersion        = PatchVersion;
            row.ReferenceVersion    = DiscoveryVersion;
            row.Title               = PRTitle;
            row.Description         = Description;
            row.Note                = Note;

            TablesManager.Instance.UpdatePR(ProjectId, row);

            return true;
        }

        #endregion
    }
}