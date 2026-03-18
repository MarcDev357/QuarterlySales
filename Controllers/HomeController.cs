using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuarterlySales.Data;
using QuarterlySales.Models;
using QuarterlySales.Models.ViewModels;

namespace QuarterlySales.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISalesRepository _repository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ISalesRepository repository, ILogger<HomeController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index(int? employeeId, string sortField = "Year",
            string sortDirection = "asc", int page = 1, int pageSize = 4)
        {
            try
            {
                var sales = _repository.GetAllSales().AsNoTracking();
                var employees = _repository.GetAllEmployees().ToList();

                // Query used for totals before paging
                var allSalesQuery = _repository.GetAllSales().AsNoTracking();

                if (employeeId.HasValue)
                {
                    sales = sales.Where(s => s.EmployeeId == employeeId);
                    allSalesQuery = allSalesQuery.Where(s => s.EmployeeId == employeeId);
                }

                var salesList = allSalesQuery.ToList();

                decimal totalSales = salesList.Sum(s => s.Amount);

                decimal avgQuarterlySales = salesList.Any()
                    ? salesList.Average(s => s.Amount)
                    : 0;

                var salesByYear = salesList
                    .GroupBy(s => s.Year)
                    .ToDictionary(g => g.Key, g => g.Sum(s => s.Amount));

                var salesByQuarter = salesList
                    .GroupBy(s => s.Quarter)
                    .ToDictionary(g => g.Key, g => g.Sum(s => s.Amount));

                decimal? previousYearTotal = null;
                decimal? yoyGrowth = null;

                if (salesByYear.Count >= 2)
                {
                    var latestYear = salesByYear.Keys.Max();
                    var previousYear = salesByYear.Keys.Where(y => y < latestYear).Max();

                    previousYearTotal = salesByYear[previousYear];
                    var currentYearTotal = salesByYear[latestYear];

                    if (previousYearTotal > 0)
                    {
                        yoyGrowth = ((currentYearTotal - previousYearTotal) / previousYearTotal) * 100;
                    }
                }

                var topEmployee = salesList
                    .Where(s => s.Employee != null)
                    .GroupBy(s => s.Employee)
                    .Select(g => new
                    {
                        Employee = g.Key,
                        Total = g.Sum(s => s.Amount)
                    })
                    .OrderByDescending(x => x.Total)
                    .FirstOrDefault();

                string topEmployeeName = "N/A";
                decimal topEmployeeSales = 0;

                if (topEmployee?.Employee != null)
                {
                    topEmployeeName = $"{topEmployee.Employee.Firstname} {topEmployee.Employee.Lastname}";
                    topEmployeeSales = topEmployee.Total;
                }

                // Sorting
                sales = sortField.ToLower() switch
                {
                    "employee" => sortDirection == "asc"
                        ? sales.OrderBy(s => s.Employee.Lastname).ThenBy(s => s.Employee.Firstname)
                        : sales.OrderByDescending(s => s.Employee.Lastname).ThenByDescending(s => s.Employee.Firstname),

                    "quarter" => sortDirection == "asc"
                        ? sales.OrderBy(s => s.Quarter)
                        : sales.OrderByDescending(s => s.Quarter),

                    "amount" => sortDirection == "asc"
                        ? sales.OrderBy(s => s.Amount)
                        : sales.OrderByDescending(s => s.Amount),

                    _ => sortDirection == "asc"
                        ? sales.OrderBy(s => s.Year)
                        : sales.OrderByDescending(s => s.Year)
                };

                int totalSalesCount = sales.Count();
                int totalPages = (int)Math.Ceiling(totalSalesCount / (double)pageSize);

                if (page < 1) page = 1;
                if (page > totalPages && totalPages > 0) page = totalPages;

                var pagedSales = sales
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var viewModel = new SalesListViewModel
                {
                    Sales = pagedSales,
                    Employees = employees,
                    SelectedEmployeeId = employeeId,
                    SortField = sortField,
                    SortDirection = sortDirection,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    TotalRecords = totalSalesCount,

                    TotalSalesAmount = totalSales,
                    AverageQuarterlySales = avgQuarterlySales,
                    SalesByYear = salesByYear,
                    SalesByQuarter = salesByQuarter,
                    PreviousYearTotal = previousYearTotal,
                    YearOverYearGrowth = yoyGrowth,
                    TopPerformingEmployee = topEmployeeName,
                    TopEmployeeSales = topEmployeeSales
                };

                ViewBag.EmployeeId = employeeId;
                ViewBag.SortField = sortField;
                ViewBag.SortDirection = sortDirection;
                ViewBag.Page = page;
                ViewBag.PageSize = pageSize;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading sales data");
                TempData["ErrorMessage"] = "An error occurred while loading sales data.";
                return View(new SalesListViewModel());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}