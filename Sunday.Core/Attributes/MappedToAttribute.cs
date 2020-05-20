using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunday.Core
{
    public class MappedToAttribute : Attribute
    {
        public List<Type> MappedType { get; set; }
        public bool TwoWay { get; set; }
        public MappedToAttribute(Type mappedType, bool twoWay = true)
        {
            if (mappedType != null)
                MappedType = new List<Type>() { mappedType };
            this.TwoWay = twoWay;
        }
        public MappedToAttribute(Type[] mappedTypes, bool twoWay = true)
        {
            if (mappedTypes != null)
                MappedType = mappedTypes.ToList();
            this.TwoWay = twoWay;
        }
    }
}
