namespace UserPanel.Interfaces.Abstract
{
    public abstract class CampaningRepository<T>
    {
        public abstract T getCampaningById(Guid id);
        public abstract List<T> getCampaningsByUser(int userId);

    }
}
