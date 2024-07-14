using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace ProjectsTracker.src.MVVM
{
    class ViewModelBase : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
