using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Sunday.Core.Constants;
using Sunday.Core.Pipelines;
using Sunday.DataAccess.SqlServer.Database;

namespace Sunday.Foundation.Implementation.Pipelines.Initialization
{
    public class FeaturesSeeding
    {
        private readonly StoredProcedureRunner _databaseRunner;
        public FeaturesSeeding(StoredProcedureRunner storeProcRunner)
        {
            _databaseRunner = storeProcRunner;
        }
        public void Process(PipelineArg arg)
        {
            var moduleTypes = typeof(SystemFeatures).GetNestedTypes(BindingFlags.Public | BindingFlags.Static |
               BindingFlags.FlattenHierarchy);
            var dbFeatureType = new DataTable("FeatureType");
            dbFeatureType.Columns.Add("Code", typeof(string));
            dbFeatureType.Columns.Add("Name", typeof(string));
            dbFeatureType.Columns.Add("ModuleCode", typeof(string));
            foreach (var type in moduleTypes)
            {
                var moduleCode = type.GetField("Code", BindingFlags.Public | BindingFlags.Static |
                                                       BindingFlags.FlattenHierarchy)!.GetValue(null) + "";
                if (string.IsNullOrEmpty(moduleCode)) continue;
                var featuresTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static |
               BindingFlags.FlattenHierarchy);
                foreach (var featureType in featuresTypes)
                {
                    var code = featureType.GetField("Code", BindingFlags.Public | BindingFlags.Static |
                                                            BindingFlags.FlattenHierarchy)!.GetValue(null) + "";
                    var displayName = featureType.GetField("DisplayName", BindingFlags.Public | BindingFlags.Static |
                                                                          BindingFlags.FlattenHierarchy)!.GetValue(null) + "";
                    if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(displayName)) continue;
                    var row = dbFeatureType.NewRow();
                    row["ModuleCode"] = moduleCode;
                    row["Code"] = code;
                    row["Name"] = displayName;
                    dbFeatureType.Rows.Add(row);
                }
            }
            var moduleTypeParam = new SqlParameter
            {
                ParameterName = "@Features",
                SqlDbType = SqlDbType.Structured,
                Value = dbFeatureType,
                TypeName = "dbo.FeatureType",
                Direction = ParameterDirection.Input
            };
            _databaseRunner.Execute(ProcedureNames.FeatureSeeding, moduleTypeParam).Wait();
        }
    }
}
