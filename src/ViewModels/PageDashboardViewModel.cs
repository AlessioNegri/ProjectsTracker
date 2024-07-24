using ProjectsTracker.src.Database;
using ProjectsTracker.src.MVVM;
using ProjectsTracker.src.Services;
using System.Collections.ObjectModel;
using System.Windows;
using UC = ProjectsTracker.ui.UserControls;

namespace ProjectsTracker.src.ViewModels
{
    /// <summary> PageDashboard View Model </summary>
    class PageDashboardViewModel : ViewModelBase
    {
        #region EVENTS

        /// <summary> Event triggered to notify the UI update </summary>
        public event EventHandler? Update = null;

        #endregion

        #region MEMBERS

        private INavigationService navigation;

        #endregion

        #region BINDINGS

        /// <summary> Navigation command </summary>
        public INavigationService Navigation { get => navigation; set { navigation = value; OnPropertyChanged(); } }

        /// <summary> Collection of Project Cards </summary>
        public ObservableCollection<UC.CardProject> Projects { get; set; }

        /// <summary> Collection of Solution Cards </summary>
        public ObservableCollection<UC.CardSolution> Solutions { get; set; }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        /// <param name="service"> Navigation service </param>
        public PageDashboardViewModel(INavigationService service)
        {
            Navigation = service;

            Projects = new ObservableCollection<UC.CardProject>();

            Solutions = new ObservableCollection<UC.CardSolution>();
        }

        /// <summary> Load the Project Cards </summary>
        public void LoadProjectCards()
        {
            Projects.Clear();

            var t_projects = new List<ROW_PROJECT>();

            ProjectsManager.Instance.SelectProjects(out t_projects);

            foreach (var row in t_projects)
            {
                UC.CardProject card = new UC.CardProject();

                card.ProjectId      = row.ProjectID;
                card.ProjectName    = row.Name;
                card.Update         += Card_Update;

                Projects.Add(card);
            }
        }

        /// <summary> Load the Project Cards </summary>
        public void LoadSolutionCards()
        {
            Solutions.Clear();

            var t_solutions = new List<ROW_SOLUTION>();

            SolutionsManager.Instance.SelectSolutions(out t_solutions);

            foreach (var row in t_solutions)
            {
                UC.CardSolution card = new UC.CardSolution();

                card.SolutionId     = row.SolutionID;
                card.SolutionName   = row.Name;
                card.SubProjects    = $"N° Sub-Projects: {row.SubProjects}";
                card.Update         += Card_Update;

                Solutions.Add(card);
            }
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Slot called when Update event is invoked </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Card_Update(object? sender, EventArgs e)
        {
            LoadProjectCards();
            LoadSolutionCards();

            if (Update != null) Update(this, new EventArgs());
        }

        #endregion
    }
}