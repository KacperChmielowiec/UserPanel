using UserPanel.Attributes;

namespace UserPanel.Models.Group
{
    public enum BillingModel
    {
        [StringValue("CPM")]
        CPM,
        [StringValue("CPC")]
        CPC,
        [StringValue("CPS")]
        CPS
    }
}
