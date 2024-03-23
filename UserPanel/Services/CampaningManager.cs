using System.Security.Claims;
using UserPanel.Helpers;
using UserPanel.Models.Campaning;
using UserPanel.Providers;

namespace UserPanel.Services
{
    public class CampaningManager
    {
        private DataBaseProvider _provider;
        private IConfiguration _configuration;
        private IHttpContextAccessor _contextAccessor;
     
        public CampaningManager(DataBaseProvider provider, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this._provider = provider;
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
        }

        public List<Campaning> GetCampanings()
        {
            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated || _contextAccessor.HttpContext.User.FindFirst("Id").Value == null) return new List<Campaning>();
            int id =  int.Parse(_contextAccessor.HttpContext.User.FindFirst("Id").Value);
            return _provider.GetCampaningRepository().getCampaningsByUser(id);
        }
        public void SetCampaningSession(Guid id)
        {
            Campaning campaning = _provider.GetCampaningRepository().getCampaningById(id);
            List<Campaning> campaningsSession = _contextAccessor.HttpContext.Session.GetJson<List<Campaning>>("sessionCamp") ?? new List<Campaning>();
            if(campaningsSession.FirstOrDefault(item => item.id == id ) == null)
                campaningsSession.Add(campaning);
            _contextAccessor.HttpContext.Session.SetJson("sessionCamp", campaningsSession);
        }
    }
}
