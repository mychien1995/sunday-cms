﻿namespace Sunday.Foundation.Implementation
{
    internal class ProcedureNames
    {
        public const string ModuleSeeding = "sp_modules_seeding";
        public const string FeatureSeeding = "sp_features_seeding";
        public class Modules
        {
            public const string GetAll = "sp_modules_getAll";
        }

        public class Features
        {
            public const string GetByModules = "sp_features_getByModules";
        }
    }
}
