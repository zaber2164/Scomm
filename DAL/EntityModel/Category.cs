using System.ComponentModel.DataAnnotations;

namespace DAL.EntityModel
{
    public class Category
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}