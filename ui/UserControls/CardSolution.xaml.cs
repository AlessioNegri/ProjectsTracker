using ProjectsTracker.src.Database;
using ProjectsTracker.src;
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

        /// <summary> Invokes the propery change event </summary>
        /// <param name="propertyName"> Name of the property </param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region EVENTS

        /// <summary> Event triggered to notify the UI update </summary>
        public event EventHandler? Update = null;

        #endregion

        #region CONST

        /// <summary> Null solution unique identifier </summary>
        public const int NullSolutionId = 0;

        #endregion

        #region MEMBERS

        private int solution_id = NullSolutionId;

        private string solution_name = string.Empty;

        private string sub_projects = string.Empty;

        #endregion

        #region BINDINGS

        /// <summary> Solution Id </summary>
        public int SolutionId { get => solution_id; set { solution_id = value; OnPropertyChanged(); } }

        /// <summary> Solution Name </summary>
        public string SolutionName { get => solution_name; set { solution_name = value; OnPropertyChanged(); } }

        /// <summary> Sub Projects text </summary>
        public string SubProjects { get => sub_projects; set { sub_projects = value; OnPropertyChanged(); } }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        public CardSolution()
        {
            DataContext = this;

            InitializeComponent();
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Opens the Edit Solution Dialog </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void EditSolution(object sender, RoutedEventArgs e)
        {
            ROW_SOLUTION row = new ROW_SOLUTION();

            bool ok = SolutionsManager.Instance.SelectSolutionById(SolutionId, out row);

            if (!ok) return;

            DialogSolution dlg = new DialogSolution(Application.Current.MainWindow);

            dlg.Edit            = true;
            dlg.SolutionHeader  = $"Edit {row.Name}";
            dlg.SolutionId      = row.SolutionID;
            dlg.SolutionName    = row.Name;

            dlg.ShowDialog();

            if (dlg.Success && Update != null) Update(this, new EventArgs());
        }

        /// <summary> Opens the Delete Solution Dialog </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void DeleteSolution(object sender, RoutedEventArgs e)
        {
            ROW_SOLUTION row = new ROW_SOLUTION();

            bool ok = SolutionsManager.Instance.SelectSolutionById(SolutionId, out row);

            if (!ok) return;

            DialogDelete dlg = new DialogDelete(Application.Current.MainWindow);

            dlg.DeleteHeader    = "Delete Solution";
            dlg.DeleteContent   = $"Do you want to delete {row.Name}?";
            dlg.ConfirmCommand  = new RelayCommand(execute => { SolutionsManager.Instance.DeleteSolution(SolutionId); });

            dlg.ShowDialog();

            if (dlg.Success && Update != null) Update(this, new EventArgs());
        }

        /// <summary> Drag Drop slot </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void CardDrop(object sender, DragEventArgs e)
        {
            int project_id = (int)e.Data.GetData(DataFormats.Serializable);

            ProjectsManager.Instance.MoveProjectInSolution(project_id, SolutionId);

            if (Update != null) Update(this, new EventArgs());
        }

        #endregion
    }
}
