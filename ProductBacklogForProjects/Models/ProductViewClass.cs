using ProductBacklogForProjects.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductBacklogForProjects.Models
{
    public class ProductViewClass
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<ProductModel> Products { get; set; }
    }
}