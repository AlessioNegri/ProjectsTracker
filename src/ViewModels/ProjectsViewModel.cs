using ProjectsTracker.src.Database;
using ProjectsTracker.src.Models;
using System.Collections.ObjectModel;

namespace ProjectsTracker.src.ViewModels
{
    internal class ProjectsViewModel
    {
        public ObservableCollection<Project> Projects { get; set; }

        public ProjectsViewModel()
        {
            Projects = new ObservableCollection<Project>();
        }

        public void LoadProjects()
        {
            Projects.Clear();

            var t_projects = new List<ROW_PROJECT>();

            ProjectsManager.Instance.SelectProjects(out t_projects);

            foreach (var row in t_projects)
            {
                var project = new Project();

                project.Id          = row.ProjectID;
                project.Name        = row.Name;
                project.SolutionId  = row.SolutionID;

                Projects.Add(project);
            }
        }
    }
}
