using UserPanel.Helpers;

namespace UserPanel.Services
{
    public class AdvertManager
    {
        public AdvertManager() { }


        public string UpdateAdvertImage(Guid id, string format, IFormFile File)
        {
            string path = PathGenerate.GetAdvertPath(id, format);

            var FormFileService = new FormFileService(File);
            FormFileService.WriteFile(path);

            return FormFileService.GetRelativePath();
        }
    }
}
