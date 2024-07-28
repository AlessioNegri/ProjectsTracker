using ProjectsTracker.src.Database;
using ProjectsTracker.src.MVVM;
using ProjectsTracker.src.Services;
using System.Collections.ObjectModel;
using UC = ProjectsTracker.ui.UserControls;

namespace ProjectsTracker.src.ViewModels
{
    /// <summary> PageSolution View Model </summary>
    class PageSolutionViewModel : ViewModelBase
    {
        #region EVENTS

        /// <summary> Event triggered to notify the UI update </summary>
        public event EventHandler? Update = null;

        #endregion

        #region MEMBERS

        private INavigationService navigation;

        private int solution_id = UC.CardSolution.NullSolutionId;

        #endregion

        #region BINDINGS

        /// <summary> Navigation command </summary>
        public INavigationService Navigation { get => navigation; set { navigation = value; OnPropertyChanged(); } }

        /// <summary> Solution id </summary>
        public int SolutionId { get => solution_id; set => solution_id = value; }

        /// <summary> Collection of Project Cards </summary>
        public ObservableCollection<UC.CardProject> SubProjects { get; set; }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        /// <param name="service"> Navigation service </param>
        public PageSolutionViewModel(INavigationService service)
        {
            Navigation = service;

            solution_id = Int32.Parse(((NavigationService)service).Parameters!["SolutionId"]);

            SubProjects = new ObservableCollection<UC.CardProject>();
        }

        /// <summary> Load the Sub Project Cards </summary>
        public void LoadSubProjectCards()
        {
            SubProjects.Clear();

            var t_projects = new List<ROW_PROJECT>();

            ProjectsManager.Instance.SelectSubProjects(out t_projects, solution_id);

            foreach (var row in t_projects)
            {
                UC.CardProject card = new UC.CardProject();

                card.SubProject     = true;
                card.ProjectId      = row.ProjectID;
                card.ProjectName    = row.Name;
                card.Update         += Card_Update;

                SubProjects.Add(card);
            }
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Slot called when Update event is invoked </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Card_Update(object? sender, EventArgs e)
        {
            LoadSubProjectCards();

            if (Update != null) Update(this, new EventArgs());
        }

        #endregion
    }
}
