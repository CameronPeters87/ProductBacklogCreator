using ProductBacklogForProjects.Models.ViewModels;
using ProductBacklogForProjects.Entities;
using ProductBacklogForProjects.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductBacklogForProjects.Extensions
{
    public static class ConversionExtension
    {
        public static async Task<IEnumerable<ProductModel>> Convert(
            this IEnumerable<Product> products, ApplicationDbContext db)
        {
            if (products == null) return new List<ProductModel>();

            var projects = await db.Projects.ToListAsync();
            var statuses = await db.Statuses.ToListAsync();
            var subjects = await db.Subjects.ToListAsync();
            var priorities = await db.Priorities.ToListAsync();

            return (from p in products
                    join pro in db.Projects on p.ProjectId equals pro.Id
                    join s in db.Subjects on p.SubjectId equals s.Id
                    join st in db.Statuses on p.StatusId equals st.Id
                    join pr in db.Priorities on p.PriorityId equals pr.Id
                    select new ProductModel
                    {
                        Id = p.Id,
                        Subjects = subjects,
                        Goal = p.Goal,
                        Benefit = p.Benefit,
                        Priorities = priorities,
                        Sprint = p.Sprint,
                        Projects = projects,
                        Statuses = statuses,
                        StatusId = p.StatusId,
                        ProjectId = p.ProjectId,
                        PriorityId = p.PriorityId,
                        SubjectId = p.SubjectId,
                        SubjectName = s.Name,
                        PriorityName = pr.Name,
                        StatusName = st.Name,
                        ProjectName = pro.Name
                    }).OrderBy(o => o.Sprint);
        }

        public static async Task<IEnumerable<ProductExcelView>> ConvertExcel(
            this IEnumerable<Product> products, ApplicationDbContext db)
        {
            if (products == null) return new List<ProductExcelView>();

            var projects = await db.Projects.ToListAsync();
            var statuses = await db.Statuses.ToListAsync();
            var subjects = await db.Subjects.ToListAsync();
            var priorities = await db.Priorities.ToListAsync();

            return (from p in products
                    join pro in db.Projects on p.ProjectId equals pro.Id
                    join s in db.Subjects on p.SubjectId equals s.Id
                    join st in db.Statuses on p.StatusId equals st.Id
                    join pr in db.Priorities on p.PriorityId equals pr.Id
                    select new ProductExcelView
                    {
                        Goal = p.Goal,
                        Benefit = p.Benefit,
                        Sprint = p.Sprint,
                        Subject = s.Name,
                        Priority = pr.Name,
                        Status = st.Name,
                    }).OrderBy(o => o.Sprint);
        }

    }
}
