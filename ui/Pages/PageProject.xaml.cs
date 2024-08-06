using ProjectsTracker.src;
using ProjectsTracker.src.Database;
using ProjectsTracker.src.MVVM;
using ProjectsTracker.src.ViewModels;
using ProjectsTracker.ui.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace ProjectsTracker.ui.Pages
{
    /// <summary> Logica di interazione per PageProject.xaml </summary>
    public partial class PageProject : UserControl
    {
        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        public PageProject()
        {
            InitializeComponent();

            DataContextChanged += PageProject_DataContextChanged;
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Slot called when DataContextChanged event is invoked </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void PageProject_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
            }
        }

        /// <summary> Opens the Add ECR Dialog </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void AddECRDialog(object sender, RoutedEventArgs e)
        {
            DialogECR dlg = new DialogECR(Application.Current.MainWindow);

            dlg.ProjectId   = ((PageProjectViewModel)DataContext).ProjectId;
            dlg.Header      = "Add Engineering Change Request";
            dlg.Importance  = dlg.Importance1;
            dlg.Status      = dlg.StatusAssigned;

            dlg.ShowDialog();

            if (dlg.Success)
            {
                ((PageProjectViewModel)DataContext).LoadProjectRows();
            }
        }

        /// <summary> Opens the Add PR Dialog </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void AddPRDialog(object sender, RoutedEventArgs e)
        {
            DialogPR dlg = new DialogPR(Application.Current.MainWindow);

            dlg.ProjectId   = ((PageProjectViewModel)DataContext).ProjectId;
            dlg.Header      = "Add Problem Report";
            dlg.Priority    = dlg.Priority1;
            dlg.Status      = dlg.StatusAssigned;

            dlg.ShowDialog();

            if (dlg.Success)
            {
                ((PageProjectViewModel)DataContext).LoadProjectRows();
            }
        }

        /// <summary> Opens the Add RELEASE Dialog </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void AddRELEASEDialog(object sender, RoutedEventArgs e)
        {
            DialogRELEASE dlg = new DialogRELEASE(Application.Current.MainWindow);

            dlg.ProjectId   = ((PageProjectViewModel)DataContext).ProjectId;
            dlg.Header      = "Add Release";
            dlg.Status      = dlg.StatusAssigned;

            dlg.ShowDialog();

            if (dlg.Success)
            {
                ((PageProjectViewModel)DataContext).LoadProjectRows();
            }
        }

        /// <summary> Opens the Add PATCH Dialog </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void AddPATCHDialog(object sender, RoutedEventArgs e)
        {
            DialogPATCH dlg = new DialogPATCH(Application.Current.MainWindow);

            dlg.ProjectId   = ((PageProjectViewModel)DataContext).ProjectId;
            dlg.Header      = "Add Patch";
            dlg.Status      = dlg.StatusAssigned;

            dlg.ShowDialog();

            if (dlg.Success)
            {
                ((PageProjectViewModel)DataContext).LoadProjectRows();
            }
        }

        /// <summary> Edits the selected row </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void EditRow(object sender, RoutedEventArgs e)
        {
            var index = _table_.SelectedIndex;

            var row = ((PageProjectViewModel)DataContext).ProjectRows[index];

            if (row.Type == ((int)RowType.ECR))
            {
                DialogECR dlg = new DialogECR(Application.Current.MainWindow);

                dlg.Edit            = true;
                dlg.ProjectId       = ((PageProjectViewModel)DataContext).ProjectId;
                dlg.Id              = row.RowId;
                dlg.Header          = "Edit " + row.Id;
                dlg.CreationDate    = row.CreationDate;
                dlg.ClosureDate     = row.ClosureDate;
                dlg.Version         = row.Version;
                dlg.PatchVersion    = row.PatchVersion;
                dlg.ECRTitle        = row.Title;
                dlg.Description     = row.Description;

                switch (row.Priority)
                {
                    case 1: dlg.Importance = dlg.Importance1; break;
                    case 2: dlg.Importance = dlg.Importance2; break;
                    case 3: dlg.Importance = dlg.Importance3; break;
                    case 4: dlg.Importance = dlg.Importance4; break;
                    case 5: dlg.Importance = dlg.Importance5; break;

                    default: break;
                }

                switch (row.Status)
                {
                    case 0: dlg.Status = dlg.StatusAssigned; break;
                    case 1: dlg.Status = dlg.StatusInProgress; break;
                    case 2: dlg.Status = dlg.StatusDone; break;

                    default: break;
                }

                dlg.ShowDialog();

                if (dlg.Success)
                {
                    ((PageProjectViewModel)DataContext).LoadProjectRows();
                }
            }
            else if (row.Type == ((int)RowType.PR))
            {
                DialogPR dlg = new DialogPR(Application.Current.MainWindow);

                dlg.Edit                = true;
                dlg.ProjectId           = ((PageProjectViewModel)DataContext).ProjectId;
                dlg.Id                  = row.RowId;
                dlg.Header              = "Edit " + row.Id;
                dlg.CreationDate        = row.CreationDate;
                dlg.ClosureDate         = row.ClosureDate;
                dlg.Version             = row.Version;
                dlg.PatchVersion        = row.PatchVersion;
                dlg.DiscoveryVersion    = row.ReferenceVersion;
                dlg.PRTitle             = row.Title;
                dlg.Description         = row.Description;
                dlg.Note                = row.Note;

                switch (row.Priority)
                {
                    case 1: dlg.Priority = dlg.Priority1; break;
                    case 2: dlg.Priority = dlg.Priority2; break;
                    case 3: dlg.Priority = dlg.Priority3; break;
                    case 4: dlg.Priority = dlg.Priority4; break;
                    case 5: dlg.Priority = dlg.Priority5; break;

                    default: break;
                }

                switch (row.Status)
                {
                    case 0: dlg.Status = dlg.StatusAssigned; break;
                    case 1: dlg.Status = dlg.StatusInProgress; break;
                    case 2: dlg.Status = dlg.StatusDone; break;

                    default: break;
                }

                dlg.ShowDialog();

                if (dlg.Success)
                {
                    ((PageProjectViewModel)DataContext).LoadProjectRows();
                }
            }
            else if (row.Type == ((int)RowType.RELEASE))
            {
                DialogRELEASE dlg = new DialogRELEASE(Application.Current.MainWindow);

                dlg.Edit            = true;
                dlg.ProjectId       = ((PageProjectViewModel)DataContext).ProjectId;
                dlg.Id              = row.RowId;
                dlg.Header          = "Edit " + row.Id;
                dlg.CreationDate    = row.CreationDate;
                dlg.ClosureDate     = row.ClosureDate;
                dlg.Version         = row.Version;

                switch (row.Status)
                {
                    case 0: dlg.Status = dlg.StatusAssigned; break;
                    case 1: dlg.Status = dlg.StatusInProgress; break;
                    case 2: dlg.Status = dlg.StatusDone; break;

                    default: break;
                }

                dlg.ShowDialog();

                if (dlg.Success)
                {
                    ((PageProjectViewModel)DataContext).LoadProjectRows();
                }
            }
            else if (row.Type == ((int)RowType.PATCH))
            {
                DialogPATCH dlg = new DialogPATCH(Application.Current.MainWindow);

                dlg.Edit            = true;
                dlg.ProjectId       = ((PageProjectViewModel)DataContext).ProjectId;
                dlg.Id              = row.RowId;
                dlg.Header          = "Edit " + row.Id;
                dlg.CreationDate    = row.CreationDate;
                dlg.ClosureDate     = row.ClosureDate;
                dlg.Version         = row.Version;
                dlg.PatchVersion    = row.PatchVersion;

                switch (row.Status)
                {
                    case 0: dlg.Status = dlg.StatusAssigned; break;
                    case 1: dlg.Status = dlg.StatusInProgress; break;
                    case 2: dlg.Status = dlg.StatusDone; break;

                    default: break;
                }

                dlg.ShowDialog();

                if (dlg.Success)
                {
                    ((PageProjectViewModel)DataContext).LoadProjectRows();
                }
            }
        }

        /// <summary> Deletes the selected row </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void DeleteRow(object sender, RoutedEventArgs e)
        {
            var index = _table_.SelectedIndex;

            var row = ((PageProjectViewModel)DataContext).ProjectRows[index];

            ROW_TABLE item = new ROW_TABLE();

            if (!TablesManager.Instance.SelectRowById(((PageProjectViewModel)DataContext).ProjectId, row.RowId, out item)) return;

            DialogDelete dlg = new DialogDelete(Application.Current.MainWindow);

            if (row.Type == ((int)RowType.ECR))
            {
                dlg.DeleteHeader    = "Delete Engineering Change Request";
                dlg.DeleteContent   = $"Do you want to delete Enigneering Change Request - {item.Number}?";
            }
            else if (row.Type == ((int)RowType.PR))
            {
                dlg.DeleteHeader = "Delete Problem Report";
                dlg.DeleteContent = $"Do you want to delete Problem Report - {item.Number}?";
            }
            else if (row.Type == ((int)RowType.RELEASE))
            {
                dlg.DeleteHeader = "Delete Release";
                dlg.DeleteContent = $"Do you want to delete Release - {item.Version}?";
            }
            else if (row.Type == ((int)RowType.PATCH))
            {
                dlg.DeleteHeader = "Delete Patch";
                dlg.DeleteContent = $"Do you want to delete Patch - {item.PatchVersion}?";
            }

            dlg.ConfirmCommand = new RelayCommand(execute => { TablesManager.Instance.DeleteRow(((PageProjectViewModel)DataContext).ProjectId, item.ID); });

            dlg.ShowDialog();

            if (dlg.Success)
            {
                ((PageProjectViewModel)DataContext).LoadProjectRows();
            }
        }

        #endregion
    }
}