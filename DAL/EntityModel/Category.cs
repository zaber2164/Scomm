using System.ComponentModel.DataAnnotations;

namespace DAL.EntityModel
{
    public class Category
    {
        [Required]
        public long ID { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}