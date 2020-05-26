using Newtonsoft.Json;
using Sunday.Core.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Sunday.Core.DataAccess.Models.Organizations
{
    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationEntity : ApplicationOrganization
    {
        public string ExtraProperties { get; set; }
        public string Hosts { get; set; }
        public override Dictionary<string, object> Properties
        {
            get
            {
                return !string.IsNullOrEmpty(ExtraProperties) ? JsonConvert.DeserializeObject<Dictionary<string, object>>(ExtraProperties)
                : new Dictionary<string, object>();
            }
            set
            {
                base.Properties = value;
            }
        }
        public override List<string> HostNames
        {
            get
            {
                return !string.IsNullOrEmpty(Hosts) ? Hosts.Split('|').ToList() : new List<string>();
            }
            set
            {
                base.HostNames = value;
            }
        }
    }
}
