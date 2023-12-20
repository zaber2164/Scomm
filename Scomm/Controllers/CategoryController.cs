using DAL;
using DAL.EntityModel;
using DAL.UnitOfWork;
using ExcelDataReader;
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
        public IActionResult UploadCategoryExcel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadCategoryExcel(IFormFile file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //Upload the excel file
            if (file != null && file.Length > 0)
            {
                var uploadDirectory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads";
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }
                var filePath = Path.Combine(uploadDirectory, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                //Read file
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    var excelData = new List<List<object>>();
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }
                                // Build the category object
                                Category category = new Category();
                                category.CategoryName = reader.GetValue(1).ToString();

                                // save to DB using entity framework
                                _unitOfWork.Categories.Add(category);
                                _unitOfWork.Complete();
                            }
                        } while (reader.NextResult());
                        ViewBag.excelData = excelData;
                        ViewBag.Message = "success";
                    }
                }
            }
            else
                ViewBag.Message = "empty";
            return View();
        }
    }
}
