using DAL.EntityModel;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Implementation
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }
        public IEnumerable<Item> GetItems()
        {
            return _context.Item.OrderByDescending(d => d.ID).Take(100).ToList();
        }
    }
}
