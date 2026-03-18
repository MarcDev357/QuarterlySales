using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuarterlySales.Models.ViewModels
{
    public class EmployeeSelectListViewModel
    {
        public IEnumerable<SelectListItem> Employees { get; set; } = new List<SelectListItem>();
        public int? SelectedEmployeeId { get; set; }
    }
}