using System.Text;
using UserPanel.Interfaces;

namespace UserPanel.Services
{
    public class StringFileService : IFileHandler<string>
    {
        private string fileString = string.Empty;
        private string fileStringM = string.Empty;

        public List<IFileModifier<string>> fileModifiers { get; set; }

        public StringFileService(params IFileModifier<string>[] modifiers)
        {
            this.fileModifiers = modifiers.ToList();
        }
        public string ProcessFile()
        {
            if (fileString != string.Empty)
            {
                fileStringM = fileString;
                foreach (var item in fileModifiers)
                {
                    fileStringM = item.ProccesFile(fileStringM);
                }
            }
            return fileStringM;
        }
        public string ReadFile(string path, string BasePath = "")
        {
            fileString = Encoding.UTF8.GetString(FileServices.ReadFile(path));
            return fileString;
        }

        public bool WriteFile(string path, string file, string BasePath = "")
        {
            return FileServices.WriteFile(path, Encoding.UTF8.GetBytes(file), BasePath);
        }
    }
      
    
}
