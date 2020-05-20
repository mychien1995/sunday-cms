using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Models
{
    public interface ISearchResult<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
