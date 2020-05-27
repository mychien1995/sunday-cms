using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core
{
    public class MappedUsingAttribute : Attribute
    {
        public Type ConverterType { get; set; }
        public Type ReverseConverterType { get; set; }
        public MappedUsingAttribute(Type converterType, Type reverseType = null)
        {
            this.ConverterType = converterType;
            this.ReverseConverterType = reverseType;
        }
    }
}
