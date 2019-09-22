using System.ComponentModel.DataAnnotations;

namespace Carsport.Common.Models
{
    public class Station
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CoreId { get; set; }
    }
}
