namespace UserPanel.Models.Product
{
    public class ProductListModelView
    {
        public Guid id_camp {  get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

    }
}
