using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EntityModel
{
    public class Item
    {
        public long ID { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public int ItemQty { get; set; }
        public int CategoryID { get; set; }
        //public string CategoryName { get; set; }
    }
    public class ItemViewModel
    {
        public long ID { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public int ItemQty { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
