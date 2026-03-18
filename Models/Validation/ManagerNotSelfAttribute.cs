using System.ComponentModel.DataAnnotations;
using QuarterlySales.Data;

namespace QuarterlySales.Models
{
    public class ManagerNotSelfAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var employee = (Employee)validationContext.ObjectInstance;

            // Check if manager is the same as employee
            if (employee.ManagerId.HasValue && employee.ManagerId.Value == employee.EmployeeId)
            {
                return new ValidationResult("Employee and manager cannot be the same person.");
            }

            return ValidationResult.Success;
        }
    }
}