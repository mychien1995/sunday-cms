using System;
using System.Collections.Generic;
using System.Linq;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Models.Fields;

namespace Sunday.ContentManagement.Extensions
{
    public static class ContentExtensions
    {
        public static Guid[] IdListValue(this Content content, string fieldName)
            => ValueFor<List<string>>(content, fieldName, new List<string>(), FieldTypes.Multilist).Select(Guid.Parse).ToArray();

        public static Guid? IdValue(this Content content, string fieldName)
            => ValueFor<Guid?>(content, fieldName, null, FieldTypes.DropTree);
        public static GeneralLinkValue? LinKValue(this Content content, string fieldName)
            => ValueFor<GeneralLinkValue>(content, fieldName, null!, FieldTypes.Link);
        public static string? TextValue(this Content content, string fieldName)
            => ValueFor<string>(content, fieldName, null!, FieldTypes.RichText, FieldTypes.MultilineText, FieldTypes.SingleLineText);

        public static string? BlobUriValue(this Content content, string fieldName)
            => ValueFor<string>(content, fieldName, null!, FieldTypes.Image);

        public static RenderingAreaValue? RenderingAreaValue(this Content content, string fieldName)
            => ValueFor<RenderingAreaValue>(content, fieldName, null!, FieldTypes.RenderingArea);

        private static T ValueFor<T>(Content content, string fieldName, T defaultValue, params FieldTypes[] fieldTypes)
        {
            var field = content[fieldName];
            if (field == null) return defaultValue;
            var fieldType = (FieldTypes)field.TemplateFieldCode;
            if (fieldTypes.Contains(fieldType))
            {
                return (T)field.FieldValue!;
            }

            return defaultValue;
        }
    }
}
