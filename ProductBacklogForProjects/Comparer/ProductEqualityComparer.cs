using ProductBacklogForProjects.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductBacklogForProjects.Comparer
{
    public class ProductEqualityComparer : IEqualityComparer<ProductModel>
    {
        public bool Equals(ProductModel product1, ProductModel product2)
        {
            return product1.Id.Equals(product2.Id);
        }

        public int GetHashCode(ProductModel product)
        {
            return product.Id;
        }
    }
}