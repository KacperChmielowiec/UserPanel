﻿namespace UserPanel.Models.Group
{
    public class Advertisement
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string template { get; set; }
        public string[] formats { get; set; }
        public bool state { get; set; }
    }
}