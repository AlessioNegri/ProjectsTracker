using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectsTracker.src.MVVM
{
    /// <summary> Class to manage the base for a View Model </summary>
    class ViewModelBase : INotifyPropertyChanged
    {
        #region EVENTS

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region METHODS - PROTECTED

        /// <summary> Invokes the propery change event </summary>
        /// <param name="propertyName"> Name of the property </param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}