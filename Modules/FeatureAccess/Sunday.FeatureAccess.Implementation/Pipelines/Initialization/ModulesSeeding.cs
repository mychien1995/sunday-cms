using Sunday.Core;
using Sunday.DataAccess.SqlServer;
using Sunday.FeatureAccess.Core;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Sunday.Core.Constants;
using Sunday.Core.Pipelines;
using Sunday.DataAccess.SqlServer.Database;

namespace Sunday.FeatureAccess.Implementation.Pipelines.Initialization
{
    public class ModulesSeeding
    {
        private readonly StoredProcedureRunner _databaseRunner;
        public ModulesSeeding(StoredProcedureRunner storeProcRunner)
        {
            _databaseRunner = storeProcRunner;
        }
        public void Process(PipelineArg arg)
        {
            var moduleTypes = typeof(SystemModules).GetNestedTypes(BindingFlags.Public | BindingFlags.Static |
               BindingFlags.FlattenHierarchy);
            var dbModuleType = new DataTable("ModuleType");
            dbModuleType.Columns.Add("Code", typeof(string));
            dbModuleType.Columns.Add("Name", typeof(string));
            foreach (var type in moduleTypes)
            {
                var row = dbModuleType.NewRow();
                var code = type.GetField("Code", BindingFlags.Public | BindingFlags.Static |
               BindingFlags.FlattenHierarchy).GetValue(null) + "";
                var displayName = type.GetField("DisplayName", BindingFlags.Public | BindingFlags.Static |
               BindingFlags.FlattenHierarchy).GetValue(null) + "";
                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(displayName)) continue;
                row["Code"] = code;
                row["Name"] = displayName;
                dbModuleType.Rows.Add(row);
            }
            var moduleTypeParam = new SqlParameter
            {
                ParameterName = "@Modules",
                SqlDbType = SqlDbType.Structured,
                Value = dbModuleType,
                TypeName = "dbo.ModuleType",
                Direction = ParameterDirection.Input
            };
            _databaseRunner.Execute(ProcedureNames.ModuleSeeding, moduleTypeParam);
        }
    }
}
