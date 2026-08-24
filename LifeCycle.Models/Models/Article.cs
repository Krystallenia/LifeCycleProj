using System.ComponentModel.DataAnnotations;

namespace LifeCycle.Models
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }

        public string ArticleNumber { get; set; }

        public string Manufacturer { get; set; }

        public string Name { get; set; }
       
        public virtual ICollection<Component> Components { get; set; } = new List<Component>(); 
    }
}
