using ProjectsTracker.src.MVVM;
using ProjectsTracker.src.Utility;
using ProjectsTracker.src.ViewModels;
using System.Windows;

namespace ProjectsTracker
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class MainWindow : Window
    {
        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        public MainWindow()
        {
            InitializeComponent();

            _back_.Visibility = Visibility.Hidden;
            _home_.Visibility = Visibility.Hidden;

            DataContextChanged += MainWindow_DataContextChanged;
        }

        #endregion

        #region METHODS - PRIVATE

        private void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                ((MainWindowViewModel)DataContext).BackIconVisibilityChanged += MainWindow_BackIconVisibilityChanged;
                ((MainWindowViewModel)DataContext).HomeIconVisibilityChanged += MainWindow_HomeIconVisibilityChanged;
            }
        }

        /// <summary> Manage the Back Icon visibility changed </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void MainWindow_BackIconVisibilityChanged(object? sender, EventArgs e)
        {
            _back_.Visibility = Globals.Instance.BackIconVisibility;
        }

        /// <summary> Manage the Home Icon visibility changed </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void MainWindow_HomeIconVisibilityChanged(object? sender, EventArgs e)
        {
            _home_.Visibility = Globals.Instance.HomeIconVisibility;
        }

        /// <summary> Drag event </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Drag(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();

        /// <summary> Minimize event </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Minimize(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary> Maximize event </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Maximize(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;

                ((MainWindowViewModel)DataContext).MaximizeIcon = "/icons/maximize.svg";
            }
            else
            {
                WindowState = WindowState.Maximized;

                ((MainWindowViewModel)DataContext).MaximizeIcon = "/icons/restore.svg";
            }
        }

        /// <summary> Close event </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Close(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Logger.Instance.Close();
            
            Application.Current.Shutdown();
        }

        /// <summary> Home icon clicked event </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void HomeIconClicked(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).NavigateToDashboard.Execute(null);

            Globals.Instance.BackIconVisibility = Visibility.Hidden;
            Globals.Instance.HomeIconVisibility = Visibility.Hidden;
        }

        #endregion
    }
}