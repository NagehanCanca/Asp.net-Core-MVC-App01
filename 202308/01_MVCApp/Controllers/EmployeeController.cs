using _01_MVCApp.Models;
using Microsoft.AspNetCore.Mvc;
using NorthwindModel;

namespace _01_MVCApp.Controllers
{
    public class EmployeeController : Controller
    {
        NorthwindDbContext context;

        public object ProductName { get; private set; }
        public object Quantity { get; private set; }

        public EmployeeController()
        {
            context = new NorthwindDbContext();
        }

        public IActionResult List()
        {
            List<EmployeeListViewModel> employees = context.Employees.Select(a => new EmployeeListViewModel()
            {
                Name = a.FirstName,
                Surname = a.LastName,
                Title = a.Title,
                EmployeeId = a.EmployeeId,
            }).ToList();


            return View(employees);
        }


        public IActionResult Orders(int id)
        {
            List<OrderListViewModel> orders = context.Orders.Include(a => a.Customer)
                                                     .Where(a => a.EmployeeId == id)
                                                     .Select(a => new OrderListViewModel()
                                                     {
                                                         OrderNo = a.OrderId,
                                                         OrderDate = a.OrderDate,
                                                         Customer = a.Customer.CompanyName
                                                     }).ToList();
            return View(orders);
        }

        public IActionResult OrderDetail(int d)
        {
            List<OrderProductListViewModel> orderProducts = context.OrderDetails.Include(a => a.Product)
                                                           .Where(a => a.OrderId == id)
                                                           .Select(a => new OrderProductListViewModel()){
                                                               ProductName = a.Product.ProductName,
                                                               Quantity =a.Quantity
                                                           }).ToList(); 

            return View(orderProducts);
        }

        public IActionResult Profile(int id)
        {
            Employee employee = context.Employees.SingleOrDefault(a => a.EmployeeId == id);
            if (employee == null)
            {
                ViewBag.Mesaj = "Kayıt Bulunamadı";
            }

            else
            {
                EmployeeDetailViewModel model = new EmployeeDetailViewModel()
                {
                    Birthdate = employee.Birthdate,
                    City = employee.City,
                    Country = employee.Country,
                    Name = employee.FirstName,
                    SurName = employee.LastName,
                    Phone = employee.Phone,
                    Title = employee.Title,
                    EmployeId=employee.EmployeId,
                };
            }

            return View();
        }
    }

    [HttpPost]

    public IActionResult Update([FromRoute]int id,EmployeeDetailViewModel model)
    {
        Employee employee = context.Employees.SingleOrDefault(a => a.EmployeeId == id);

        if (employee != null)
        {
            employee.City = model.City;
            employee.Country = model.Country;
            employee.HomePhone = model.Phone;
            context.SaveChanges();
        }

        return RedirectToAction(nameof(List));
    }
}


