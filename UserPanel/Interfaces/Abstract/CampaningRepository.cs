namespace UserPanel.Interfaces.Abstract
{
    public abstract class CampaningRepository<T>
    {
        public abstract T getCampaningById(Guid id);
        public abstract List<T> getCampaningsByUser(int userId);
        public abstract List<T> getCampaningsByUserWithGroups(int userId);
        public abstract void UpdateCampaningById(Guid id, T model);
        public abstract void CreateCampaning(T model);

        public abstract void DeleteCampaning(Guid id);

    }
}
