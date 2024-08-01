using UserPanel.Attributes;

namespace UserPanel.Models.Product
{
    public enum FormatSize
    {
         [StringValue("300x300")]
         size_300_300,
         [StringValue("300x600")]
         size_300_600,
    }
    public class RenderModel
    {
        public Guid camp_id { get; set; }
        public string LogoSrc { get; set; }
        public string MainUrl { get; set; }
        public string Size { get; set; }

        public List<Product> Products { get; set; }
    }
}
