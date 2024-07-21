using ProjectsTracker.src.Database;
using ProjectsTracker.src;
using ProjectsTracker.src.Models;
using ProjectsTracker.ui.Dialogs;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using ProjectsTracker.src.MVVM;

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

        #region EVENTS

        public event EventHandler Update;

        #endregion

        #region MEMBERS

        private Window? parent_window = null;

        private int solution_id = Solution.NullSolutionId;

        private string solution_name = String.Empty;

        private string sub_projects = String.Empty;

        #endregion

        #region BINDINGS

        public int SolutionId { get => solution_id; set { solution_id = value; OnPropertyChanged(); } }

        public string SolutionName { get => solution_name; set { solution_name = value; OnPropertyChanged(); } }

        public string SubProjects { get => sub_projects; set { sub_projects = value; OnPropertyChanged(); } }

        #endregion

        #region METHODS - PUBLIC

        public CardSolution(Window? parent = null)
        {
            parent_window = parent;

            DataContext = this;

            InitializeComponent();
        }

        #endregion

        #region METHODS - PRIVATE

        private void EditSolution(object sender, RoutedEventArgs e)
        {
            ROW_SOLUTION row = new ROW_SOLUTION();

            bool ok = SolutionsManager.Instance.SelectSolutionById(SolutionId, out row);

            if (!ok) return;

            DialogSolution dlg = new DialogSolution(parent_window);

            dlg.Edit            = true;
            dlg.SolutionHeader  = $"Edit {row.Name}";
            dlg.SolutionId      = row.SolutionID;
            dlg.SolutionName    = row.Name;

            dlg.ShowDialog();

            if (dlg.Success && Update != null) Update(this, new EventArgs());
        }

        private void DeleteSolution(object sender, RoutedEventArgs e)
        {
            ROW_SOLUTION row = new ROW_SOLUTION();

            bool ok = SolutionsManager.Instance.SelectSolutionById(SolutionId, out row);

            if (!ok) return;

            DialogDelete dlg = new DialogDelete(parent_window);

            dlg.DeleteHeader    = "Delete Solution";
            dlg.DeleteContent   = $"Do you want to delete {row.Name}?";
            dlg.ConfirmCommand  = new RelayCommand(execute => { SolutionsManager.Instance.DeleteSolution(SolutionId); });

            dlg.ShowDialog();

            if (dlg.Success && Update != null) Update(this, new EventArgs());
        }

        private void CardDrop(object sender, DragEventArgs e)
        {
            int project_id = (int)e.Data.GetData(DataFormats.Serializable);

            ProjectsManager.Instance.MoveProjectInSolution(project_id, SolutionId);

            Update(this, new EventArgs());
        }

        #endregion
    }
}
