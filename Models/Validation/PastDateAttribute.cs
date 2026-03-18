using System;
using System.ComponentModel.DataAnnotations;

namespace QuarterlySales.Models
{
    public class PastDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true; // Let Required attribute handle null

            if (value is DateTime date)
            {
                return date < DateTime.Now;
            }

            return false;
        }
    }
}