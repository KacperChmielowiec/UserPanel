namespace UserPanel.Interfaces.Abstract
{
    public abstract class AdvertRepository<T>
    {
        public abstract T GetAdvertById(Guid id);
        public abstract List<T> GetAdvertGroupId(Guid id); 
        public abstract void CreateAdvert(T entity, Guid idGroup);
        public abstract List<T> GetAdvertByUserId(int id);
        public abstract void DeleteAdvertsById(Guid[] ids);
        public abstract void DeleteAdvertsById(Guid id);
        public abstract void DettachAdvertFromGroup(Guid id, Guid group);
        public abstract void ChangeAttachStateAdverts(Guid[] ids, Guid group, bool attach);
        public abstract List<T> GetAdvertsByCampId(Guid id);
        public abstract void UpdateAdvert(T entity);
    }
}
