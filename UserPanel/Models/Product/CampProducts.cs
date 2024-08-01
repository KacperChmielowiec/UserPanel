namespace UserPanel.Models.Product
{
    public class CampProducts
    {
        public Guid id_camp {  get; set; }

        public Guid id_feed { get; set; }
        public List<Product> Products { get; set; }
    }
}
