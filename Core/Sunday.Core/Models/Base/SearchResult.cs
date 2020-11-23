using System;

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
    }
}
