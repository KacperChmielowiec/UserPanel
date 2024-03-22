using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserPanel.Controllers
{
    [Route("api/panel")]
    [ApiController]
    public class ApiPanel : ControllerBase
    {
        // GET: api/<ApiPanel>
        [HttpGet]
        public IEnumerable<string> GetViews()
        {
            return new string[] { "value1", "value2" };
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
