namespace UserPanel.Interfaces.Abstract
{
    public abstract class FeedRepository<T>
    {
        public abstract List<T> GetFeedsByCampId(Guid id);
        public abstract void AddFeedByCampId(T feed, Guid id);

        public abstract void RemoveFeedById(Guid id);
    }
}
