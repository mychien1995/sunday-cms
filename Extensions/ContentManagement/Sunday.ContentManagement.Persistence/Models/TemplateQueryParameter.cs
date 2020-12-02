using System;
using Sunday.ContentManagement.Models;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.DataAccess.SqlServer.Attributes;
using Sunday.DataAccess.SqlServer.Extensions;

namespace Sunday.ContentManagement.Persistence.Models
{
    [MappedTo(typeof(TemplateQuery))]
    public class TemplateQueryParameter : PagingCriteria
    {
        public bool? IsAbstract { get; set; }
        public string Text { get; set; } = string.Empty;
        [DapperIgnoreParam] 
        public string[] IncludeIds { get; set; } = Array.Empty<string>();

        public string IdList => IncludeIds.ToDatabaseList();
    }
}
