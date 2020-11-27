﻿using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;

namespace Sunday.ContentDelivery.Core.Services
{
    public interface IContentReader
    {
        public Task<Content[]> GetWebsiteRoots(Guid websiteId);

        public Task<Option<Content>> GetContentByNamePath(Guid websiteId, string path);
    }
}
