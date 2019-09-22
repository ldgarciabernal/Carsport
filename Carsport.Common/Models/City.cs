namespace Carsport.Common.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Route> Origins { get; set; }

        [JsonIgnore]
        public virtual ICollection<Route> Destinies { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}