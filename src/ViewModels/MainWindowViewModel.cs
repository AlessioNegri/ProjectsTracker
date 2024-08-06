using ProjectsTracker.src.Database;
using ProjectsTracker.src.MVVM;
using ProjectsTracker.src.Services;
using ProjectsTracker.src.Utility;

namespace ProjectsTracker.src.ViewModels
{
    /// <summary> MainWindow View Model </summary>
    class MainWindowViewModel : ViewModelBase
    {
        #region EVENTS

        /// <summary> Event triggered to notify the UI update </summary>
        public event EventHandler? WindowTitleChanged = null;

        /// <summary> Event triggered to notify the UI update </summary>
        public event EventHandler? BackIconVisibilityChanged = null;

        /// <summary> Event triggered to notify the UI update </summary>
        public event EventHandler? HomeIconVisibilityChanged = null;

        #endregion

        #region MEMBERS

        private string maximize_icon = "/icons/maximize.svg";

        private INavigationService navigation;

        #endregion

        #region BINDINGS

        /// <summary> Stores the maximize icon </summary>
        public string MaximizeIcon { get => maximize_icon; set { maximize_icon = value; OnPropertyChanged(); } }

        /// <summary> Navigation command </summary>
        public INavigationService Navigation {  get => navigation; set { navigation = value; OnPropertyChanged(); } }

        /// <summary> Navigation toward the Dashboard page </summary>
        public RelayCommand NavigateToDashboard { get; set; }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        /// <param name="service"> Navigation service </param>
        public MainWindowViewModel(INavigationService service)
        {
            Logger.Instance.Init();

            DBMS.Instance.Init();

            Navigation = service;

            NavigateToDashboard = new RelayCommand(execute => Navigation.NavigateTo<PageDashboardViewModel>());

            Navigation.NavigateTo<PageDashboardViewModel>();

            Globals.Instance.WindowTitleChanged         += Instance_WindowTitleChanged;
            Globals.Instance.HomeIconVisibilityChanged  += Instance_HomeIconVisibilityChanged;
            Globals.Instance.BackIconVisibilityChanged  += Instance_BackIconVisibilityChanged;
        }

        /// <summary> Navigates back to the solution page </summary>
        public void NavigateToSolution()
        {
            Globals.Instance.WindowTitle = ((NavigationService)Navigation).Parameters!["SolutionName"].ToUpper();

            var solution_id = Int32.Parse(((NavigationService)Navigation).Parameters!["SolutionId"]);

            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("SolutionId", solution_id.ToString());

            Navigation.NavigateTo<PageSolutionViewModel>(data);
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Notifies the View that the Window title has changed </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Instance_WindowTitleChanged(object? sender, EventArgs e)
        {
            if (WindowTitleChanged != null) WindowTitleChanged(this, new EventArgs());
        }

        /// <summary> Notifies the View that the Home Icon visibility has changed </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Instance_HomeIconVisibilityChanged(object? sender, EventArgs e)
        {
            if (HomeIconVisibilityChanged != null) HomeIconVisibilityChanged(this, new EventArgs());
        }

        /// <summary> Notifies the View that the Back Icon visibility has changed </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Instance_BackIconVisibilityChanged(object? sender, EventArgs e)
        {
            if (BackIconVisibilityChanged != null) BackIconVisibilityChanged(this, new EventArgs());
        }

        #endregion
    }
}