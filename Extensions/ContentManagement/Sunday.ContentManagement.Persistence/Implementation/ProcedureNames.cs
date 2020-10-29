namespace Sunday.ContentManagement.Persistence.Implementation
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
        }
    }
}
