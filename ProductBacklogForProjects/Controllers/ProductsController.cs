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
using ProductBacklogForProjects.Extensions;
using System.Security.Cryptography;
using ProductBacklogForProjects.Models.ViewModels;
using System.Web.UI.WebControls;
using ProductBacklogForProjects.Helper;
using System.IO;
using NPOI.SS.Formula.Functions;
using FluentNHibernate.Automapping;
using AutoMapper;

namespace ProductBacklogForProjects.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }

        [Authorize]
        public async Task<ActionResult> ProductBacklog(int id)
        {
            /*
             * display list products where product.ProjectId = userSelectedProjectId
             */
            var productModel = new ProductViewClass();
            var products = await (from p in db.Products
                                  where p.ProjectId == id
                                  select p).ToListAsync();

            productModel.Products = await products.Convert(db);
            productModel.ProjectId = id;
            productModel.ProjectName = (from p in db.Projects
                                        where p.Id.Equals(id)
                                        select p.Name).FirstOrDefault();

            return View(productModel);
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> AddProductUser(int projectId)
        {
            var model = new ProductCreateViewModel
            {
                ProjectId = projectId,
                Statuses = await db.Statuses.ToListAsync(),
                Priorities = await db.Priorities.ToListAsync(),
                Subjects = await db.Subjects.Where(s => s.ProjectId.Equals(projectId))
                    .ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProduct(ProductCreateViewModel model)
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

            return Redirect("/Products/ProductBacklog/" + model.ProjectId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(ProductCreateViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (ModelState.IsValid)
                {
                    db.Subjects.Add(new Subject
                    {
                        Name = model.SubjectName,
                        ProjectId = model.ProjectId
                    });
                    await db.SaveChangesAsync();
                }
            }
            catch { }
            return Redirect("/Products/AddProductUser?projectId=" + model.ProjectId);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> EditProductUser(int projectId, int productId)
        {
            List<Product> products = await db.Products.Where(p => p.ProjectId.Equals(projectId)
                && p.Id.Equals(productId)).ToListAsync();

            var statuses = await db.Statuses.ToListAsync();
            var priorities = await db.Priorities.ToListAsync();
            var subjects = await db.Subjects.Where(s => s.ProjectId.Equals(projectId))
                .ToListAsync();

            var productModel = (from p in products
                                      join pro in db.Projects on p.ProjectId equals pro.Id
                                      join s in db.Subjects on p.SubjectId equals s.Id
                                      join st in db.Statuses on p.StatusId equals st.Id
                                      join pr in db.Priorities on p.PriorityId equals pr.Id
                                      select new ProductCreateViewModel
                                      {
                                          ProductId = productId,
                                          Subjects = subjects,
                                          Goal = p.Goal,
                                          Benefit = p.Benefit,
                                          Priorities = priorities,
                                          Sprint = p.Sprint,
                                          Statuses = statuses,
                                          StatusId = p.StatusId,
                                          ProjectId = projectId,
                                          PriorityId = p.PriorityId,
                                          SubjectId = p.SubjectId,
                                          SubjectName = s.Name
                                      }).FirstOrDefault();
            return View(productModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProduct(ProductCreateViewModel model)
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
                    Product product = await db.Products.Where(
                        p => p.ProjectId.Equals(model.ProjectId)
                            && p.Id.Equals(model.ProductId)).FirstOrDefaultAsync();

                    product.Id = model.ProductId;
                    product.PriorityId = model.PriorityId;
                    product.ProjectId = model.ProjectId;
                    product.SubjectId = model.SubjectId;
                    product.StatusId = model.StatusId;
                    product.Benefit = model.Benefit;
                    product.Goal = model.Goal;
                    product.Sprint = model.Sprint;

                    db.Entry(product).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                }
            }
            catch { }


            return Redirect("/Products/ProductBacklog/" + model.ProjectId);
        }

        public async Task<ActionResult> RemoveUser(ProductCreateViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (ModelState.IsValid)
                {
                    var subject = await db.Subjects.Where(s => s.Id.Equals(model.SubjectId))
                        .FirstOrDefaultAsync();
                    db.Subjects.Remove(subject);
                    await db.SaveChangesAsync();
                }
            }
            catch { }
            return Redirect("/Products/AddProductUser?projectId=" + model.ProjectId);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SubjectId,Goal,Benefit,PriorityId,Sprint,StatusId,ProjectId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;

                //product.ProjectId = _tempProjectId;

                await db.SaveChangesAsync();
                return RedirectToAction("ProductBacklog/");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var model = new ProductViewClass();
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("ProductBacklog/");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public async Task<ActionResult> DeleteProduct(int projectId, int productId)
        {
            try
            {
                if (projectId == null || productId <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var product = db.Products.Where(p => p.ProjectId.Equals(projectId) &&
                p.Id.Equals(productId)).FirstOrDefault();

                db.Products.Remove(product);
                await db.SaveChangesAsync();
            }
            catch { }

            return Redirect("/Products/ProductBacklog/" + projectId);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadXls(int projectId)
        {
            var excelHelper = new ExcelHelper();

            var productExcelModel = new ProductExcelModel();
            var products = await (from p in db.Products
                                  where p.ProjectId == projectId
                                  select p).ToListAsync();

            productExcelModel.ProductExcels = await products.ConvertExcel(db);

            try
            {
                //Only export 3 columns
               var properties = excelHelper.GetProperties(typeof(ProductExcelView),
                   new string[] { "Subject", "Goal", "Benefit", "Priority",
                        "Sprint", "Status" });

                //            var properties = excelHelper.GetProperties(typeof(Product),
                //new string[] { "Id", "SubjectId", "Goal", "Benefit", "PriorityId",
                //                    "Sprint", "StatusId", "ProjectId" });


                var workbook = excelHelper.CreateXls<ProductExcelView>
                    (productExcelModel.ProductExcels, properties);

                var memoryStream = new MemoryStream();

                workbook.Write(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.ms-excel", "ProductBacklog.xls");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Oops! Something went wrong.";

                return RedirectToAction("Error");
            }
        }
    }
}
