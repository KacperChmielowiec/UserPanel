namespace UserPanel.Models.Feed
{
    public class FeedsListViewModel
    {
       public Guid id_camp {  get; set; }
       public List<Feed> Feeds { get; set; } = new List<Feed>();
    }
}
