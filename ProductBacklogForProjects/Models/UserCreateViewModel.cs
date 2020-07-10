using ProductBacklogForProjects.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductBacklogForProjects.Models
{
    public class UserCreateViewModel
    {
        // For Creating the UserView
        public string SubjectName { get; set; }
        // For creating the product
        public int ProjectId { get; set; }
        public int ProductId { get; set; }

    }
}