namespace Carsport.Common.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Route
    {
        [Key]
        public int RouteId { get; set; }

        [Required]
        [Display(Name = "Origin city")]
        public int OriginID { get; set; }

        [Required]
        [Display(Name = "Destination city")]
        public int DestinyID { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Numeber of seats")]
        public string NumSeats { get; set; }

        [Required]
        [Display(Name = "Departure date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy HH:mm}")]
        public DateTime Date { get; set; }

        [Display(Name = "Is deleted")]
        public bool IsDeleted { get; set; }
        
        [StringLength(128)]
        public string UserId { get; set; }

        public virtual City Origin { get; set; }

        public virtual City Destiny { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Conversation> Conversations { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public bool Inbackend { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "";
                }

                if (Inbackend)
                {
                    return $"http://movilidaducabackend.somee.com/{this.ImagePath.Substring(1)}";
                } else
                {
                    return $"https://movilidadicaapi.azurewebsites.net/{this.ImagePath.Substring(1)}";
                }
            }
        }

        [NotMapped]
        public byte[] ImageArray { get; set; }
    }
}
