using Newtonsoft.Json;
using ProductBacklogForProjects.Areas.Admin.Model;
using ProductBacklogForProjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ProductBacklogForProjects.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {

        // GET: Admin/Dashboard
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Chart()
        {
            var db = ApplicationDbContext.Create();

            return View();
        }

        public ActionResult CreateChart()
        {
            var db = ApplicationDbContext.Create();
            var yValues = db.Users.Count().ToString();

            var chart = new Chart(width: 480, height: 280, theme: ChartTheme.Blue)
                .AddTitle("Number of users registered")
                .AddSeries(
                    chartType: "column",
                    name: "Series name",
                    xValue: new[] { "Users"},
                    yValues: new[] { yValues }
                ).GetBytes("png");

            return File(chart, "image/bytes");
        }
    }
}