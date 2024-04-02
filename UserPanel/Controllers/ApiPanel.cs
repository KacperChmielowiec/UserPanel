using Microsoft.AspNetCore.Mvc;
using UserPanel.Interfaces;
using UserPanel.Models.Camp;
namespace UserPanel.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiPanel : ControllerBase
    {
        IDataBaseProvider DataBaseProvider;
        public ApiPanel(IDataBaseProvider dataBaseProvider)
        {
            this.DataBaseProvider = dataBaseProvider;
        }
        [HttpGet("campstat/{id}")]
        public IActionResult GetCampaningStatistic(Guid id)
        {
            if(id == Guid.Empty)
            {
                return NotFound();
            }

            var campStat = GetStat(id);
           
            if(campStat == null) return NotFound();

            return Ok(campStat);
        }

        [HttpPost("campstats")]
        public IActionResult GetListCampaningStatistic([FromBody] Guid[] ids)
        {
            if(ids?.Count() == 0 || ids?.Count() == null) {
                return NotFound();
            }
            List<CampaningStat> campaningStats = new List<CampaningStat>();
            foreach (var id in ids)
            {
                var buffor = GetStat(id);
                if(buffor == null) continue;
                campaningStats.Add(buffor);
            }

            if(campaningStats.Count > 0)
                return Ok(campaningStats);
            return NotFound();
        }

        private CampaningStat GetStat(Guid id)
        {
            var list = DataBaseProvider
               .GetGroupStatRepository()
               .getGroupStatByCampId(id);

            string name = DataBaseProvider.GetCampaningRepository().getCampaningById(id)?.name;

            if (list == null || list.Count == 0) return null;

            var campStat = list.Aggregate((acc, curr) =>
            {
                acc.Visit = acc.Visit.Zip(curr.Visit, (x, y) => x += y)
                    .Concat(acc.Visit.Skip(curr.Visit.Length))
                    .Concat(curr.Visit.Skip(acc.Visit.Length))
                    .ToArray();

                acc.Clicks = acc.Clicks.Zip(curr.Clicks, (x, y) => x += y)
                    .Concat(acc.Clicks.Skip(curr.Clicks.Length))
                    .Concat(curr.Clicks.Skip(acc.Clicks.Length))
                    .ToArray();

                acc.Budged = acc.Budged.Zip(curr.Budged, (x, y) => x += y)
                    .Concat(acc.Budged.Skip(curr.Budged.Length))
                    .Concat(curr.Budged.Skip(acc.Budged.Length))
                    .ToArray();

                return acc;
            });

            return new CampaningStat(campStat.Id_Camp, campStat.Visit, campStat.Clicks, campStat.Budged,name);
        }
    }
}
