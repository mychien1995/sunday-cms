using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunday.Core.Models.Base
{
    public class ListApiResponse<T> : BaseApiResponse
    {
        public int Total { get; set; }
        public List<T> List { get; set; } = new List<T>();
        public ListApiResponse()
        {
        }
        public ListApiResponse(int total, IEnumerable<T> list)
        {
            Total = total;
            List = list.ToList();
        }

        public static ListApiResponse<T1> From<T1, T2>(SearchResult<T2> searchResult, Func<T2, T1> converter) where T2 : class
        => new ListApiResponse<T1>(searchResult.Total, searchResult.Result.Select(converter).ToList());
    }
}
