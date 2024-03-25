using UserPanel.Models.Campaning;
namespace UserPanel.Models.Home
{
    public class HomeModel
    {
        public FilterParametr FilterParametr { get; set; }
        public List<Campaning.Campaning>? campaningsUser { get; set; }
    }
}
