using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models
{
    public class ListApiResponse<T> : BaseApiResponse
    {
        public int Total { get; set; }
        public List<T> List { get; set; }
        public ListApiResponse()
        {
            List = new List<T>();
        }
    }
}
