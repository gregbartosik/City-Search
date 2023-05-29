using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;
using EFCore.BulkExtensions;

namespace CitySearchAPI.Data
{
    public class ApiResult<T>
    {
        /// Private constructor called by the CreateAsync method.
        private ApiResult(
            List<T> data,
            int count,
            int pageIndex,
            int pageSize,
            string? sortColumn,
            string? sortOrder,
            string? filterColumn,
            string? filterQuery)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            SortColumn = sortColumn;
            SortOrder = sortOrder;
            FilterColumn = filterColumn;
            FilterQuery = filterQuery;
        }

        //  Methods
        /// Pages, sorts and/or filters a IQueryable source.
        /// <param name="source">An IQueryable source of generic 
        /// type</param>
        /// <param name="pageIndex">Zero-based current page index 
        /// (0 = first page)</param>
        /// <param name="pageSize">The actual size of 
        /// each page</param>
        /// <param name="sortColumn">The sorting colum name</param>
        /// <param name="sortOrder">The sorting order ("ASC" or 
        /// "DESC")</param>
        /// <param name="filterColumn">The filtering column
        ///  name</param>
        /// <param name="filterQuery">The filtering query (value to
        /// lookup)</param>
        /// <returns>
        /// A object containing the IQueryable paged/sorted/filtered
        /// result 
        /// and all the relevant paging/sorting/filtering navigation
        /// info.
        /// </returns>
        public static async Task<ApiResult<T>> CreateAsync(
            IQueryable<T> source,
            int pageIndex,
            int pageSize,
            string? sortColumn = null,
            string? sortOrder = null,
            string? filterColumn = null,
            string? filterQuery = null)
        {
            if (!string.IsNullOrEmpty(filterColumn)
                && !string.IsNullOrEmpty(filterQuery)
                && IsValidProperty(filterColumn))
            {
                source = source.Where(
                    string.Format("{0}.StartsWith(@0)",
                    filterColumn),
                    filterQuery);
            }

            var count = await source.CountAsync();

            if (!string.IsNullOrEmpty(sortColumn)
                && IsValidProperty(sortColumn))
            {
                sortOrder = !string.IsNullOrEmpty(sortOrder)
                    && sortOrder.ToUpper() == "ASC"
                    ? "ASC"
                    : "DESC";
                source = source.OrderBy(
                    string.Format(
                        "{0} {1}",
                        sortColumn,
                        sortOrder)
                    );
            }

            source = source
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

#if DEBUG 
            // retrieve the SQL query (for debug purposes)
            var sql = source.ToParametrizedSql();
            // TODO: do something with the sql string
#endif

            var data = await source.ToListAsync();

            return new ApiResult<T>(
                data,
                count,
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery);
        }

        
        /// Checks if the given property name exists
        /// to protect against SQL injection attacks
        public static bool IsValidProperty(
            string propertyName,
            bool throwExceptionIfNotFound = true)
        {
            var prop = typeof(T).GetProperty(
                propertyName,
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.Instance);
            if (prop == null && throwExceptionIfNotFound)
                throw new NotSupportedException($"ERROR: Property '{propertyName}' does not exist.");
            return prop != null;
        }
        //  Properties  
        /// IQueryable data result to return.
        public List<T> Data { get; private set; }
        
        /// Zero-based index of current page.
        public int PageIndex { get; private set; }

        /// Number of items contained in each page.        
        public int PageSize { get; private set; }

        /// Total items count        
        public int TotalCount { get; private set; }

        /// Total pages count        
        public int TotalPages { get; private set; }

        /// TRUE if the current page has a previous page,
        /// FALSE otherwise.
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        
        /// TRUE if the current page has a next page, FALSE otherwise.
        public bool HasNextPage
        {
            get
            {
                return ((PageIndex + 1) < TotalPages);
            }
        }

        
        /// Sorting Column name (or null if none set)   
        public string? SortColumn { get; set; }

        /// Sorting Order ("ASC", "DESC" or null if none set)        
        public string? SortOrder { get; set; }

        /// Filter Column name (or null if none set)
        public string? FilterColumn { get; set; }
        /// Filter Query string 
        /// (to be used within the given FilterColumn)
        
        public string? FilterQuery { get; set; }
    }
}
