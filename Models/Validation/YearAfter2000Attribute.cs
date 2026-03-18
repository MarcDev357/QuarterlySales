using System.ComponentModel.DataAnnotations;

namespace QuarterlySales.Models
{
    public class YearAfter2000Attribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (value is int year)
            {
                return year > 2000;
            }

            return false;
        }
    }
}