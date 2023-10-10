using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Demo.PL.Helper;


namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly ILogger<DepartmentController> _logger;
        private readonly IMapper mapper;

        public DepartmentController(
            IUnitOfWork _unitOfWork,
            ILogger<DepartmentController> logger,
            IMapper _mapper)
        {
           unitOfWork = _unitOfWork;
            _logger = logger;
            mapper = _mapper;
        }
        public IActionResult Index()
        {

            var departments = unitOfWork.DepartmentRepository.GetAll();
            return View(departments);

        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {

            if (ModelState.IsValid)
            {
                var department=mapper.Map<Department>(departmentVM);
                unitOfWork.DepartmentRepository.Add(department);
                return RedirectToAction("Index");


            }
            return (View(departmentVM));

        }


        public IActionResult Details(int? id)
        {

            if (id is null)
            {
                return Redirect("/Home/Error");
            }

            var department = unitOfWork.DepartmentRepository.GetById(id);
            var mappedDept=mapper.Map<DepartmentViewModel>(department);
            if(department == null)
            {
                return View("/Home/Error");

            }

            return View(mappedDept);
        }




        [HttpGet]
        public IActionResult Update(int? id)
        {

            if (id is null)
               return NotFound();

          

            var department = unitOfWork.DepartmentRepository.GetById(id);
            if (department == null)
                return NotFound();

            var mappedDept = mapper.Map< Department,DepartmentViewModel>(department);

            
            return View(mappedDept);
        }
        [HttpPost]
        public IActionResult Update(int id,DepartmentViewModel departmentVm)
        {

            if(id!=departmentVm.Id)
                return NotFound();


            try
            {
                if (ModelState.IsValid)
                {
                    var mappedDept = mapper.Map<DepartmentViewModel, Department>(departmentVm);
                    unitOfWork.DepartmentRepository.Update(mappedDept);
                    return RedirectToAction("Index");
                }
            }
             catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return (View(departmentVm));
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
