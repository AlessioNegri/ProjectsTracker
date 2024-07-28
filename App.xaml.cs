using Microsoft.Extensions.DependencyInjection;
using ProjectsTracker.src.MVVM;
using ProjectsTracker.src.Services;
using ProjectsTracker.src.ViewModels;
using System.Windows;

namespace ProjectsTracker
{
    /// <summary> Interaction logic for App.xaml </summary>
    public partial class App : Application
    {
        #region MEMBERS

        /// <summary> Service Provider </summary>
        private readonly ServiceProvider service_provider;

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        public App()
        {
            IServiceCollection services = new ServiceCollection();

            // Set MainWindows data context

            services.AddSingleton<MainWindow>(provider => new MainWindow { DataContext = provider.GetRequiredService<MainWindowViewModel>() });
            services.AddSingleton<MainWindowViewModel>();

            // Add pages

            services.AddSingleton<PageDashboardViewModel>();
            services.AddTransient<PageSolutionViewModel>();

            // Add navigation

            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<Func<Type, ViewModelBase>>(service_provider => view_model_type => (ViewModelBase)service_provider.GetRequiredService(view_model_type));

            // Initialize service provider

            service_provider = services.BuildServiceProvider();
        }

        #endregion

        #region METHODS - PROTECTED

        protected override void OnStartup(StartupEventArgs e)
        {
            service_provider.GetRequiredService<MainWindow>().Show();

            base.OnStartup(e);
        }

        #endregion
    }
}