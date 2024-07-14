using ProjectsTracker.src.Database;
using ProjectsTracker.src.ViewModels;
using ProjectsTracker.ui.Dialogs;
using System.Windows;

using uc = ProjectsTracker.ui.UserControls;

namespace ProjectsTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProjectsViewModel projects_view_model { get; set; }

        private SolutionsViewModel solutions_view_model { get; set; }

        public MainWindow()
        {
            DataContext = this;

            DBMS.Instance.Init();

            projects_view_model     = new ProjectsViewModel();
            solutions_view_model    = new SolutionsViewModel();

            InitializeComponent();

            LoadProjectCards();
            LoadSolutionCards();
        }

        private void LoadProjectCards()
        {
            projects_view_model.LoadProjects();

            _projects_container_.Children.Clear();

            foreach (var project in projects_view_model.Projects)
            {
                uc.CardProject card = new uc.CardProject(this);

                card.ProjectId      = project.Id;
                card.ProjectName    = project.Name;

                _projects_container_.Children.Add(card);
            }

            _projects_container_.UpdateLayout();
        }

        private void LoadSolutionCards()
        {
            solutions_view_model.LoadSolutions();

            _solutions_container_.Children.Clear();

            foreach (var solution in solutions_view_model.Solutions)
            {
                uc.CardSolution card = new uc.CardSolution(this);

                card.SolutionId     = solution.Id;
                card.SolutionName   = solution.Name;

                _solutions_container_.Children.Add(card);
            }

            _solutions_container_.UpdateLayout();
        }

        private void AddProjectDialog(object sender, RoutedEventArgs e)
        {
            DialogProject dlg = new DialogProject(this);

            dlg.ProjectHeader = "Create New Project";

            Opacity = 0.5;

            dlg.ShowDialog();

            Opacity = 1.0;
        }

        private void AddSolutionDialog(object sender, RoutedEventArgs e)
        {

        }
    }
}