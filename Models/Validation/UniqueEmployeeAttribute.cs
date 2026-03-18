using System.ComponentModel.DataAnnotations;
using QuarterlySales.Data;

namespace QuarterlySales.Models
{
    public class UniqueEmployeeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var employee = (Employee)validationContext.ObjectInstance;

            // Get the database context
            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            if (context == null)
                return ValidationResult.Success;

            // Check if employee already exists (excluding current employee if editing)
            var existingEmployee = context.Employees
                .FirstOrDefault(e => e.Firstname == employee.Firstname &&
                                    e.Lastname == employee.Lastname &&
                                    e.DOB.Date == employee.DOB.Date && // Compare dates only
                                    e.EmployeeId != employee.EmployeeId); // Exclude current

            if (existingEmployee != null)
            {
                return new ValidationResult("An employee with the same first name, last name, and date of birth already exists.");
            }

            return ValidationResult.Success;
        }
    }
}