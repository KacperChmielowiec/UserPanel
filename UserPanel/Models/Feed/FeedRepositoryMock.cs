using UserPanel.Helpers;
using UserPanel.Interfaces.Abstract;
using UserPanel.References;

namespace UserPanel.Models.Feed
{
    public class FeedRepositoryMock : FeedRepository<Feed>
    {
        ISession _session;
        public FeedRepositoryMock(ISession session) { 
            _session = session;
        }
        public override void AddFeedByCampId(Feed feed, Guid id)
        {
            if (feed == null) throw new ArgumentNullException("");
            FeedMock mock = new FeedMock();
            mock.id_camp = id;

            mock.Updated = feed.Updated;
            mock.Created = feed.Created;  
            mock.FeedType = feed.FeedType;
            mock.Id = feed.Id;
            mock.Url = feed.Url;

            var FeedList = _session.GetJson<List<FeedMock>>(SessionKeysReferences.feedKey) ?? new List<FeedMock>();

            FeedList.Add(mock);

            _session.SetJson(SessionKeysReferences.feedKey, FeedList);
        }

        public override List<Feed> GetFeedsByCampId(Guid id)
        {
            return _session.GetJson<List<FeedMock>>(SessionKeysReferences.feedKey)?.Where(f => f.id_camp == id)?.Cast<Feed>().ToList() ?? new List<Feed>();
        }

        public override void RemoveFeedById(Guid id)
        {
            var FeedList = _session.GetJson<List<FeedMock>>(SessionKeysReferences.feedKey) ?? new List<FeedMock>(); 
            int elements = FeedList.RemoveAll(x => x.Id == id);
            if(elements == 0)
            {
                throw new KeyNotFoundException();
            }
            _session.SetJson(SessionKeysReferences.feedKey,FeedList);
        }
    }
}
