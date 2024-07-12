using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.Json;
using UserPanel.Interfaces;
using UserPanel.Models.Camp;
using UserPanel.Models;
using System.Drawing;
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
        public IActionResult GetCampaningStatistic(Guid id, [FromBody] GetListCampaningStatisticModel data)
        {
            if(id == Guid.Empty)
            {
                return NotFound();
            }

            if (!DateTime.TryParse(data?.StartDate, out DateTime startTime))
            {
                startTime = DateTime.Now;
            }
            if (!DateTime.TryParse(data?.EndDate, out DateTime endTime))
            {
                endTime = DateTime.Now;
            }


            var campStat = shrinkHelper(GetStat(id),startTime,endTime);
           
            if(campStat == null) return NotFound();

            return Ok(campStat);
        }

        [HttpPost("campstats")]
        public IActionResult GetListCampaningStatistic
        (
           [FromBody] GetListCampaningStatisticModel data
        )
        {
            if(!DateTime.TryParse(data?.StartDate, out DateTime startTime))
            {
                startTime = DateTime.Now;
            }
            if(!DateTime.TryParse(data?.EndDate, out DateTime endTime))
            {
                endTime = DateTime.Now;
            }

            if (data.List?.Count() == 0) {
                return NotFound();
            }
            List<CampaningStat> campaningStats = new List<CampaningStat>();
            foreach (var id in data.List)
            {
                var buffor = shrinkHelper(GetStat(id),startTime,endTime);
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

            string name = DataBaseProvider.GetCampaningRepository().GetCampaningById(id)?.name;

            if (list == null || list.Count == 0) return null;

            var campStat = list.Aggregate((acc, curr) =>
            {
                acc.Visit = acc.Visit.Zip(curr.Visit, (x, y) => new UnitData<int>(x.Value + y.Value, x.Date))
                    .Concat(acc.Visit.Skip(curr.Visit.Length))
                    .Concat(curr.Visit.Skip(acc.Visit.Length))
                    .ToArray();

                acc.Clicks = acc.Clicks.Zip(curr.Clicks, (x, y) => new UnitData<int>(x.Value + y.Value, x.Date))
                    .Concat(acc.Clicks.Skip(curr.Clicks.Length))
                    .Concat(curr.Clicks.Skip(acc.Clicks.Length))
                    .ToArray();

                acc.Budged = acc.Budged.Zip(curr.Budged, (x, y) => new UnitData<decimal>(x.Value + y.Value, x.Date))
                    .Concat(acc.Budged.Skip(curr.Budged.Length))
                    .Concat(curr.Budged.Skip(acc.Budged.Length))
                    .ToArray();

                return acc;
            });

            return new CampaningStat(campStat.Id_Camp, campStat.Visit, campStat.Clicks, campStat.Budged,name);
        }

        private CampaningStat shrinkHelper(CampaningStat campStat, DateTime start, DateTime end)
        {

            if (campStat == null) return null;

            var diff = (start - end).Days;

            campStat.Visit = ConcatUnitArray<int>(campStat.Visit
                .Where(item => DateTime.Parse(item.Date).Date <= start.Date && DateTime.Parse(item.Date).Date >= end.Date)
                .ToArray(), start, end);

            campStat.Clicks = ConcatUnitArray<int>(campStat.Clicks
                .Where(item => DateTime.Parse(item.Date).Date <= start.Date && DateTime.Parse(item.Date).Date >= end.Date)
                .ToArray(), start, end);

            campStat.Budget = ConcatUnitArray<decimal>(campStat.Budget
                .Where(item => DateTime.Parse(item.Date).Date <= start.Date && DateTime.Parse(item.Date).Date >= end.Date)
                .ToArray(),start, end);

            return campStat;

        }

        private UnitData<T>[] FillUnitDataEmpty<T>(int size, DateTime start) where T : struct
        {
            var Empty = new UnitData<T>[size];
            for (var i = 0; i < size; i++)
            {
                Empty[i].Value = default(T);
                Empty[i].Date = start.AddDays(i).ToString("dd-MM-yyyy");
            }
            return Empty;
        }

        private UnitData<T>[] ConcatUnitArray<T>(UnitData<T>[] data, DateTime start, DateTime end) where T : struct
        {
            if (data.Length > 0)
            {

                DateTime first = DateTime.Parse(data.First().Date);
                DateTime last = DateTime.Parse(data.Last().Date);

                var DiffFirst = (first - end).Days;
                var DiffLast = (start - last).Days;

                var suffixArray = FillUnitDataEmpty<T>(DiffFirst, end);
                var prefixArray = FillUnitDataEmpty<T>(DiffLast, last.AddDays(1));

                if (suffixArray.Length > 0)
                {
                    data = suffixArray.Concat(data).ToArray();
                }
                if (prefixArray.Length > 0)
                {
                    data = data.Concat(prefixArray).ToArray();
                }

                return data;
            }

            return FillUnitDataEmpty<T>((start - end).Days, end);
        }

    }
}
