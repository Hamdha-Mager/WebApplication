using System.ComponentModel.DataAnnotations;


namespace WebApplication1.Models
{
    public class AirlineDto
    {
        public string Name { get; set; } = "";

        [Required, MaxLength(50)]
        public string ShortName { get; set; } = "";
        [Required]
        public string AirlineCode { get; set; } = "";

        [Required, MaxLength(200)]
        public string Location { get; set; } = "";
        [Required]
        public DateTime CreatedDate { get; set; }

        public IFormFile? imageFile { get; set; } 

        
        public bool Active { get; set; }

        
        public bool Delete { get; set; }
    }
}
