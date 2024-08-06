using ProjectsTracker.src;
using ProjectsTracker.src.Database;
using ProjectsTracker.ui.UserControls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProjectsTracker.ui.Dialogs
{
    /// <summary> Logica di interazione per DialogPATCH.xaml </summary>
    public partial class DialogPATCH : Window, INotifyPropertyChanged
    {
        #region READONLY

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

        private KeyValuePair<int, string> status = new KeyValuePair<int, string>();

        private string creation_date = "0000-00-00";

        private string closure_date = "0000-00-00";

        private string version = string.Empty;

        private string patch_version = string.Empty;

        #endregion

        #region BINDINGS

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

        /// <summary> Status </summary>
        public KeyValuePair<int, string> Status { get => status; set { status = value; OnPropertyChanged(); } }

        /// <summary> Creation Date </summary>
        public string CreationDate { get => creation_date; set { creation_date = value; OnPropertyChanged(); } }

        /// <summary> Closure Date </summary>
        public string ClosureDate { get => closure_date; set { closure_date = value; OnPropertyChanged(); } }

        /// <summary> Version </summary>
        public string Version { get => version; set { version = ctbVersion.Text = value; Validate(); OnPropertyChanged(); } }

        /// <summary> Patch Version </summary>
        public string PatchVersion { get => patch_version; set { patch_version = ctbPatchVersion.Text = value; Validate(); OnPropertyChanged(); } }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        /// <param name="parent"> Parent Window </param>
        public DialogPATCH(Window? parent = null)
        {
            Owner = parent;

            DataContext = this;

            StatusItems = new ObservableCollection<KeyValuePair<int, string>>();

            InitializeComponent();

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

            if (propertyName == "Version")
            {
                if (string.IsNullOrEmpty(Version) || string.IsNullOrWhiteSpace(Version))
                {
                    message = "Version is empty!";
                }
            }
            else if (propertyName == "PatchVersion")
            {
                if (string.IsNullOrEmpty(PatchVersion) || string.IsNullOrWhiteSpace(PatchVersion))
                {
                    message = "Patch Version is empty!";
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
            Version         = ctbVersion.Text;
            PatchVersion    = ctbPatchVersion.Text;

            if (errors.Count() > 0)
            {
                Error = errors.First().Value;

                return;
            }

            CreationDate    = (CreationDate == "0000-00-00" || CreationDate == "") ? CreationDate : DateTime.ParseExact(CreationDate, "M/d/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            ClosureDate     = (ClosureDate == "0000-00-00" || ClosureDate == "") ? ClosureDate : DateTime.ParseExact(ClosureDate, "M/d/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            if (edit == true)
            {
                EditPATCH();
            }
            else
            {
                InsertPATCH();
            }

            Success = true;

            Close();
        }

        /// <summary> Inserts a new PATCH </summary>
        /// <returns> Success of the operations </returns>
        private bool InsertPATCH()
        {
            var row = new ROW_TABLE();

            row.Status          = Status.Key;
            row.CreationDate    = CreationDate;
            row.ClosureDate     = ClosureDate;
            row.Version         = Version;
            row.PatchVersion    = PatchVersion;

            TablesManager.Instance.InsertPATCH(ProjectId, row);

            return true;
        }

        /// <summary> Edits an existing PATCH </summary>
        /// <returns> Success of the operations </returns>
        private bool EditPATCH()
        {
            var row = new ROW_TABLE();

            row.ID              = Id;
            row.Status          = Status.Key;
            row.CreationDate    = CreationDate;
            row.ClosureDate     = ClosureDate;
            row.Version         = Version;
            row.PatchVersion    = PatchVersion;

            TablesManager.Instance.UpdatePATCH(ProjectId, row);

            return true;
        }

        #endregion
    }
}
