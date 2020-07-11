using ProductBacklogForProjects.Entities;
using ProductBacklogForProjects.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductBacklogForProjects.Models
{
    public class ProductCreateViewModel
    {
        // For Creating the UserView
        [DisplayName("Subject Name")]
        public string SubjectName { get; set; }
        // For Creating the Product
        public ICollection<Subject> Subjects { get; set; }
        [DisplayName("I want to")]
        public string Goal { get; set; }
        [DisplayName("So that")]
        public string Benefit { get; set; }
        [DisplayName("Priority")]
        public ICollection<Priority> Priorities { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        [Required]
        public int Sprint { get; set; } = 1;
        [DisplayName("Status")]
        public ICollection<Status> Statuses { get; set; }
        [DisplayName("Project")]
        public ICollection<Project> Projects { get; set; }
        // Ids
        public int SubjectId { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public int ProjectId { get; set; }
        public int ProductId { get; set; }

    }
}