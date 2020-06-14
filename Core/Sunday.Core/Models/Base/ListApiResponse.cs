using System.Collections.Generic;

namespace Sunday.Core
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
