using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitySearchAPI.Data.Models
{
    [Table("Cities")]
    [Index(nameof(Name))]
    [Index(nameof(Lat))]
    [Index(nameof(Lon))]
    public class City
    {
        // Properties
        /// The unique id and primary key for this City
        [Key]
        [Required]
        public int Id { get; set; }

        
        /// City name (in UTF8 format)
        public string Name { get; set; } = null!;

        
        /// City latitude
        [Column(TypeName = "decimal(7,4)")]
        public decimal Lat { get; set; }

        
        /// City longitude      
        [Column(TypeName = "decimal(7,4)")]
        public decimal Lon { get; set; }

        
        /// Country Id (foreign key)       
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
       

        // Navigation Properties
        
        /// The country related to this city.       
        public Country? Country { get; set; } = null!;
        
    }
}