using Microsoft.EntityFrameworkCore;
using QuarterlySales.Models;

namespace QuarterlySales.Data
{
    public class SalesRepository : ISalesRepository
    {
        private readonly ApplicationDbContext _context;

        public SalesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Sales> GetAllSales()
        {
            return _context.Sales.Include(s => s.Employee);
        }

        public IQueryable<Employee> GetAllEmployees()
        {
            return _context.Employees;
        }

        public Sales GetSalesById(int id)
        {
            return _context.Sales
                .Include(s => s.Employee)
                .FirstOrDefault(s => s.SalesId == id);
        }

        public void AddSales(Sales sales)
        {
            _context.Sales.Add(sales);
            _context.SaveChanges();
        }

        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public bool EmployeeExists(Employee employee)
        {
            return _context.Employees.Any(e =>
                e.Firstname == employee.Firstname &&
                e.Lastname == employee.Lastname &&
                e.DOB.Date == employee.DOB.Date);
        }

        public bool SalesExists(Sales sales)
        {
            return _context.Sales.Any(s =>
                s.Quarter == sales.Quarter &&
                s.Year == sales.Year &&
                s.EmployeeId == sales.EmployeeId);
        }
    }
}