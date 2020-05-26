using Newtonsoft.Json;
using Sunday.Core.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.DataAccess.Pipelines.Organizations
{
    public class TranslateToSqlParam
    {
        public async Task ProcessAsync(PipelineArg arg)
        {
            var org = arg["organization"] as ApplicationOrganization;
            var input = arg["input"];
            if (input == null)
            {
                input = new
                {
                    org.OrganizationName,
                    org.CreatedBy,
                    org.CreatedDate,
                    org.Description,
                    org.UpdatedBy,
                    org.UpdatedDate,
                    org.IsActive,
                    org.LogoBlobUri,
                    HostNames = string.Join('|', org.HostNames.Where(x => !string.IsNullOrEmpty(x))),
                    Properties = org.Properties != null ? JsonConvert.SerializeObject(org.Properties) : ""
                };
                arg["input"] = input;
            }
        }
    }
}
