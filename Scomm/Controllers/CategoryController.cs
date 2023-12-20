using DAL;
using DAL.EntityModel;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Scomm.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(ILogger<CategoryController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
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
