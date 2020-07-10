using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductBacklogForProjects.Extensions
{
    public static class ICollectionExtension
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>
            (this ICollection<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id")
                                    .Equals(selectedValue.ToString())
                   };
        }
    }
}
