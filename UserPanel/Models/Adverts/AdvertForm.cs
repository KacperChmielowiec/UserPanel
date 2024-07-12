using System.ComponentModel.DataAnnotations;

namespace UserPanel.Models.Adverts
{
    
    public class AdvertForm
    {
        public Guid Id { get; set; }
        [Required]
        public Guid id_group { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public AD_TEMPLATE Template { get; set; }
        [Required]
        public List<AdvertFormatForm> Formats { get; set; }
    }
}
