using System.ComponentModel.DataAnnotations;
using QuarterlySales.Data;

namespace QuarterlySales.Models
{
    public class UniqueSalesAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var sales = (Sales)validationContext.ObjectInstance;

            // Get the database context
            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            if (context == null)
                return ValidationResult.Success;

            // Check if sales record already exists (excluding current if editing)
            var existingSales = context.Sales
                .FirstOrDefault(s => s.Quarter == sales.Quarter &&
                                    s.Year == sales.Year &&
                                    s.EmployeeId == sales.EmployeeId &&
                                    s.SalesId != sales.SalesId); // Exclude current

            if (existingSales != null)
            {
                return new ValidationResult("Sales data for this quarter, year, and employee already exists.");
            }

            return ValidationResult.Success;
        }
    }
}