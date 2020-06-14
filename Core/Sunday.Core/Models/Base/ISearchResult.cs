using System.Collections.Generic;

namespace Sunday.Core.Models
{
    public interface ISearchResult<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
