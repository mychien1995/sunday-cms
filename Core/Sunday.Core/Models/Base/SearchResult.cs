﻿using System.Collections.Generic;

namespace Sunday.Core.Models
{
    public class SearchResult<T> : ISearchResult<T>
    {
        public SearchResult()
        {
            Result = new List<T>();
            Total = 0;
        }
        public int Total { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
