using ProjectsTracker.src.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ProjectsTracker.ui.UserControls
{
    /// <summary>
    /// Logica di interazione per CardSolution.xaml
    /// </summary>
    public partial class CardSolution : UserControl, INotifyPropertyChanged
    {
        #region INTERFACE

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region MEMBERS

        private Window? parent_window = null;

        private int solution_id = Solution.NullSolutionId;

        private string solution_name = string.Empty;

        #endregion

        #region BINDINGS

        public int SolutionId { get => solution_id; set { solution_id = value; OnPropertyChanged(); } }

        public string SolutionName { get => solution_name; set { solution_name = value; OnPropertyChanged(); } }

        #endregion


        public CardSolution(Window? parent = null)
        {
            parent_window = parent;

            DataContext = this;

            InitializeComponent();
        }

        private void EditSolution(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSolution(object sender, RoutedEventArgs e)
        {

        }
    }
}
