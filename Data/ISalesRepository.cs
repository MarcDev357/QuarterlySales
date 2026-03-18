using QuarterlySales.Models;

namespace QuarterlySales.Data
{
    public interface ISalesRepository
    {
        IQueryable<Sales> GetAllSales();
        IQueryable<Employee> GetAllEmployees();
        Sales GetSalesById(int id);
        void AddSales(Sales sales);
        void AddEmployee(Employee employee);
        bool EmployeeExists(Employee employee);
        bool SalesExists(Sales sales);
    }
}