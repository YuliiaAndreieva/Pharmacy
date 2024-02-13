using System.ComponentModel.DataAnnotations;

namespace WepPha2.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }     
        public string CategoryName { get; set; }
    }
}
