namespace UserPanel.Models.Adverts
{
    public class AdvertGroupJoin
    {
        public Guid id_advert { get; set; }
        public List<Guid> one_to_many { get; set; }
    }
}
