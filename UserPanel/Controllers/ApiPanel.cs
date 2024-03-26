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

            var list = DataBaseProvider
                .GetGroupStatRepository()
                .getGroupStatByCampId(id);

            if(list == null || list.Count == 0) return NotFound();

            list.Sort((x, y) => x.Budged.Length < y.Budged.Length ? 1 : -1);

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

            return Ok(new CampaningStat(campStat.Id_Camp, campStat.Visit, campStat.Clicks, campStat.Budged));
        }

        // GET api/<ApiPanel>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ApiPanel>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ApiPanel>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApiPanel>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
