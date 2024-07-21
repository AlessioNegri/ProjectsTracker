using ProjectsTracker.src.Database;
using ProjectsTracker.src.Models;
using System.Collections.ObjectModel;

namespace ProjectsTracker.src.ViewModels
{
    internal class SolutionsViewModel
    {
        public ObservableCollection<Solution> Solutions { get; set; }

        public SolutionsViewModel()
        {
            Solutions = new ObservableCollection<Solution>();
        }

        public void LoadSolutions()
        {
            Solutions.Clear();

            var t_solutions = new List<ROW_SOLUTION>();

            SolutionsManager.Instance.SelectSolutions(out t_solutions);

            foreach (var row in t_solutions)
            {
                var solution = new Solution();

                solution.Id             = row.SolutionID;
                solution.Name           = row.Name;
                solution.SubProjects    = row.SubProjects;

                Solutions.Add(solution);
            }
        }
    }
}
