using System.ComponentModel.DataAnnotations;

namespace LifeCycle.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
    
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Line> Lines { get; set; } = new List<Line>();
    }
}
