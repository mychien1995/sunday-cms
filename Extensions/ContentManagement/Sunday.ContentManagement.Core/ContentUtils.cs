using System.Text.RegularExpressions;

namespace Sunday.ContentManagement
{
    public static class ContentUtils
    {
        public static string FormalizeName(string name)
            => Regex.Replace(name.Trim().ToLower(), "[ ](?=[ ])|[^-_,A-Za-z0-9 ]+", "", RegexOptions.Compiled).Replace(" ", "-");

        public static string FormalizeDisplayName(string name)
            => Regex.Replace(name.Trim(), "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }
}