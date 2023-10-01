﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Employee:BaseEntity
    {


        [Required]
        [MaxLength(50)]
        [MinLength(5)]

        public string Name { get; set; }

        public string Address { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public DateTime HireDate { get; set; }

        public int DepartmentID { get; set; }
        public Department Department { get; set; }
    }
}
