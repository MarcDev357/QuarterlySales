using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using QuarterlySales.Data;
using QuarterlySales.Models;

namespace QuarterlySales.Controllers
{
    // Only Admin users can access this controller
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly ISalesRepository _repository;

        public EmployeeController(ISalesRepository repository)
        {
            _repository = repository;
        }

        // GET: Employee/Add
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Managers = GetManagerSelectList();
            return View();
        }

        // POST: Employee/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Employee employee)
        {
            // Check if employee already exists
            if (_repository.EmployeeExists(employee))
            {
                ModelState.AddModelError("",
                    "An employee with the same first name, last name, and date of birth already exists.");
            }

            // Convert "None" manager option (0) to null
            if (employee.ManagerId == 0)
            {
                employee.ManagerId = null;
            }

            if (ModelState.IsValid)
            {
                _repository.AddEmployee(employee);
                TempData["SuccessMessage"] = "Employee added successfully!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Managers = GetManagerSelectList();
            return View(employee);
        }

        private SelectList GetManagerSelectList()
        {
            var managers = _repository.GetAllEmployees()
                .Select(e => new SelectListItem
                {
                    Value = e.EmployeeId.ToString(),
                    Text = $"{e.Firstname} {e.Lastname}"
                })
                .ToList();

            managers.Insert(0, new SelectListItem { Value = "0", Text = "None" });

            return new SelectList(managers, "Value", "Text");
        }
    }
}