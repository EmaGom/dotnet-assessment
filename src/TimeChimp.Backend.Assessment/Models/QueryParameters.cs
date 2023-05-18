using Microsoft.Identity.Client;
using System;

namespace TimeChimp.Backend.Assessment.Models
{
    public class QueryParameters
    {
        public DateTime? PostedDate { get; set; }
        public string Title { get; set; }

        // Parameters for being used by a table or optimized the query.
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;

        public bool SamePropertiesAs(QueryParameters other)
        {
            var result = false;
            if (this.PostedDate == other.PostedDate &&
               this.Title == other.Title &&
               this.SortBy == other.SortBy &&
               this.SortDirection == other.SortDirection &&
               this.PageIndex == other.PageIndex &&
               this.PageSize == other.PageSize)
                result = true;

            return result;
        }

    }
}
