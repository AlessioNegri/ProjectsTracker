using ProjectsTracker.src.Database;
using ProjectsTracker.src.ViewModels;
using ProjectsTracker.ui.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace ProjectsTracker.ui.Pages
{
    /// <summary> Logica di interazione per PageSolution.xaml </summary>
    public partial class PageSolution : UserControl
    {
        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        public PageSolution()
        {
            InitializeComponent();

            DataContextChanged += PageSolution_DataContextChanged;
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Slot called when DataContextChanged event is invoked </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void PageSolution_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                ((PageSolutionViewModel)DataContext).Update += PageSolution_Update;

                ((PageSolutionViewModel)DataContext).LoadSubProjectCards();

                LoadSubProjectCards();
            }
        }

        /// <summary> Slot called when Update event is invoked </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void PageSolution_Update(object? sender, EventArgs e)
        {
            if (DataContext != null)
            {
                ((PageSolutionViewModel)DataContext).LoadSubProjectCards();

                LoadSubProjectCards();
            }
        }

        /// <summary> Loads the Sub Project Cards on the UI </summary>
        private void LoadSubProjectCards()
        {
            _sub_projects_container_.Children.Clear();

            foreach (var project in ((PageSolutionViewModel)DataContext).SubProjects)
            {
                _sub_projects_container_.Children.Add(project);
            }

            _sub_projects_container_.UpdateLayout();
        }

        /// <summary> Opens the Add Sub Project Dialog </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void AddSubProjectDialog(object sender, RoutedEventArgs e)
        {
            DialogSubProject dlg = new DialogSubProject(Application.Current.MainWindow);

            dlg.SolutionId      = ((PageSolutionViewModel)DataContext).SolutionId;
            dlg.ProjectHeader   = "Create New Sub Project";

            dlg.ShowDialog();

            if (dlg.Success)
            {
                ((PageSolutionViewModel)DataContext).LoadSubProjectCards();

                LoadSubProjectCards();
            }
        }

        /// <summary> Drag Drop slot </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void CardDrop(object sender, DragEventArgs e)
        {
            int project_id = (int)e.Data.GetData(DataFormats.Serializable);

            ProjectsManager.Instance.MoveProjectOutSolution(project_id);

            ((PageSolutionViewModel)DataContext).LoadSubProjectCards();

            LoadSubProjectCards();
        }

        #endregion
    }
}