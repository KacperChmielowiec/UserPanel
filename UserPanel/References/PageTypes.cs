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
        [StringValue("Adverts List")]
        ADVERT_LIST,
        [StringValue("Feed List")]
        FEEDS,
        [StringValue("Product List")]
        PRODUCTS,
        [StringValue("Dasboard")]
        DASHBOARD,
        [StringValue("Dasboard")]
        DASHBOARD_CREATE_USER,
        [StringValue("Dasboard Settings")]
        DASHBOARD_SETTINGS,

    }

    public enum DataType
    {
        User,
        Campaning,
        Group,
        Advert
    }
}
