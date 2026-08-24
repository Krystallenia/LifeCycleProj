using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeCycle.Models
{
    public class Component
    {

        [Key]
        public int ComponentId { get; set; }

        public int MachineId { get; set; }
        [ForeignKey("MachineId")]
        public Machine Machine { get; set; }


        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }


        public string ComponentGroup { get; set; }

        public string ComponentType { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }


    }
}
