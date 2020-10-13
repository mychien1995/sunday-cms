using System.Collections.Generic;
using Sunday.Core.Extensions;

namespace Sunday.Core.Models.Base
{
    public class SearchResult<T> : ISearchResult<T> where T : class
    {
        public SearchResult()
        {
            Result = new List<T>();
            Total = 0;
        }
        public int Total { get; set; }
        public IEnumerable<T> Result { get; set; }

        public SearchResult<T2> CloneTo<T2>() where T2 : class => new SearchResult<T2>()
        {
            Result = this.Result.CastListTo<T2>(),
            Total = Total
        };
    }
}
