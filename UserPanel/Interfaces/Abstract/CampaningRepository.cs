namespace UserPanel.Interfaces.Abstract
{
    public abstract class CampaningRepository<T>
    {
        public abstract T GetCampaningById(Guid id);
        public abstract List<T> GetCampaningsByUser(int userId);
        public abstract void UpdateCampaningById(T model, int userId);
        public abstract void CreateCampaning(T model, int userId);
        public abstract void DeleteCampaning(Guid id);

    }
}
