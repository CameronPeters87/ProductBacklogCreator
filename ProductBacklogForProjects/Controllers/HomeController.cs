using Microsoft.AspNet.Identity;
using NHibernate.Criterion;
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
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else
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
        public async Task<ActionResult> EditProject(int projectId)
        {
            var db = ApplicationDbContext.Create();

            if (projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(projectId);
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
        public async Task<ActionResult> DeleteProject(int projectId)
        {
            var db = ApplicationDbContext.Create();

            if (projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(projectId);
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
        public async Task<ActionResult> DeleteConfirmed(int projectId)
        {
            var db = ApplicationDbContext.Create();

            Project project = await db.Projects.FindAsync(projectId);
            var products = await (from p in db.Products
                                  where p.ProjectId == projectId
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
    }
}