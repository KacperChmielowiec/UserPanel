using System.Security.Claims;
using UserPanel.Helpers;
using UserPanel.Interfaces;
using UserPanel.Models.Camp;
using UserPanel.Providers;

namespace UserPanel.Services
{
    public class CampaningManager
    {
        private IDataBaseProvider _provider;
        private IConfiguration _configuration;
        private IHttpContextAccessor _contextAccessor;
     
        public CampaningManager(IDataBaseProvider provider, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
        public void UpdateCampaning(Campaning model)
        {
            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated || _contextAccessor.HttpContext.User.FindFirst("Id").Value == null) return;
            int id = int.Parse(_contextAccessor.HttpContext.User.FindFirst("Id").Value);
            if (model.FK_User != id) return;

            _provider.GetCampaningRepository().UpdateCampaningById(model.id,model);

        }
        public void CreateCampaning(CreateCampaning model)
        {
            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated || _contextAccessor.HttpContext.User.FindFirst("Id").Value == null) return;
            int id = int.Parse(_contextAccessor.HttpContext.User.FindFirst("Id").Value);
            Campaning campaning = new Campaning();
            campaning.id = Guid.NewGuid();
            campaning.status = false;
            campaning.FK_User = id;
            campaning.name = model.name;
            campaning.website = model.url;
            campaning.details = new DetailsCampaning();
            campaning.details.EmailNotify = true;
            campaning.details.logo = model.logo.FileName;
            campaning.details.Currency = model.currency;
            campaning.details.Country = model.country;
            campaning.details.CampaningFlags = new CampaningFlags()
            {
                Advert = CampaningFlagState.Inactive,
                Display = CampaningFlagState.Inactive,
                Budget = CampaningFlagState.Inactive,
                Lists = CampaningFlagState.Inactive,
                Products = CampaningFlagState.Inactive,

            };
            campaning.details.Utm_Medium = "cpm";
            campaning.details.Utm_Source = "panel";
            campaning.budget = new BudgetCampaning()
            {
                dayBudget = 0,
                dayBudgetLeft = 0,
                totalBudget = 0,
                totalBudgetLeft = 0,
            };
            _provider.GetCampaningRepository().CreateCampaning(campaning);

        }
        public void DeleteCampaning(Guid id)
        {
            _provider.GetCampaningRepository().DeleteCampaning(id);
        }
    }
}
