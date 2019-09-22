namespace Carsport.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Bycicle
    {
        [Key]
        public int BycicleId { get; set; }

        [Required]
        [StringLength(256)]
        public string University { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [StringLength(256)]
        public string Street { get; set; }

        [Required]
        [StringLength(16)]
        public string Longitude { get; set; }

        [Required]
        [StringLength(16)]
        public string Latitude { get; set; }

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "no_bycicle";
                }

                return $"http://movilidaducabackend.somee.com/{this.ImagePath.Substring(1)}";
            }
        }

        public override string ToString()
        {
            return this.University;
        }
    }
}