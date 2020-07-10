using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductBacklogForProjects.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string Goal { get; set; }
        public string Benefit { get; set; }
        public int PriorityId { get; set; }
        public int Sprint { get; set; }
        public int StatusId { get; set; }
        public int ProjectId { get; set; }
    }
}
