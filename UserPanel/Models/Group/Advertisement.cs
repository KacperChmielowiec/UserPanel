namespace UserPanel.Models.Group
{
    public class Advertisement
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public TemplateAds template { get; set; }
        public string[] formats { get; set; }
        public bool status { get; set; }
    }
}
