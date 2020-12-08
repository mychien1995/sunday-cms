using System;

// ReSharper disable once CheckNamespace
namespace Sunday.Core
{
    public class MappedUsingAttribute : Attribute
    {
        public Type ConverterType { get; }
        public Type? ReverseConverterType { get; }
        public MappedUsingAttribute(Type converterType, Type? reverseType = null)
        {
            ConverterType = converterType;
            ReverseConverterType = reverseType;
        }
    }
}
