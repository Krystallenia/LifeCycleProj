using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeCycle.Models
{
    public class Machine
    {
        [Key]
        public int MachineId { get; set; }

        public string Name { get; set; }

        public int LineId { get; set; }
        [ForeignKey("LineId")]
        public Line Line { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Component> Components { get; set; } = new List<Component>();
    }
}
