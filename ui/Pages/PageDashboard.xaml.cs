using ProjectsTracker.src.ViewModels;
using ProjectsTracker.ui.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace ProjectsTracker.ui.Pages
{
    /// <summary>
    /// Logica di interazione per PageDashboard.xaml
    /// </summary>
    public partial class PageDashboard : UserControl
    {
        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        public PageDashboard()
        {
            InitializeComponent();
            
            DataContextChanged += PageDashboard_DataContextChanged;
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Slot called when DataContextChanged event is invoked </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void PageDashboard_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((PageDashboardViewModel)DataContext).Update += PageDashboard_Update;

            ((PageDashboardViewModel)DataContext).LoadProjectCards();
            ((PageDashboardViewModel)DataContext).LoadSolutionCards();

            LoadProjectCards();
            LoadSolutionCards();
        }

        /// <summary> Slot called when Update event is invoked </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void PageDashboard_Update(object? sender, EventArgs e)
        {
            LoadProjectCards();
            LoadSolutionCards();
        }

        /// <summary> Loads the Project Cards on the UI </summary>
        private void LoadProjectCards()
        {
            _projects_container_.Children.Clear();

            foreach (var project in ((PageDashboardViewModel)DataContext).Projects)
            {
                _projects_container_.Children.Add(project);
            }

            _projects_container_.UpdateLayout();
        }

        /// <summary> Loads the Solution Cards on the UI </summary>
        private void LoadSolutionCards()
        {
            _solutions_container_.Children.Clear();

            foreach (var project in ((PageDashboardViewModel)DataContext).Solutions)
            {
                _solutions_container_.Children.Add(project);
            }

            _solutions_container_.UpdateLayout();
        }
        
        /// <summary> Opens the Add Project Dialog </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void AddProjectDialog(object sender, RoutedEventArgs e)
        {
            DialogProject dlg = new DialogProject(Application.Current.MainWindow);

            dlg.ProjectHeader = "Create New Project";

            dlg.ShowDialog();

            if (dlg.Success)
            {
                LoadProjectCards();
                LoadSolutionCards();
            }
        }

        /// <summary> Opens the Add Solution Dialog </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void AddSolutionDialog(object sender, RoutedEventArgs e)
        {
            DialogSolution dlg = new DialogSolution(Application.Current.MainWindow);

            dlg.SolutionHeader = "Create New Solution";

            dlg.ShowDialog();

            if (dlg.Success)
            {
                LoadProjectCards();
                LoadSolutionCards();
            }
        }

        #endregion
    }
}