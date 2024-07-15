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
        [StringValue("Group Details")]
        GROUP_DETAILS,
    }

    public enum DataType
    {
        User,
        Campaning,
        Group,
        Advert
    }
}
