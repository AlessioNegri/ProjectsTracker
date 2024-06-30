using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Numerics;

namespace ProjectsTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Dashboard containing all the projects and solutions
        /// </summary>
        private src.Dashboard dashboard;

        public MainWindow()
        {
            InitializeComponent();

            dashboard = new src.Dashboard();
        }
    }
}