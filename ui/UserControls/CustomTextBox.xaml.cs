﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ProjectsTracker.ui.UserControls
{
    /// <summary> Logica di interazione per CustomTextBox.xaml </summary>
    public partial class CustomTextBox : UserControl, INotifyPropertyChanged
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

        private string regex = "[\\s\\S]*";

        #endregion

        #region BINDINGS

        /// <summary> Text </summary>
        public string Text { get => text; set { text = value; OnPropertyChanged(); } }

        /// <summary> Placeholder text </summary>
        public string Placeholder { get => placeholder; set { placeholder = value; OnPropertyChanged(); } }

        /// <summary> Regex </summary>
        public string Regex { get => regex; set { regex = value; OnPropertyChanged(); } }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        public CustomTextBox()
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

        /// <summary> Regex validation </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Validation(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            /*
            Regex re = new Regex(Regex);

            e.Handled = re.IsMatch(e.Text);

            if (!e.Handled && !String.IsNullOrEmpty(ctbInput.Text))
            {
                ctbInput.Text = e.Text.Remove(e.Text.Length - 1, 1);
            }
            */
        }

        #endregion
    }
}