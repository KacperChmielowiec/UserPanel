using System.ComponentModel.DataAnnotations;

namespace UserPanel.Models.Adverts
{
    public class AdvertGroupEdit
    {
        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Required]
        public bool IsAttached { get; set; }

        public AD_TEMPLATE Template { get; set; }

        public DateTime ModifiedTime { get; set; }

        public bool isActive { get; set; }

        public List<string> Formats { get; set; }

    }
    public class AdvertGroupListView
    {
        [Required]
        public Guid Id_Group { get; set; }
        [Required]
        public string Name_Group { get; set; }
        public List<AdvertGroupEdit> AdvertGroups { get; set; }
    }
}
