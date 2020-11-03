﻿using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Contents;

namespace Sunday.CMS.Core.Application
{
    public interface IContentTreeManager
    {
        Task<ContentTreeJsonResult> GetRoots();

        Task<ContentTreeItem[]> GetChilds(ContentTreeItem current);
    }
}
