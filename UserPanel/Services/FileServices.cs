using UserPanel.Helpers;
using UserPanel.References;

namespace UserPanel.Services
{
    public static class FileServices
    {
       private static string BASE_PATH = AppReferences.BASE_APP_PATH;
       public static byte[] ReadFile(string path, string BasePath =  "")
       {
            if(!File.Exists(BASE_PATH + path)) return new byte[0];
            if (BasePath == "") BasePath = BASE_PATH;
            using FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[fileStream.Length];
            int total = 0;
            while (total < fileStream.Length)
            {
                int read = fileStream.Read(bytes, 0, bytes.Length);
                total += read;
                if (read == 0)
                {
                    break;
                }
            }
            return bytes;
       }

        public static bool WriteFile(string path, byte[] buffor, string BasePath = "")
        {
            if (BasePath == "") BasePath = BASE_PATH;
            using FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Write);
            fileStream.Write(buffor, 0, buffor.Length);
            return true;
        }
        public static bool WriteFile(string path, MemoryStream memoryStream, string BasePath = "")
        {
            if (BasePath == "") BasePath = BASE_PATH;
            string fullPath = Path.GetFullPath(Path.Combine(BasePath, path));
            using FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.CopyTo(fileStream,(int)memoryStream.Length);
            return true;
        }
        public static string GetSafeFilename(string filename)
        {

            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
