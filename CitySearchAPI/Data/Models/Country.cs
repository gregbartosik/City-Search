using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CitySearchAPI.Data.Models
{
    [Table("Countries")]
    [Index(nameof(Name))]
    [Index(nameof(ISO2))]
    [Index(nameof(ISO3))]
    public class Country
    {
        // Properties       
        /// The unique id and primary key for this Country
        [Key]
        [Required]
        public int Id { get; set; }

        
        /// Country name (in UTF8 format)        
        public string Name { get; set; } = null!;

        
        /// Country code (in ISO 3166-1 ALPHA-2 format)
        [JsonPropertyName("iso2")]
        [GraphQLName("iso2")]
        public string ISO2 { get; set; } = null!;

        /// Country code (in ISO 3166-1 ALPHA-3 format)
        [JsonPropertyName("iso3")]
        [GraphQLName("iso3")]
        public string ISO3 { get; set; } = null!;

        // Client-side properties
        /// The number of cities related to this country.
        [NotMapped]
        public int TotCities
        {
            get
            {
                return (Cities != null)
                    ? Cities.Count
                    : _TotCities;
            }
            set { _TotCities = value; }
        }

        private int _TotCities = 0;
        // Navigation Properties
        /// A list containing all the cities related to this country.       
        [JsonIgnore]
        public ICollection<City>? Cities { get; set; } = null!;
        
    }
}
