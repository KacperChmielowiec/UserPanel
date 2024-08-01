using System.ComponentModel.DataAnnotations;

namespace UserPanel.Models.Adverts
{
    
    public class AdvertForm<T>
    {
        public Guid Id { get; set; }
        [Required]
        public Guid id_camp { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public DateTime Created { get; set; }

        public AD_TEMPLATE Template { get; set; }
        [Required]
        public List<T> Formats { get; set; } = new List<T>();
    }
}
