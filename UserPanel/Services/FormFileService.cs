using System.Text;
using UserPanel.References;

namespace UserPanel.Services
{
    public class FormFileService
    {
        IFormFile _formFile;
        string _basePath;
        private string _savedPath = "";
        public FormFileService(IFormFile formFile, string BasePath = "") {

            _formFile = formFile;
            _basePath = BasePath;

        }
        public string GettFullSavedPath()
        {
            return _savedPath;
        }
        public bool WriteFile(string path)
        {
            if (_formFile.Length > 0)
            {
                if (!Directory.Exists(path)) { 
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, FileServices.GetSafeFilename(_formFile.FileName));
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
