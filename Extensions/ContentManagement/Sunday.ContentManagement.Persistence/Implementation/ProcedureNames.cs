﻿namespace Sunday.ContentManagement.Persistence.Implementation
{
    internal class ProcedureNames
    {
        public class Layout
        {
            public const string Search = "sp_layouts_search";
            public const string Create = "sp_layouts_insert";
            public const string Update = "sp_layouts_update";
            public const string Delete = "sp_layouts_delete";
        }
        public class Websites
        {
            public const string Search = "sp_websites_search";
            public const string Create = "sp_websites_insert";
            public const string Update = "sp_websites_update";
            public const string Delete = "sp_websites_delete";
            public const string GetById = "sp_websites_getById";
            public const string GetByHostName = "sp_websites_getByHostName";
        }
        public class Templates
        {
            public const string Search = "sp_templates_search";
            public const string Save = "sp_templates_save";
            public const string SaveProperties = "sp_templates_saveProperties";
            public const string Delete = "sp_templates_delete";
            public const string GetById = "sp_templates_getById";
        }

        public class Contents
        {
            public const string GetByParents = "sp_contents_getByParents";
            public const string Create = "sp_content_create";
            public const string Update = "sp_content_update";
            public const string UpdateContentExplicit = "sp_content_updateContentExplicit";
            public const string Delete = "sp_content_delete";
            public const string GetById = "sp_contents_getById";
            public const string NewVersion = "sp_content_newVersion";
            public const string Publish = "sp_content_publish";
            public const string GetMultiples = "sp_contents_getMultiples";
            public const string SaveOrders = "sp_contents_saveOrders";
            public const string GetOrders = "sp_contents_getOrders";
        }
        public class Renderings
        {
            public const string Search = "sp_renderings_search";
            public const string CreateOrUpdate = "sp_renderings_createOrUpdate";
            public const string Delete = "sp_renderings_delete";
            public const string GetById = "sp_renderings_getById";
        }
        public class ContentLinks
        {
            public const string Save = "sp_contentLinks_save";
            public const string GetReferencesTo = "sp_contentLinks_getReferenceTo";
            public const string GetReferencesFrom = "sp_contentLinks_getReferenceFrom";
        }
    }
}
