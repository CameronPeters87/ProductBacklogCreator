using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductBacklogForProjects.Models
{
    public class ProductExcelModel
    {
        public IEnumerable<ProductExcelView> ProductExcels { get; set; }
    }
}