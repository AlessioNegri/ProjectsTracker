using ProjectsTracker.src.Database;
using ProjectsTracker.src.MVVM;
using ProjectsTracker.src.Services;
using ProjectsTracker.ui.UserControls;
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

        private string solution_name = string.Empty;

        #endregion

        #region BINDINGS

        /// <summary> Navigation command </summary>
        public INavigationService Navigation { get => navigation; set { navigation = value; OnPropertyChanged(); } }

        /// <summary> Solution id </summary>
        public int SolutionId { get => solution_id; set => solution_id = value; }

        /// <summary> Solution name </summary>
        public string SolutionName { get => solution_name; set => solution_name = value; }

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
                card.SolutionId     = row.SolutionID ?? UC.CardProject.NullSolutionId;
                card.ProjectName    = row.Name;
                card.SolutionName   = row.SolutionName;
                card.Update         += Card_Update;
                card.OpenProject    += Card_OpenProject;

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

        /// <summary> Slot called when OpenProject event is invoked </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Card_OpenProject(object? sender, EventArgs e)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("ProjectId", ((OpenProjectEventArgs)e).ProjectId.ToString());
            data.Add("SolutionId", ((OpenProjectEventArgs)e).SolutionId.ToString());
            data.Add("SolutionName", ((OpenProjectEventArgs)e).SolutionName);

            Navigation.NavigateTo<PageProjectViewModel>(data);
        }

        #endregion
    }
}