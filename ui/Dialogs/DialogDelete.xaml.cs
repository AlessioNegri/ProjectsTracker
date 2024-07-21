using ProjectsTracker.src.MVVM;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProjectsTracker.ui.Dialogs
{
    /// <summary>
    /// Logica di interazione per DialogDelete.xaml
    /// </summary>
    public partial class DialogDelete : Window, INotifyPropertyChanged
    {
        #region INTERFACES

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region MEMBERS

        private string delete_header = String.Empty;

        private string delete_content = String.Empty;

        private bool success = false;

        #endregion

        #region BINDINGS

        public string DeleteHeader { get { return delete_header; } set { delete_header = value; OnPropertyChanged(); } }

        public string DeleteContent { get { return delete_content; } set { delete_content = value; OnPropertyChanged(); } }

        public bool Success { get => success; set => success = value; }

        #endregion

        #region COMMANDS

        public RelayCommand ConfirmCommand { get; set; }

        #endregion

        #region METHODS - PUBLIC

        public DialogDelete(Window? parent = null)
        {
            Owner = parent;

            DataContext = this;

            ConfirmCommand = new RelayCommand(execute => { });

            InitializeComponent();
        }

        public void SetRelayCommand(RelayCommand command)
        {
            ConfirmCommand = command;
        }

        #endregion

        private void Cancel(object sender, RoutedEventArgs e) { Success = false; Close(); }

        private void Confirm(object sender, RoutedEventArgs e) { ConfirmCommand.Execute(this); Success = true; Close(); }
    }
}
