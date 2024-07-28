using ProjectsTracker.src;
using ProjectsTracker.src.Database;
using ProjectsTracker.src.MVVM;
using ProjectsTracker.ui.Dialogs;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjectsTracker.ui.UserControls
{
    /// <summary>
    /// Logica di interazione per Card.xaml
    /// </summary>
    public partial class CardProject : UserControl, INotifyPropertyChanged
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

        #region EVENTS

        /// <summary> Event triggered to notify the UI update </summary>
        public event EventHandler? Update = null;

        #endregion

        #region CONST

        /// <summary> Null project unique identifier </summary>
        public const int NullProjectId = 0;

        #endregion

        #region MEMBERS

        private bool sub_project = false;

        private string caption = "PROJECT";

        private int project_id = NullProjectId;

        private string project_name = string.Empty;

        #endregion

        #region BINDINGS

        /// <summary> True for a sub project </summary>
        public bool SubProject
        {
            get => sub_project;
            set
            {
                sub_project = value;

                if (sub_project)
                {
                    Caption = "SUB PROJECT";
                }
                else
                {
                    Caption = "PROJECT";
                }

                OnPropertyChanged();
            }
        }

        /// <summary> Project Caption </summary>
        public string Caption { get => caption; set { caption = value; OnPropertyChanged(); } }

        /// <summary> Project Id </summary>
        public int ProjectId { get => project_id; set { project_id = value; OnPropertyChanged(); } }

        /// <summary> Project Name </summary>
        public string ProjectName { get => project_name; set { project_name = value; OnPropertyChanged(); } }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        public CardProject()
        {
            DataContext = this;

            InitializeComponent();
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Opens the Edit Project Dialog </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void EditProject(object sender, RoutedEventArgs e)
        {
            ROW_PROJECT project = new ROW_PROJECT();

            bool ok = ProjectsManager.Instance.SelectProjectById(ProjectId, out project);

            if (!ok) return;

            if (SubProject)
            {
                DialogSubProject dlg = new DialogSubProject(Application.Current.MainWindow);

                dlg.Edit            = true;
                dlg.ProjectId       = project.ProjectID;
                dlg.SolutionId      = (int)project.SolutionID!;
                dlg.ProjectHeader   = $"Edit {project.Name}";
                dlg.ProjectName     = project.Name;

                dlg.ShowDialog();

                if (dlg.Success && Update != null) Update(this, new EventArgs());
            }
            else
            {
                DialogProject dlg = new DialogProject(Application.Current.MainWindow);

                dlg.Edit            = true;
                dlg.ProjectId       = project.ProjectID;
                dlg.ProjectHeader   = $"Edit {project.Name}";
                dlg.ProjectName     = project.Name;

                dlg.ShowDialog();

                if (dlg.Success && Update != null) Update(this, new EventArgs());
            }
        }

        /// <summary> Opens the Delete Project Dialog </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void DeleteProject(object sender, RoutedEventArgs e)
        {
            ROW_PROJECT project = new ROW_PROJECT();

            bool ok = ProjectsManager.Instance.SelectProjectById(ProjectId, out project);

            if (!ok) return;

            DialogDelete dlg = new DialogDelete(Application.Current.MainWindow);

            dlg.DeleteHeader    = SubProject ? "Delete Sub Project" : "Delete Project";
            dlg.DeleteContent   = $"Do you want to delete {project.Name}?";
            dlg.ConfirmCommand  = new RelayCommand(execute => { ProjectsManager.Instance.DeleteProject(ProjectId); });

            dlg.ShowDialog();

            if (dlg.Success && Update != null) Update(this, new EventArgs());
        }

        /// <summary> Mouse Move slot </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Card_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(cardProject, new DataObject(DataFormats.Serializable, ProjectId), DragDropEffects.Move);
            }
        }
        
        #endregion
    }
}
