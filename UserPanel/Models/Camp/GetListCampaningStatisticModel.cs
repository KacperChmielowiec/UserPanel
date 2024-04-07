using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace UserPanel.Models.Camp
{
    public class GetListCampaningStatisticModel
    {
        [BindProperty]
        public Guid[]? List {  get; set; }
        [JsonPropertyName("start")]
        public string? StartDate { get; set; }

        [JsonPropertyName("end")]
        public string? EndDate { get; set;}

    }
}
