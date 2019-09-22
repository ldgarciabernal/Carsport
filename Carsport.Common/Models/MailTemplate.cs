namespace Carsport.Common.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class MailTemplate
    {
        [Key]
        public int MailTemplateId { get; set; }

        [Required]
        [Display(Name = "Type")]
        [MaxLength(64)]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Text")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}
