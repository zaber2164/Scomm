using DAL.EntityModel;
using DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Implementation
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
        public IEnumerable<Category> GetCategories()
        {
            return _context.Category.OrderByDescending(d => d.ID).Take(100).ToList();
        }
    }
}
