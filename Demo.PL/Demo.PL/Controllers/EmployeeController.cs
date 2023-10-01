using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        public IUnitOfWork UnitOfWork { get; }

        public EmployeeController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            var employees=UnitOfWork.EmployeeRepository.GetAll();
            return View(employees);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.departments=UnitOfWork.DepartmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee  employee)
        {
            {
                UnitOfWork.EmployeeRepository.Add(employee);

            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Details(int? id)
        {

            if (id is null)
            {
                return Redirect("/Home/Error");
            }

            var employee = UnitOfWork.EmployeeRepository.GetById(id);
            if (employee == null)
            {
                return View("/Home/Error");

            }

            return View(employee);
        }


        [HttpGet]
        public IActionResult Update(int? id)
        {
            ViewBag.departments = UnitOfWork.DepartmentRepository.GetAll();

            if (id is null)
                return NotFound();


            var employee = UnitOfWork.EmployeeRepository.GetById(id);
            if (employee == null)
                return NotFound();


            return View(employee);
        }
        [HttpPost]
        public IActionResult Update(int id, Employee employee)
        {

           var department = UnitOfWork.DepartmentRepository.GetById(id);

            if (id != employee.Id)
                return NotFound();


            try
            {

                    employee.Department=department;
                    UnitOfWork.EmployeeRepository.Update(employee);
                    return RedirectToAction("Index");
                
            }
            catch (Exception ex)
            {
                ex.InnerException.InnerException.ToString();

            }
            return (View(employee));
        }


        public IActionResult Delete(int? id)
        {

            if (id is null)
            {
                return Redirect("/Home/Error");
            }

            var employee = UnitOfWork.EmployeeRepository.GetById(id);
            if (employee is null)
            {
                return View("/Home/Error");

            }
            UnitOfWork.EmployeeRepository.Delete(employee);
            return RedirectToAction("Index");
        }



    }
}
