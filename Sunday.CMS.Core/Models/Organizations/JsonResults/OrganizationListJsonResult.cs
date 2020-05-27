using AutoMapper;
using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Framework.Automap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Organizations
{
    public class OrganizationListJsonResult : BaseApiResponse
    {
        public OrganizationListJsonResult() : base()
        {
            Organizations = new List<OrganizationItem>();
        }
        public int Total { get; set; }
        public IEnumerable<OrganizationItem> Organizations { get; set; }
    }

    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationItem : IOrganizationProperties
    {
        public int ID { get; set; }
        public string LogoLink { get; set; }
        public string OrganizationName { get; set; }
        public string CreatedBy { get; set; }
        public long CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public long UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ColorScheme { get; set; }

        public OrganizationItem()
        {
        }
        public string DoSmt()
        {
            short a = 0;
            Test.Do(a);
            return a + "";
        }
        
    }
    public class Test
    {
        public static void Do(long x)
        {
        }
        public static void Do(int x)
        {
        }
   
}
