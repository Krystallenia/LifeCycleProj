using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeCycle.Models
{
    public class Line
    {
        [Key]
        public int LineId { get; set; }

        public string Name { get; set; }

        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        public Location Location { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();
    }
}
