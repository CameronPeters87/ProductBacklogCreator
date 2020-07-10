using ProductBacklogForProjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductBacklogForProjects.Models.ViewModels
{
    public class ProjectThumbnailAreaModel
    {
        public string Title { get; set; }
        public IEnumerable<ProjectModel> Projects { get; set; }
    }
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string UserId { get; set; }
    }
}