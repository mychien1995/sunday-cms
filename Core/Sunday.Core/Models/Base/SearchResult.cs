using System;
using System.Linq;

namespace Sunday.Core.Models.Base
{
    public class SearchResult<T> : ISearchResult<T> where T : class
    {
        public SearchResult()
        {
            Result = Array.Empty<T>();
            Total = 0;
        }
        public int Total { get; set; }
        public T[] Result { get; set; }

        public static SearchResult<T> Empty => new SearchResult<T>();

        public void Append(SearchResult<T> other)
        {
            Total = other.Total;
            Result = Result.Concat(other.Result).ToArray();
        }
    }
}
