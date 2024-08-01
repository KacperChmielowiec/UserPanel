using NuGet.Protocol;
using UserPanel.Types;
namespace UserPanel.Models.Feed
{
    public class Feed
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Url { get; set; }
        public FeedTypes FeedType { get; set; } = FeedTypes.Ceneo;
    }
}
