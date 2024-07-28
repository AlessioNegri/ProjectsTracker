using System.Windows;

namespace ProjectsTracker.src.MVVM
{
    /// <summary> Class to manage global variables </summary>
    class Globals : ViewModelBase
    {
        #region EVENTS

        /// <summary> Event triggered to notify the UI update </summary>
        public event EventHandler? BackIconVisibilityChanged = null;

        /// <summary> Event triggered to notify the UI update </summary>
        public event EventHandler? HomeIconVisibilityChanged = null;

        #endregion

        #region MEMBERS

        /// <summary> Singleton instance </summary>
        private static Globals? instance = null;

        /// <summary> Thread lock </summary>
        private static readonly object padlock = new object();

        private Visibility back_icon_visibility = Visibility.Hidden;

        private Visibility home_icon_visibility = Visibility.Hidden;

        #endregion

        /// <summary> Stores the visibility of the Back Icon </summary>
        public Visibility BackIconVisibility
        {
            get => back_icon_visibility;

            set
            {
                back_icon_visibility = value;
                
                if (BackIconVisibilityChanged != null) BackIconVisibilityChanged(this, new EventArgs());
            }
        }

        /// <summary> Stores the visibility of the Home Icon </summary>
        public Visibility HomeIconVisibility
        {
            get => home_icon_visibility;

            set
            {
                home_icon_visibility = value;

                if (HomeIconVisibilityChanged != null) HomeIconVisibilityChanged(this, new EventArgs());
            }
        }

        #region METHODS - PUBLIC

        /// <summary> Retrieves the class instance </summary>
        public static Globals Instance
        {
            get { lock (padlock) { if (instance is null) instance = new Globals(); return instance; } }
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Constructor </summary>
        private Globals() { }

        #endregion
    }
}
