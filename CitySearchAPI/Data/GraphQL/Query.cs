using Microsoft.EntityFrameworkCore;
using CitySearchAPI.Data.Models;

namespace CitySearchAPI.Data.GraphQL
{
    public class Query
    {
        // Get all Cities
        [Serial]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<City> GetCities([Service] ApplicationDbContext context)
            => context.Cities;

        // Get all Countries
        [Serial]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Country> GetCountries([Service] ApplicationDbContext context)
            => context.Countries;
    }


}
