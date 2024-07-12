using UserPanel.Attributes;

namespace UserPanel.References
{
    public enum PageTypes
    {
        [StringValue("Home")]
        HOME,
        [StringValue("Campaning")]
        CAMP,
        [StringValue("Group")]
        GROUP,
    }

    public enum DataType
    {
        User,
        Campaning,
        Group,
        Advert
    }
}
