﻿using UserPanel.Models.Camp;

namespace UserPanel.Models.Home
{
    public enum ButtonFilterRate
    {
        RATE_1 = 7,
        RATE_2 = 14, 
        RATE_3 = 30,
    }
    public class FilterParametr
    {
       public ButtonFilterRate rate;
       public List<Campaning> FilterCampanings { get; set; }
    }
}
