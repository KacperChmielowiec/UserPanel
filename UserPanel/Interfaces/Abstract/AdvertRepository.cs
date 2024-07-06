namespace UserPanel.Interfaces.Abstract
{
    public abstract class AdvertRepository<T>
    {
        public abstract T GetAdvertById(Guid id);
        public abstract List<T> GetAdvertGroupId(Guid id); 
        public abstract void CreateAdvert(T entity);
        public abstract List<T> GetAdvertByUserId(int id);
    }
}
