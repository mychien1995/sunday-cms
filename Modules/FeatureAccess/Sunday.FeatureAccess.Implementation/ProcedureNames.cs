using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.FeatureAccess.Implementation
{
    internal class ProcedureNames
    {
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
