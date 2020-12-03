using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Models.Fields;

namespace Sunday.ContentManagement.Extensions
{
    public static class ContentExtensions
    {
        public static Guid[] IdListValue(this Content content, string fieldName)
            => ValueFor<List<string>>(content, fieldName, FieldTypes.Multilist).Select(Guid.Parse).ToArray();

        public static Guid? IdValue(this Content content, string fieldName)
            => ValueFor<Guid?>(content, fieldName, FieldTypes.DropTree);
        public static GeneralLinkValue? LinKValue(this Content content, string fieldName)
            => ValueFor<GeneralLinkValue>(content, fieldName, FieldTypes.Link);
        public static string? TextValue(this Content content, string fieldName)
            => ValueFor<string>(content, fieldName, FieldTypes.RichText, FieldTypes.MultilineText, FieldTypes.SingleLineText);

        public static string? BlobUriValue(this Content content, string fieldName)
            => ValueFor<string>(content, fieldName, FieldTypes.Image);

        public static RenderingAreaValue? RenderingAreaValue(this Content content, string fieldName)
            => ValueFor<RenderingAreaValue>(content, fieldName, FieldTypes.RenderingArea);

        private static T ValueFor<T>(Content content, string fieldName, params FieldTypes[] fieldTypes)
        {
            var field = content[fieldName];
            if (field == null) return GetDefaultValue<T>();
            var fieldType = (FieldTypes)field.TemplateFieldCode;
            if (fieldTypes.Contains(fieldType))
            {
                return (T)field.FieldValue!;
            }
            return GetDefaultValue<T>();
        }

        private static T GetDefaultValue<T>()
        {
            var isEnumerable = typeof(IEnumerable).IsAssignableFrom(typeof(T));
            if (isEnumerable)
            {
                var innerType = typeof(T).GetGenericArguments()[0];
                var isList = typeof(IList).IsAssignableFrom(typeof(T));
                if (isList)
                {
                    var listType = typeof(List<>);
                    var constructedListType = listType.MakeGenericType(innerType);
                    return (T)Activator.CreateInstance(constructedListType)!;
                }
                return (T)(object)Array.CreateInstance(innerType, 0);
            }
            return default!;
        }
    }
}
