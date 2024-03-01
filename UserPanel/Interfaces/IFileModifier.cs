namespace UserPanel.Interfaces
{
    public interface IFileModifier<T>
    {
        string Name { get; }
        public T ProccesFile(T arg);

    }
}
