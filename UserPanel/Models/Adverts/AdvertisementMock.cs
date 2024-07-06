using UserPanel.Models.Group;

namespace UserPanel.Models.Adverts
{
    public class AdvertisementMock
    {
        public int id_user { get; set; }
        public Guid id_camp { get; set; }
        public Guid id_group { get; set; }
        public Guid id { get; set; }
        public string name { get; set; }
        public AD_TEMPLATE template { get; set; }
        public List<AdvertFormat> formats { get; set; }
        public bool status { get; set; }
    }
}
