using ProjectsTracker.src.MVVM;

namespace ProjectsTracker.src.Services
{
    /// <summary> Interface for navigation service </summary>
    interface INavigationService
    {
        /// <summary> Current view </summary>
        public ViewModelBase CurrentView { get; }

        /// <summary> Navigation utility </summary>
        /// <typeparam name="T"> View model </typeparam>
        public void NavigateTo<T>() where T : ViewModelBase;
    }

    /// <summary> Class to manage the navigation between views </summary>
    class NavigationService : ViewModelBase, INavigationService
    {
        #region MEMBERS

        private ViewModelBase current_view;

        /// <summary> View model factory </summary>
        private Func<Type, ViewModelBase> view_model_factory;

        #endregion

        #region BINDINGS

        /// <summary> Stores the current view </summary>
        public ViewModelBase CurrentView { get => current_view; set { current_view = value; OnPropertyChanged(); } }

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
        public void NavigateTo<T>() where T : ViewModelBase => CurrentView = view_model_factory.Invoke(typeof(T));

        #endregion
    }
}
