namespace UserPanel.Interfaces
{
    public interface IFileHandler<T>
    {
        public List<IFileModifier<T>> fileModifiers { get; set; }
        public T ProcessFile();
        public string ReadFile(string path, string BasePath = "");
        public bool WriteFile(string path, string file, string BasePath = "");
    }
}
