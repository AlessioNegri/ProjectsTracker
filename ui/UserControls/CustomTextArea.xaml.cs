using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ProjectsTracker.ui.UserControls
{
    /// <summary> Logica di interazione per CustomTextArea.xaml </summary>
    public partial class CustomTextArea : UserControl, INotifyPropertyChanged
    {
        #region INTERFACE

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary> Invokes the propery change event </summary>
        /// <param name="propertyName"> Name of the property </param>
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region MEMBERS

        private string text = string.Empty;

        private string placeholder = string.Empty;

        #endregion

        #region BINDINGS

        /// <summary> Text </summary>
        public string Text { get => text; set { text = value; OnPropertyChanged(); } }

        /// <summary> Placeholder text </summary>
        public string Placeholder { get => placeholder; set { placeholder = value; OnPropertyChanged(); } }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        public CustomTextArea()
        {
            DataContext = this;

            InitializeComponent();
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Text changed slot </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            ctbPlaceholder.Visibility = string.IsNullOrEmpty(ctbInput.Text) ? Visibility.Visible : Visibility.Hidden;
        }

        #endregion
    }
}