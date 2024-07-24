using ProjectsTracker.src.Database;
using ProjectsTracker.src.MVVM;
using ProjectsTracker.src.Services;
using ProjectsTracker.src.Utility;

namespace ProjectsTracker.src.ViewModels
{
    /// <summary> MainWindow View Model </summary>
    class MainWindowViewModel : ViewModelBase
    {
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
        }

        #endregion
    }
}
