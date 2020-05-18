using DbUp;
using Microsoft.Extensions.Configuration;
using Sunday.Core.DataAccess.Database;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Sunday.Core.DataAccess.Pipelines.Initialization
{
    public class UpdateDatabaseSchema
    {
        private readonly IConfiguration _configuration;
        private readonly StoredProcedureRunner _storedProcedureRunner;
        public UpdateDatabaseSchema(IConfiguration configuration, StoredProcedureRunner storedProcedureRunner)
        {
            _configuration = configuration;
            _storedProcedureRunner = storedProcedureRunner;
        }

        public void Process(PipelineArg arg)
        {
            var connectionString = _configuration.GetConnectionString("SundayDB");
            var upgrader = DeployChanges.To
                   .SqlDatabase(connectionString)
                   .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                   .LogToConsole()
                   .WithTransactionPerScript()
                   .Build();
            var result = upgrader.PerformUpgrade();
            if (result.Successful)
                _storedProcedureRunner.Execute(ProcedureNames.ClearSchemaVersion);
        }
    }
}
