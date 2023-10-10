using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IMapper mapper;

        public IUnitOfWork UnitOfWork { get; }

        public EmployeeController(IUnitOfWork unitOfWork,IMapper _mapper)
        {
            UnitOfWork = unitOfWork;
            mapper = _mapper;
        }


        public IActionResult Index(string SearchByName="", string SearchByDeptId = "")
        {
            IEnumerable<Employee> employees;
            IEnumerable<EmployeeViewModel> mappedEmployees;


            if ( String.IsNullOrEmpty(SearchByName) && String.IsNullOrEmpty(SearchByDeptId))
            {
                 employees = UnitOfWork.EmployeeRepository.GetAll();
                 mappedEmployees= mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

            }

            else if (!String.IsNullOrEmpty(SearchByName))
            {
                 employees = UnitOfWork.EmployeeRepository.Search(SearchByName);
                 //                                              source ,destination                     source
                mappedEmployees = mapper.Map< IEnumerable < Employee > ,IEnumerable <EmployeeViewModel>>(employees);


            }
            else
            {
                int deptId = Convert.ToInt32(SearchByDeptId);
                employees = UnitOfWork.EmployeeRepository.GetAllEmployeesByDepartmentId(deptId);
                //                                              source ,destination                     source
                mappedEmployees = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            }
            return View(mappedEmployees);

        }


        [HttpGet]
        public IActionResult Create()
        {
             ViewBag.departments=UnitOfWork.DepartmentRepository.GetAll();

            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            ViewBag.departments = UnitOfWork.DepartmentRepository.GetAll();


            try
            {
            if (ModelState.IsValid)
                {

                //                                     source ,destination                     source
                Employee MappedeEmployee = mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                MappedeEmployee.ImgUrl = DocumentSettings.UploadFile(employeeVM.Image, "ImagesStore");
                    UnitOfWork.EmployeeRepository.Add(MappedeEmployee);
                    return RedirectToAction(nameof(Index));

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(employeeVM);

        }


        public IActionResult Details(int? id)
        {

            if (id is null)
            {
                return Redirect("/Home/Error");
            }
            //from model to viewmodel
            var employee = UnitOfWork.EmployeeRepository.GetById(id);
            var mappedEmployee=mapper.Map<EmployeeViewModel>(employee);
            if (employee == null)
            {
                return View("/Home/Error");
            }
            return View(mappedEmployee);
        }


        [HttpGet]
        public IActionResult Update(int? id)
        {

            ViewBag.departments = UnitOfWork.DepartmentRepository.GetAll();

            if (id is null)
                return NotFound();


            var employee = UnitOfWork.EmployeeRepository.GetById(id);
            var mappedEmployee = mapper.Map<Employee,EmployeeViewModel>(employee);
            if (employee == null)
                return NotFound();


            return View(mappedEmployee);
        }
        [HttpPost]
        public IActionResult Update(int id, EmployeeViewModel employeeVM)
        {

            Department department = UnitOfWork.DepartmentRepository.GetById(employeeVM.DepartmentID);

            if (id != employeeVM.Id)
                return NotFound();

            try
            {
             
                Employee employee = mapper.Map<EmployeeViewModel,Employee>(employeeVM);

                if (employeeVM.Image == null)//no update in image
                {
                    //المفروض الصوره تفضل زي ماهي

                    string imageToBeAddedAgain = employeeVM.ImgUrl;

                    employee.ImgUrl = imageToBeAddedAgain;
                }

                else
                {//new image
                    string imageToBeDeleted = employeeVM.ImgUrl;
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", "ImagesStore");
                    var filePath = Path.Combine(folderPath, imageToBeDeleted);
                    System.IO.File.Delete(filePath);
                    employee.ImgUrl = DocumentSettings.UploadFile(employeeVM.Image, "ImagesStore");

                }


                employee.Department=department; //as it's not in the model

                UnitOfWork.EmployeeRepository.Update(employee);
                    return RedirectToAction("Index");
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return (View(employeeVM));
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
            if (employee.ImgUrl != null)
            {
                string imageToBeDeleted = employee.ImgUrl;
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", "ImagesStore");
                var filePath = Path.Combine(folderPath, imageToBeDeleted);
                System.IO.File.Delete(filePath);


            }

            // new FileStream(filePath, FileMode.Truncate);

            UnitOfWork.EmployeeRepository.Delete(employee);
            return RedirectToAction("Index");
        }



    }
}
