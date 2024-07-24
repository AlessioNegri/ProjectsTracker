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
        public MainWindow() => InitializeComponent();

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Drag event </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Drag(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();

        /// <summary> Minimize event </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimize(object sender, System.Windows.Input.MouseButtonEventArgs e) => WindowState = WindowState.Minimized;

        /// <summary> Maximize event </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Logger.Instance.Close();
            
            Application.Current.Shutdown();
        }

        #endregion
    }
}