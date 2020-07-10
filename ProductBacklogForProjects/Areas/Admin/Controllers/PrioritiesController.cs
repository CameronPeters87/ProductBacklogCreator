using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProductBacklogForProjects.Entities;
using ProductBacklogForProjects.Models;

namespace ProductBacklogForProjects.Areas.Admin.Controllers
{
    public class PrioritiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Priorities
        public async Task<ActionResult> Index()
        {
            return View(await db.Priorities.ToListAsync());
        }

        // GET: Admin/Priorities/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Priority priority = await db.Priorities.FindAsync(id);
            if (priority == null)
            {
                return HttpNotFound();
            }
            return View(priority);
        }

        // GET: Admin/Priorities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Priorities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Priority priority)
        {
            if (ModelState.IsValid)
            {
                db.Priorities.Add(priority);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(priority);
        }

        // GET: Admin/Priorities/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Priority priority = await db.Priorities.FindAsync(id);
            if (priority == null)
            {
                return HttpNotFound();
            }
            return View(priority);
        }

        // POST: Admin/Priorities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Priority priority)
        {
            if (ModelState.IsValid)
            {
                db.Entry(priority).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(priority);
        }

        // GET: Admin/Priorities/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Priority priority = await db.Priorities.FindAsync(id);
            if (priority == null)
            {
                return HttpNotFound();
            }
            return View(priority);
        }

        // POST: Admin/Priorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Priority priority = await db.Priorities.FindAsync(id);
            db.Priorities.Remove(priority);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
