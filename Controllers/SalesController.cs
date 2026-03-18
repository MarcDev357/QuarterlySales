using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using QuarterlySales.Data;
using QuarterlySales.Models;

namespace QuarterlySales.Controllers
{
    // Only authenticated users can access this controller
    [Authorize]
    public class SalesController : Controller
    {
        private readonly ISalesRepository _repository;

        public SalesController(ISalesRepository repository)
        {
            _repository = repository;
        }

        // GET: Sales/Add
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Employees = GetEmployeeSelectList();
            return View();
        }

        // POST: Sales/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Sales sales)
        {
            // Check if the sales record already exists
            if (_repository.SalesExists(sales))
            {
                ModelState.AddModelError("",
                    "Sales data for this quarter, year, and employee already exists.");
            }

            if (ModelState.IsValid)
            {
                _repository.AddSales(sales);
                TempData["SuccessMessage"] = "Sales data added successfully!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Employees = GetEmployeeSelectList();
            return View(sales);
        }

        // Helper method to populate employee dropdown
        private SelectList GetEmployeeSelectList()
        {
            var employees = _repository.GetAllEmployees()
                .Select(e => new SelectListItem
                {
                    Value = e.EmployeeId.ToString(),
                    Text = $"{e.Firstname} {e.Lastname}"
                })
                .ToList();

            return new SelectList(employees, "Value", "Text");
        }
    }
}