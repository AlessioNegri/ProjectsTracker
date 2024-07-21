using ProjectsTracker.src.Database;
using ProjectsTracker.src.ViewModels;
using ProjectsTracker.ui.Dialogs;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

using UC = ProjectsTracker.ui.UserControls;

namespace ProjectsTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INTERFACES

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private ProjectsViewModel projects_view_model { get; set; }

        private SolutionsViewModel solutions_view_model { get; set; }

        private string maximize_icon = "/icons/maximize.svg";

        public string MaximizeIcon { get => maximize_icon; set { maximize_icon = value; OnPropertyChanged(); } }

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
                UC.CardProject card = new UC.CardProject(this);

                card.ProjectId      = project.Id;
                card.ProjectName    = project.Name;

                card.Update += Card_Update;

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
                UC.CardSolution card = new UC.CardSolution(this);

                card.SolutionId     = solution.Id;
                card.SolutionName   = solution.Name;
                card.SubProjects    = $"N° Sub-Projects: {solution.SubProjects}";

                card.Update += Card_Update;

                _solutions_container_.Children.Add(card);
            }

            _solutions_container_.UpdateLayout();
        }

        private void AddProjectDialog(object sender, RoutedEventArgs e)
        {
            DialogProject dlg = new DialogProject(this);

            dlg.ProjectHeader = "Create New Project";

            dlg.ShowDialog();

            LoadProjectCards();
            LoadSolutionCards();
        }

        private void AddSolutionDialog(object sender, RoutedEventArgs e)
        {
            DialogSolution dlg = new DialogSolution(this);

            dlg.SolutionHeader = "Create New Solution";

            dlg.ShowDialog();

            LoadProjectCards();
            LoadSolutionCards();
        }

        private void Drag(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Minimize(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;

                MaximizeIcon = "/icons/maximize.svg";
            }
            else
            {
                WindowState = WindowState.Maximized;

                MaximizeIcon = "/icons/restore.svg";
            }
        }

        private void Close(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Card_Update(object? sender, EventArgs e)
        {
            LoadProjectCards();
            LoadSolutionCards();
        }
    }
}