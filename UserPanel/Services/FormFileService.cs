using System.Text;
using UserPanel.References;

namespace UserPanel.Services
{
    public class FormFileService
    {
        IFormFile _formFile;
        string _basePath;
        public FormFileService(IFormFile formFile, string BasePath = "") {

            _formFile = formFile;
            _basePath = BasePath;
        }
    
        public bool WriteFile(string path)
        {
            if (_formFile.Length > 0)
            {
                if (!Directory.Exists(path)) { 
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, _formFile.FileName);
                using MemoryStream memoryStream = new MemoryStream();
                _formFile.CopyTo(memoryStream);
                FileServices.WriteFile(filePath, memoryStream);
                memoryStream.Close();
                return true;
            }
            return false;
        }
    }
}
