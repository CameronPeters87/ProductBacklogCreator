using ProductBacklogForProjects.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductBacklogForProjects.Comparer
{
    public class ProjectEqualityComparer : IEqualityComparer<ProjectModel>
    {
        public bool Equals(ProjectModel project1, ProjectModel project2)
        {
            return project1.Id.Equals(project2.Id);
        }

        public int GetHashCode(ProjectModel project)
        {
            return project.Id;
        }
    }
}