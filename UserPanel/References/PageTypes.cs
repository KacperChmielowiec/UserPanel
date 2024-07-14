using UserPanel.Attributes;

namespace UserPanel.References
{
    public enum PageTypes
    {
        [StringValue("Home")]
        HOME,
        [StringValue("Campaign")]
        CAMP,
        [StringValue("Campaigns")]
        CAMPS,
        [StringValue("Other")]
        OTHER,
        [StringValue("Campaigns List")]
        LIST_CAMPS,
        [StringValue("Campaign Details")]
        CAMP_DETAILS,
        [StringValue("Groups")]
        GROUPS,
    }

    public enum DataType
    {
        User,
        Campaning,
        Group,
        Advert
    }
}
