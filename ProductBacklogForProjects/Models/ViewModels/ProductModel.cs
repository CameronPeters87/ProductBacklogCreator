using ProductBacklogForProjects.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ProductBacklogForProjects.Models.ViewModels
{
    public class ProductModel
    {
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
        public int ProjectId { get; set; }
        public int SubjectId { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public string SubjectName { get; set; }
        public string StatusName { get; set; }
        public string PriorityName { get; set; }
        public string ProjectName { get; set; }

    }
}