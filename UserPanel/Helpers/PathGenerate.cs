using System.Text.RegularExpressions;
using UserPanel.References;

namespace UserPanel.Helpers
{
    public static class PathGenerate
    {
        public static string GetAdvertPath(Guid id, string format)
        {
            return $"{AppReferences.ADVERT_PATH}/{id}/{format}";
        }
        public static string ShrinkRoot(string path)
        {
            return $"/{Regex.Replace(path, @"(\/|\\)*wwwroot(\/|\\)*", "")}";
        }
        public static string FormatToHostPath(string path)
        {
            return $"{AppReferences.BASE_APP_HOST}/{Regex.Replace(path, @"^(\/|\\)+", "")}";
        }
    }
}
