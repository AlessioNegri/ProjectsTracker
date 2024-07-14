using ProjectsTracker.src;
using ProjectsTracker.src.Database;
using ProjectsTracker.src.Models;
using ProjectsTracker.ui.Dialogs;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ProjectsTracker.ui.UserControls
{
    /// <summary>
    /// Logica di interazione per Card.xaml
    /// </summary>
    public partial class CardProject : UserControl, INotifyPropertyChanged
    {
        #region INTERFACE

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region MEMBERS

        private Window? parent_window = null;

        private int project_id = Project.NullProjectId;

        private string project_name = string.Empty;

        #endregion

        #region BINDINGS

        public int ProjectId { get => project_id; set { project_id = value; OnPropertyChanged(); } }

        public string ProjectName { get => project_name; set { project_name = value; OnPropertyChanged(); } }

        #endregion

        #region METHODS - PUBLIC

        public CardProject(Window? parent = null)
        {
            parent_window = parent;

            DataContext = this;

            InitializeComponent();
        }

        #endregion

        #region METHODS - PRIVATE

        private void EditProject(object sender, System.Windows.RoutedEventArgs e)
        {
            ROW_PROJECT project = new ROW_PROJECT();

            bool ok = ProjectsManager.Instance.SelectProjectById(ProjectId, out project);

            if (!ok) return;

            DialogProject dlg = new DialogProject(parent_window);

            dlg.ProjectHeader   = $"Edit {project.Name}";
            dlg.ProjectName     = project.Name;

            Opacity = 0.5;

            dlg.ShowDialog();

            Opacity = 1.0;
        }

        private void DeleteProject(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        #endregion
    }
}
