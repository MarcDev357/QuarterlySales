using QuarterlySales.Models;

namespace QuarterlySales.Models.ViewModels
{
    public class SalesListViewModel
    {
        public IEnumerable<Sales> Sales { get; set; } = new List<Sales>();
        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();
        public int? SelectedEmployeeId { get; set; }
        public string SortField { get; set; } = "Year";
        public string SortDirection { get; set; } = "asc";
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }

        // Total sales properties
        public decimal TotalSalesAmount { get; set; }
        public decimal AverageQuarterlySales { get; set; }
        public int TotalRecords { get; set; }
        public Dictionary<int, decimal> SalesByYear { get; set; } = new Dictionary<int, decimal>();
        public Dictionary<int, decimal> SalesByQuarter { get; set; } = new Dictionary<int, decimal>();
        public decimal? PreviousYearTotal { get; set; }
        public decimal? YearOverYearGrowth { get; set; }
        public string TopPerformingEmployee { get; set; } = string.Empty;
        public decimal TopEmployeeSales { get; set; }
    }
}