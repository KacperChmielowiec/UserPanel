using UserPanel.Models.Camp;

namespace UserPanel.Models.Group
{
    public class GroupDetails
    {
        public BillingModel Billing { get; set; }
        public DevicesModel[] Devices { get; set; }

        public string Utm_Source { get; set; }
        public string Utm_Medium { get; set; }
        public string Utm_Camp { get; set;}
        public CampaningFlags CampaningFlags { get; set; }
    }
}
