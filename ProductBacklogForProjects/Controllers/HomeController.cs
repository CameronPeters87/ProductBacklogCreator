using Microsoft.AspNet.Identity;
using ProductBacklogForProjects.Entities;
using ProductBacklogForProjects.Extensions;
using ProductBacklogForProjects.Models;
using ProductBacklogForProjects.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProductBacklogForProjects.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public async Task<ActionResult> ProjectsMain()
        {
            var userId = User.Identity.GetUserId();
            var projects = await new List<ProjectModel>().GetProjectsAsync(userId);
            var count = projects.Count() / 4;

            var model = new List<ProjectThumbnailAreaModel>();

            for (int i = 0; i <= count; i++)
            {
                model.Add(new ProjectThumbnailAreaModel
                {
                    Title = i.Equals(0) ? "My Projects" : null,
                    Projects = projects.Skip(i * 3).Take(3)
                });
            }

            return View(model);
        }

        [Authorize]
        public ActionResult CreateProject()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProject([Bind(Include = "Id,Name, Description, UserId")] Project project)
        {
            var db = ApplicationDbContext.Create();

            if (ModelState.IsValid)
            {
                db.Projects.Add(project);

                project.UserId = User.Identity.GetUserId();

                await db.SaveChangesAsync();
                return RedirectToAction("ProjectsMain");
            }

            return View(project);
        }
        [Authorize]
        public async Task<ActionResult> EditProject(int? id)
        {
            var db = ApplicationDbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Admin/Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProject([Bind(Include = "Id, Name,Description, UserId")] Project project)
        {
            var db = ApplicationDbContext.Create();

            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;

                project.UserId = User.Identity.GetUserId();

                await db.SaveChangesAsync();
                return RedirectToAction("ProjectsMain");
            }
            return View(project);
        }

        // GET: Admin/Subjects/Delete/5
        [Authorize]
        public async Task<ActionResult> DeleteProject(int? id)
        {
            var db = ApplicationDbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Admin/Subjects/Delete/5
        [Authorize]
        [HttpPost, ActionName("DeleteProject")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var db = ApplicationDbContext.Create();

            Project project = await db.Projects.FindAsync(id);
            var products = await (from p in db.Products
                                  where p.ProjectId == id
                                  select p).ToListAsync();

            db.Projects.Remove(project);
            db.Products.RemoveRange(products);

            await db.SaveChangesAsync();
            return RedirectToAction("ProjectsMain");
        }

        protected override void Dispose(bool disposing)
        {
            var db = ApplicationDbContext.Create();

            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [Authorize]
        public async Task<ActionResult> ProductBacklog(int projectId)
        {
            if (projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var db = ApplicationDbContext.Create();
            var model = new ProjectProductsViewModel();

            model.ProductModels = await (from p in db.Products
                                         join pro in db.Projects on p.ProjectId equals pro.Id
                                         join s in db.Subjects on p.SubjectId equals s.Id
                                         join st in db.Statuses on p.StatusId equals st.Id
                                         join pr in db.Priorities on p.PriorityId equals pr.Id
                                         where p.ProjectId.Equals(projectId)
                                         select new ProductModel
                                         {
                                             Id = p.Id,
                                             Goal = p.Goal,
                                             Benefit = p.Benefit,
                                             Sprint = p.Sprint,
                                             StatusId = p.StatusId,
                                             ProjectId = p.ProjectId,
                                             PriorityId = p.PriorityId,
                                             SubjectId = p.SubjectId,
                                             SubjectName = s.Name,
                                             PriorityName = pr.Name,
                                             StatusName = st.Name,
                                             ProjectName = pro.Name
                                         }).ToListAsync();

            // For dropdownlists
            model.Subjects = await db.Subjects.ToListAsync();
            model.Priorities = await db.Priorities.ToListAsync();
            model.Statuses = await db.Statuses.ToListAsync();
            model.ProjectId = projectId;

            //model.ProjectName = await (from p in db.Projects
            //                           where p.Id.Equals(projectId)
            //                           select p.Name).FirstOrDefaultAsync();
            ViewBag.ProjectName = "Hello";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> ProductBacklog(ProjectProductsViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (ModelState.IsValid)
                {
                    var db = new ApplicationDbContext();
                    db.Products.Add(new Product
                    {
                        Id = model.Id,
                        PriorityId = model.PriorityId,
                        ProjectId = model.ProjectId,
                        SubjectId = model.SubjectId,
                        StatusId = model.StatusId,
                        Benefit = model.Benefit,
                        Goal = model.Goal,
                        Sprint = model.Sprint,
                    });
                    await db.SaveChangesAsync();
                }
            }
            catch
            {

            }
            return RedirectToAction("ProductBacklog", "Home",
                new { projectId = model.ProjectId });
        }

        public async Task<ActionResult> RemoveProduct(int projectId, int productId)
        {
            try
            {
                if (projectId == null || productId <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (ModelState.IsValid)
                {
                    var db = new ApplicationDbContext();
                    var prods = db.Products.Where(p => p.Id.Equals(productId) &&
                        p.ProjectId.Equals(projectId));
                    db.Products.RemoveRange(prods);
                    await db.SaveChangesAsync();
                }
            }
            catch
            {

            }
            return RedirectToAction("ProductBacklog", "Home",
                new { projectId = projectId });
        }
        //[Authorize]
        //public async Task<ActionResult> EditProduct(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var db = ApplicationDbContext.Create();
        //    Product product = await db.Products.FindAsync(id);
        //    //Product product = db.Products.Where(p => p.Id.Equals(productId) &&
        //    //    p.ProjectId.Equals(projectId)).First();



        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var prod = new List<Product>();
        //    prod.Add(product);
        //    var ProductModel = await prod.Convert(db);

        //    // For dropdownlists

        //    return View(ProductModel.First());
        //    //return View(product);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]

    }
}