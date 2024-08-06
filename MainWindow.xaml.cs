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

            _window_title_.Text = "PROJECTS TRACKER";
            _home_.Visibility   = Visibility.Hidden;
            _back_.Visibility   = Visibility.Hidden;

            DataContextChanged += MainWindow_DataContextChanged;
        }

        #endregion

        #region METHODS - PRIVATE

        private void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                ((MainWindowViewModel)DataContext).WindowTitleChanged           += MainWindow_WindowTitleChanged;
                ((MainWindowViewModel)DataContext).HomeIconVisibilityChanged    += MainWindow_HomeIconVisibilityChanged;
                ((MainWindowViewModel)DataContext).BackIconVisibilityChanged    += MainWindow_BackIconVisibilityChanged;
            }
        }

        /// <summary> Manage the Back Icon visibility changed </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void MainWindow_WindowTitleChanged(object? sender, EventArgs e)
        {
            _window_title_.Text = Globals.Instance.WindowTitle;
        }

        /// <summary> Manage the Home Icon visibility changed </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void MainWindow_HomeIconVisibilityChanged(object? sender, EventArgs e)
        {
            _home_.Visibility = Globals.Instance.HomeIconVisibility;
        }

        /// <summary> Manage the Back Icon visibility changed </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void MainWindow_BackIconVisibilityChanged(object? sender, EventArgs e)
        {
            _back_.Visibility = Globals.Instance.BackIconVisibility;
        }

        /// <summary> Drag event </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void Drag(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
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
            else
            {
                DragMove();
            }
        }

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

            Globals.Instance.WindowTitle        = "PROJECTS TRACKER";
            Globals.Instance.HomeIconVisibility = Visibility.Hidden;
            Globals.Instance.BackIconVisibility = Visibility.Hidden;
        }

        /// <summary> Back icon clicked event </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Event arguments </param>
        private void HomeBackClicked(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).NavigateToSolution();

            Globals.Instance.HomeIconVisibility = Visibility.Visible;
            Globals.Instance.BackIconVisibility = Visibility.Hidden;
        }

        #endregion
    }
}