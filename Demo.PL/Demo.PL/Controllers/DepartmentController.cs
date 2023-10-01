using Demo.DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        //private readonly IDepartmentRepository departmentRepository;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(
            //IDepartmentRepository _departmentRepository,
            IUnitOfWork _unitOfWork,
            ILogger<DepartmentController> logger)
        {
           unitOfWork = _unitOfWork;

            //departmentRepository = _departmentRepository;
            _logger = logger;
        }
        public IActionResult Index()
        {

            var departments = unitOfWork.DepartmentRepository.GetAll();
            return View(departments);
            //departments is the model that will be passed to the front-part to be rendered

        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
            //departments is the model that will be passed to the front-part to be rendered

        }
        [HttpPost]
        public IActionResult Create(Department department)
        {

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Valid Objeccccccccccccccccccct");
                unitOfWork.DepartmentRepository.Add(department);
                return RedirectToAction("Index");


            }
            return (View(department));
            //departments is the model that will be passed to the front-part to be rendered

        }


        public IActionResult Details(int? id)
        {

            if (id is null)
            {
               // return NotFound();
                return Redirect("/Home/Error");
            }

            var department = unitOfWork.DepartmentRepository.GetById(id);
            if(department == null)
            {
                return View("/Home/Error");

            }

            return View(department);
        }




        [HttpGet]
        public IActionResult Update(int? id)
        {

            if (id is null)
               return NotFound();
            

            var department = unitOfWork.DepartmentRepository.GetById(id);
            if (department == null)
                return NotFound();


            return View(department);
        }
        [HttpPost]
        public IActionResult Update(int id,Department department)
        {
            if(id!=department.Id)
                return NotFound();


            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.DepartmentRepository.Update(department);
                    return RedirectToAction("Index");
                }
            }
             catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return (View(department));
        }


        public IActionResult Delete(int? id)
        {

            if (id is null)
            {
                return Redirect("/Home/Error");
            }

            var department = unitOfWork.DepartmentRepository.GetById(id);
            if (department is null)
            {
                return View("/Home/Error");

            }
            unitOfWork.DepartmentRepository.Delete(department);
            return RedirectToAction("Index");
        }




    }
}
