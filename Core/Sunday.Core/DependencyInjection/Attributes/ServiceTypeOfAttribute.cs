using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core
{
    public class ServiceTypeOfAttribute : Attribute
    {
        public Type ServiceType;
        public LifetimeScope LifetimeScope;
        public ServiceTypeOfAttribute(Type type, LifetimeScope lifetimeScope = LifetimeScope.Transient)
        {
            ServiceType = type;
            LifetimeScope = lifetimeScope;
        }
    }
}
