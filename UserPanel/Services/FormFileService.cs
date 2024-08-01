using System.Text;
using UserPanel.Helpers;
using UserPanel.References;

namespace UserPanel.Services
{
    public class FormFileService
    {
        IFormFile _formFile;
        private string _savedPath = "";
        public FormFileService(IFormFile formFile) {
            _formFile = formFile;
        }
        public string GettFullSavedPath()
        {
            return _savedPath;
        }
        public string GetRelativePath()
        {
            return PathGenerate.ShrinkRoot(_savedPath);
        }
        public bool WriteFile(string path)
        {

            if (_formFile.Length > 0)
            {
                if (!Directory.Exists(path)) { 
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, FileServices.GetSafeFilename(_formFile.FileName)).Replace(@"\", "/");
                using MemoryStream memoryStream = new MemoryStream();
                _formFile.CopyTo(memoryStream);
                FileServices.WriteFile(filePath, memoryStream);
                memoryStream.Close();
                _savedPath = filePath;
                return true;
            }
            return false;
        }
    }
}
