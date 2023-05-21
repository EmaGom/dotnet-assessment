using System;
using Elasticsearch.Net;
using TimeChimp.Backend.Assessment.Enums;

namespace TimeChimp.Backend.Assessment.Models
{
    public class QueryParameters
    {
        public DateTime? PublishDate { get; set; }
        public string Title { get; set; }

        // Parameters for being used by a table or optimized the query.
        public string SortBy { get; set; } = SortByEnum.PublishDate.GetStringValue();
        public string SortDirection { get; set; } = SortDirectionEnum.Desc.GetStringValue();
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;

        public bool SamePropertiesAs(QueryParameters other)
        {
            var result = false;
            if (this.PublishDate == other.PublishDate &&
               this.Title == other.Title &&
               this.PageIndex == other.PageIndex &&
               this.PageSize == other.PageSize &&
               this.SortBy == other.SortBy &&
               this.SortDirection == other.SortDirection)
                result = true;

            return result;
        }
    }
}
