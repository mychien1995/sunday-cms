using System;

namespace Sunday.DataAccess.SqlServer.Attributes
{
    public enum DbOperation
    {
        All,
        Query,
        Update,
        Create
    }
    public class DapperIgnoreParamAttribute : Attribute
    {
        public DapperIgnoreParamAttribute(DbOperation dbOperation = DbOperation.All)
        {
            DbOperation = dbOperation;
        }

        public DbOperation DbOperation { get; set; }
    }

    public class DapperParamAttribute : Attribute
    {
        public string ParamName { get; set; }

        public DapperParamAttribute(string name)
        {
            ParamName = name;
        }
    }
}
