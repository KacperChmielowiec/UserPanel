using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserPanel.Helpers;
using UserPanel.Interfaces;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;
using UserPanel.Providers;

namespace UserPanel.Services
{
    public class CampaningManager
    {
        private IDataBaseProvider _provider;
        private IConfiguration _configuration;
        private IHttpContextAccessor _contextAccessor;
        private UserManager _userManager;
        private ISession session;
        public CampaningManager(IDataBaseProvider provider, IConfiguration configuration, IHttpContextAccessor httpContextAccessor,UserManager userManager)
        {
            _provider = provider;
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
            _userManager = userManager;
            session = httpContextAccessor.HttpContext.Session;
        }

        public List<Campaning> GetCampanings()
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return new List<Campaning>();
            int id = _userManager.getUserId();
            return _provider.GetCampaningRepository().getCampaningsByUser(id);
        }
        public void SetCampaningSession(Guid id)
        {
            Campaning campaning = _provider
                .GetCampaningRepository()
                .getCampaningById(id);

            List<Campaning> campaningsSession = session.GetJson<List<Campaning>>("sessionCamp") ?? new List<Campaning>();

            if (campaningsSession.FirstOrDefault(item => item.id == id) == null)
                campaningsSession.Add(campaning);

            session.SetJson("sessionCamp", campaningsSession);
        }
        public void UpdateCampaning(Campaning model)
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return; 

            int id = _userManager.getUserId();
            if (model.FK_User != id) return;

            _provider.GetCampaningRepository().UpdateCampaningById(model.id,model);

        }
        public void CreateCampaning(CreateCampaning model)
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return;

            int id = _userManager.getUserId();
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
