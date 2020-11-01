using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Sunday.Core
{
    public class MappedToAttribute : Attribute
    {
        public List<Type> MappedType { get; set; } = new List<Type>();
        public bool TwoWay { get; set; }

        public string[] Ignores { get; set; }
        public MappedToAttribute(Type mappedType, bool twoWay = true, params string[] ignores)
        {
            if (mappedType != null)
                MappedType = new List<Type> { mappedType };
            TwoWay = twoWay;
            Ignores = ignores;
        }
    }
}
