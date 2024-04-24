namespace UserPanel.Interfaces.Abstract
{
    public abstract class ManagerValidate<T>
    {
        public abstract bool Validate(T obj);
        public abstract bool TryValidModel(T obj);
    }
}
