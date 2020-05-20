using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Models
{
    public interface IPagingCriteria
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
