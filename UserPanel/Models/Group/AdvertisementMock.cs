namespace UserPanel.Models.Group
{
    public class AdvertisementMock
    {
        public int id_user {  get; set; }
        public Guid id_camp {get; set;}
        public Guid id_group { get; set;}
        public Guid id { get; set; }
        public string name { get; set; }
        public TemplateAds template { get; set; }
        public string[] formats { get; set; }
        public bool status { get; set; }
    }
}
