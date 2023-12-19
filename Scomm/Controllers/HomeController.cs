using DAL;
using DAL.EntityModel;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scomm.Data;
using Scomm.Models;
using System.Diagnostics;

namespace Scomm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Item()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Category()
        {
            //IEnumerable<Category> model = _context.Category.Select(s => new Category
            //{
            //    ID = s.ID,
            //    CategoryName = s.CategoryName
            //});
            IEnumerable<Category> model = _unitOfWork.Categories.GetCategories();
            return View(model);
        }
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            var Category = new Category
            {
                CategoryName = category.CategoryName
            };
            _unitOfWork.Categories.Add(Category);
            _unitOfWork.Complete();
            return Ok();
        }
        public PartialViewResult AddCategoryPartialView()
        {
            return PartialView("_categoryAdd", new Category());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("Home/RemoveCategory/{id:int}")]
        public ActionResult<Category> RemoveCategory(int id)
        {
            try
            {
                var Category = _unitOfWork.Categories.GetById(id);

                if (Category == null)
                {
                    return NotFound($"Category with Id = {id} not found");
                }
                _unitOfWork.Categories.Remove(Category);
                _unitOfWork.Complete();
                return RedirectToAction("Category", "Home");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            //var Category = _unitOfWork.Categories.GetById(category.ID);

            //if (Category == null)
            //{
            //    return NotFound($"Category with Id = {category.ID} not found");
            //}
            _unitOfWork.Categories.Update(category);
            _unitOfWork.Complete();
            return Ok();
        }
    }
}