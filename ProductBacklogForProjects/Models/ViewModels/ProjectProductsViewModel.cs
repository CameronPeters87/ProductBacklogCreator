using ProductBacklogForProjects.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ProductBacklogForProjects.Models.ViewModels
{
    public class ProjectProductsViewModel
    {
        // To create a new product
        public int Id { get; set; }
        [DisplayName("As a")]
        public ICollection<Subject> Subjects { get; set; }
        [DisplayName("I want to")]
        public string Goal { get; set; }
        [DisplayName("So that")]
        public string Benefit { get; set; }
        [DisplayName("Priority")]
        public ICollection<Priority> Priorities { get; set; }
        public int Sprint { get; set; }
        [DisplayName("Status")]
        public ICollection<Status> Statuses { get; set; }
        [DisplayName("Project")]
        public ICollection<Project> Projects { get; set; }
        //public int ProjectId { get; set; }
        public int SubjectId { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }

        // To List the Products
        public ICollection<ProductModel> ProductModels { get; set; }
        public int ProjectId { get; set; }
        // For when I delete or edit a product
        public int ProductId { get; set; }
        public string ProjectName { get; set; }
    }
}