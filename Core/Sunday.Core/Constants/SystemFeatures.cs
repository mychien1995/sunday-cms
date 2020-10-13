namespace Sunday.Core.Constants
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

        public static class OrganizationProfileManagement
        {
            public const string Code = SystemModules.OrganizationProfileManagement.Code;
            public static class UpdateProfile
            {
                public const string DisplayName = "Update Profile";
                public const string Code = "OPM_U";
            }
        }
    }
}
