using Microsoft.VisualBasic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Airline
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; } = "";
        
        [MaxLength(50)]
        public string ShortName { get; set; } = "";
        
        public string AirlineCode { get; set; } = "";

        [MaxLength(200)]
        public string Location { get; set; } = "";
        
        public DateTime CreatedDate { get; set; } 

        public string imageFileName { get; set; } = "";

        [DefaultValue(true)]
        public bool Active { get; set; } 
        
        [DefaultValue(false)]
        public bool Delete { get; set; }
        
    }
}
