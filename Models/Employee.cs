using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuarterlySales.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]
        public string Firstname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public string Lastname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Date of birth must be in the past.")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Date of hire is required.")]
        [Display(Name = "Date of Hire")]
        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Date of hire must be in the past.")]
        [MinimumHireDate("1995-01-01", ErrorMessage = "Date of hire cannot be before the company was founded (1/1/1995).")]
        public DateTime DateOfHire { get; set; }

        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }

        // Navigation property
        [ForeignKey("ManagerId")]
        public Employee? Manager { get; set; }
    }
}