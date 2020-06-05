using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.FeatureAccess.Core
{
    public static class SystemFeatures
    {
        public static class UsersManagement
        {
            public const string Code = SystemModules.UsersManagement.Code;
            public static class Read
            {
                public const string DisplayName = "View Users";
                public const string Code = "UM_V";
            }
            public static class Write
            {
                public const string DisplayName = "Create/Edit Users";
                public const string Code = "UM_W";
            }
            public static class Activate
            {
                public const string DisplayName = "Activate/Deactivate Users";
                public const string Code = "UM_A";
            }
            public static class Delete
            {
                public const string DisplayName = "Delete Users";
                public const string Code = "UM_D";
            }
            public static class Administer
            {
                public const string DisplayName = "Change Users Role";
                public const string Code = "UM_AM";
            }
            public static class ResetPassword
            {
                public const string DisplayName = "Reset User Password";
                public const string Code = "UM_R";
            }
        }
        public static class OrganizationsManagement
        {
            public const string Code = SystemModules.OrganizationsManagement.Code;
            public static class Read
            {
                public const string DisplayName = "View Organization";
                public const string Code = "OM_R";
            }
            public static class Write
            {
                public const string DisplayName = "Create/Edit Organization";
                public const string Code = "OM_W";
            }
            public static class Activate
            {
                public const string DisplayName = "Activate/Deactivate Organization";
                public const string Code = "OM_A";
            }
            public static class Delete
            {
                public const string DisplayName = "Delete Organization";
                public const string Code = "OM_D";
            }
        }
        public static class RolesManagement
        {
            public const string Code = SystemModules.RolesManagement.Code;
            public static class Read
            {
                public const string DisplayName = "View Roles";
                public const string Code = "ORM_R";
            }
            public static class Write
            {
                public const string DisplayName = "Create/Edit Roles";
                public const string Code = "ORM_W";
            }
            public static class Activate
            {
                public const string DisplayName = "Activate/Deactivate Roles";
                public const string Code = "ORM_A";
            }
            public static class Delete
            {
                public const string DisplayName = "Delete Roles";
                public const string Code = "ORM_D";
            }
        }
        public static class Dump1
        {
            public const string Code = "DMP";
            public static class Read
            {
                public const string DisplayName = "View Roles";
                public const string Code = "DMP_1";
            }
            public static class Write
            {
                public const string DisplayName = "Create/Edit Roles";
                public const string Code = "DMP_2";
            }
            public static class Activate
            {
                public const string DisplayName = "Activate/Deactivate Roles";
                public const string Code = "DMP_3";
            }
            public static class Delete
            {
                public const string DisplayName = "Delete Roles";
                public const string Code = "DMP_4";
            }
        }
        public static class Dump2
        {
            public const string Code = "DMP1";
            public static class Read
            {
                public const string DisplayName = "View Roles";
                public const string Code = "DMP1_1";
            }
            public static class Write
            {
                public const string DisplayName = "Create/Edit Roles";
                public const string Code = "DMP1_2";
            }
            public static class Activate
            {
                public const string DisplayName = "Activate/Deactivate Roles";
                public const string Code = "DMP1_3";
            }
            public static class Delete
            {
                public const string DisplayName = "Delete Roles";
                public const string Code = "DMP1_4";
            }
        }
        public static class Dump3
        {
            public const string Code = "DMP2";
            public static class Read
            {
                public const string DisplayName = "View Roles";
                public const string Code = "DMP2_1";
            }
            public static class Write
            {
                public const string DisplayName = "Create/Edit Roles";
                public const string Code = "DMP2_2";
            }
            public static class Activate
            {
                public const string DisplayName = "Activate/Deactivate Roles";
                public const string Code = "DMP2_3";
            }
            public static class Delete
            {
                public const string DisplayName = "Delete Roles";
                public const string Code = "DMP2_4";
            }
        }
    }
}
