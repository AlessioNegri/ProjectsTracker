using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ProjectsTracker.ui.UserControls
{
    /// <summary>
    /// Logica di interazione per CustomTextBox.xaml
    /// </summary>
    public partial class CustomTextBox : UserControl, INotifyPropertyChanged
    {
        #region INTERFACE

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region MEMBERS

        private string text = "";

        private string placeholder = "";

        #endregion

        #region BINDINGS

        public string Text { get => text; set { text = value; OnPropertyChanged(); } }

        public string Placeholder { get => placeholder; set { placeholder = value; OnPropertyChanged(); } }

        #endregion

        #region METHODS - PUBLIC

        public CustomTextBox()
        {
            DataContext = this;

            InitializeComponent();
        }

        #endregion

        #region METHODS - PRIVATE

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ctbClear_Click(object sender, RoutedEventArgs e)
        {
            ctbInput.Clear();
            ctbInput.Focus();
        }

        private void ctbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            ctbPlaceholder.Visibility = string.IsNullOrEmpty(ctbInput.Text) ? Visibility.Visible : Visibility.Hidden;
        }

        #endregion
    }
}
