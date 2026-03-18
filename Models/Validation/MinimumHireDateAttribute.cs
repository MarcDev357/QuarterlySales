using System;
using System.ComponentModel.DataAnnotations;

namespace QuarterlySales.Models
{
    public class MinimumHireDateAttribute : ValidationAttribute
    {
        private readonly DateTime _minDate;

        public MinimumHireDateAttribute(string minDate)
        {
            _minDate = DateTime.Parse(minDate);
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (value is DateTime date)
            {
                return date >= _minDate;
            }

            return false;
        }
    }
}