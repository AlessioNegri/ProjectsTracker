using ProjectsTracker.src.MVVM;

namespace ProjectsTracker.src.Services
{
    /// <summary> Interface for navigation service </summary>
    interface INavigationService
    {
        /// <summary> Current view </summary>
        public ViewModelBase CurrentView { get; }

        /// <summary> Parameters </summary>
        Dictionary<string, string> Parameters { get; }

        /// <summary> Navigation utility </summary>
        /// <typeparam name="T"> View model </typeparam>
        /// <param name="parameters"> Parameters </param>
        public void NavigateTo<T>(in Dictionary<string, string>? parameters = null) where T : ViewModelBase;
    }

    /// <summary> Class to manage the navigation between views </summary>
    class NavigationService : ViewModelBase, INavigationService
    {
        #region EVENTS

        /// <summary> Event triggered to notify the UI update </summary>
        public event EventHandler? CurrentViewChanged = null;

        #endregion

        #region MEMBERS

        private ViewModelBase current_view;

        private Dictionary<string, string>? parameters;

        /// <summary> View model factory </summary>
        private Func<Type, ViewModelBase> view_model_factory;

        #endregion

        #region BINDINGS

        /// <summary> Stores the current view </summary>
        public ViewModelBase CurrentView { get => current_view; set { current_view = value; OnPropertyChanged(); } }

        /// <summary> Stores the parameters </summary>
        public Dictionary<string, string>? Parameters { get => parameters; set { parameters = value; OnPropertyChanged(); } }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        /// <param name="view_model_factory"> View model factory </param>
        public NavigationService(Func<Type, ViewModelBase> view_model_factory)
        {
            this.view_model_factory = view_model_factory;
        }

        /// <summary> Navigation utility </summary>
        /// <typeparam name="T"> View model </typeparam>
        /// <param name="parameters"> Parameters </param>
        public void NavigateTo<T>(in Dictionary<string, string>? parameters = null) where T : ViewModelBase
        {
            Parameters  = parameters;
            CurrentView = view_model_factory.Invoke(typeof(T));
        }

        #endregion
    }
}
