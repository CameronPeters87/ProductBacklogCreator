﻿using ProductBacklogForProjects.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ProductBacklogForProjects.Models
{
    public class ProductExcelView
    {
        public string Subject { get; set; }
        [DisplayName("I want to")]
        public string Goal { get; set; }
        [DisplayName("So that")]
        public string Benefit { get; set; }
        [DisplayName("Priority")]
        public string Priority { get; set; }
        public int Sprint { get; set; }
        [DisplayName("Status")]
        public string Status { get; set; }
    }
}