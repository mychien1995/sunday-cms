using System;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Contents
{
    public class CreateContentJsonResult : BaseApiResponse
    {
        public Guid Id { get; set; }

        public CreateContentJsonResult(Guid id)
        {
            Id = id;
        }
    }
}
