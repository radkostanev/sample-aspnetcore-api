using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleNetCoreApi.Services.Helpers
{
    public class QueryParams
    {
        private const int MAX_PAGE_COUNT = 100;
        private int pageCount = MAX_PAGE_COUNT;
        public int Page { get; set; } = 1;

        public int PageCount
        {
            get => this.pageCount;
            set => this.pageCount = value > MAX_PAGE_COUNT
                ? MAX_PAGE_COUNT
                : value;
        }

        public string Query { get; set; }

        public string OrderBy { get; set; } = "Name";

        public bool HasPrevious() => this.Page > 1;

        public bool HasNext(int totalCount) => (int)this.GetTotalPages(totalCount) > this.Page;

        public double GetTotalPages(int totalCount) => Math.Ceiling(totalCount / (double)this.PageCount);

        public bool HasQuery() => !string.IsNullOrEmpty(this.Query);

        public bool IsDescending() => !string.IsNullOrEmpty(this.OrderBy) 
            && this.OrderBy.Split(' ')
            .Last()
            .ToLowerInvariant()
            .StartsWith("desc");
    }
}
