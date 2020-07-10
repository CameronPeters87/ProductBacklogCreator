using ProductBacklogForProjects.Comparer;
using ProductBacklogForProjects.Models;
using ProductBacklogForProjects.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProductBacklogForProjects.Extensions
{
    public static class ThumbnailExtension
    {
        public static async Task<IEnumerable<ProjectModel>> GetProjectsAsync (
            this List<ProjectModel> projects, string userId)
        {
            try
            {
                var db = ApplicationDbContext.Create();
                projects = await (from p in db.Projects
                                  where p.UserId.Equals(userId)
                                  select new ProjectModel
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      Description = p.Description,
                                      UserId = p.UserId,
                                      // Link = "/Home/ProductBacklog?projectid=" + p.Id
                                      Link = "/Products/ProductBacklog/" + p.Id
                                  }).ToListAsync();
            }
            catch { }

            return projects.Distinct(new ProjectEqualityComparer()).OrderBy(o => o.Name);
        }
    }
}