using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class EmployeeViewModel
    {


        public int Id { get; set; }

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

        public IFormFile? Image { get; set; }
        public string? ImgUrl { get; set; }

    }
}
