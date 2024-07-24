using ProjectsTracker.src.MVVM;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProjectsTracker.ui.Dialogs
{
    /// <summary> Logica di interazione per DialogDelete.xaml </summary>
    public partial class DialogDelete : Window, INotifyPropertyChanged
    {
        #region INTERFACE

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary> Invokes the propery change event </summary>
        /// <param name="propertyName"> Name of the property </param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region COMMANDS

        /// <summary> Relay Command for confirm button </summary>
        public RelayCommand ConfirmCommand { get; set; }

        #endregion

        #region MEMBERS

        private string delete_header = string.Empty;

        private string delete_content = string.Empty;

        private bool success = false;

        #endregion

        #region BINDINGS

        /// <summary> Delete dialog header </summary>
        public string DeleteHeader { get { return delete_header; } set { delete_header = value; OnPropertyChanged(); } }

        /// <summary> Delete dialog content </summary>
        public string DeleteContent { get { return delete_content; } set { delete_content = value; OnPropertyChanged(); } }

        /// <summary> Success of form </summary>
        public bool Success { get => success; set => success = value; }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        /// <param name="parent"> Parent Window </param>
        public DialogDelete(Window? parent = null)
        {
            Owner = parent;

            DataContext = this;

            ConfirmCommand = new RelayCommand(execute => { });

            InitializeComponent();
        }

        /// <summary> Sets the relay command </summary>
        /// <param name="command"> Command </param>
        public void SetRelayCommand(RelayCommand command) => ConfirmCommand = command;

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Cancel button action </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            Success = false;
            
            Close();
        }

        /// <summary> Confirm button action </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Confirm(object sender, RoutedEventArgs e)
        {
            ConfirmCommand.Execute(this);
            
            Success = true;
            
            Close();
        }

        #endregion
    }
}