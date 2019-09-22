namespace Carsport.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PublishedOn { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "";
                }

                return $"http://movilidaducabackend.somee.com/{this.ImagePath.Substring(1)}";
            }
        }

        [StringLength(128)]
        public string UserId { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
