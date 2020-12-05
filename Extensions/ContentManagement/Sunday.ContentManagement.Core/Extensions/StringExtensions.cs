namespace Sunday.ContentManagement.Extensions
{
    public static class StringExtensions
    {
        public static string FormalizeAsContentPath(this string path)
        => path.Trim().Trim('/').Trim('\\').Replace('\\', '/').ToLower();
    }
}
