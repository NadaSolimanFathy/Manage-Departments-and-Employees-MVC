﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Department:BaseEntity
    {
        public string Code { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfCreation { get; set; }
        //date only /time only => فيهم مشاكل فال مابينج
    }
}
