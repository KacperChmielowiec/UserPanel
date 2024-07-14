using UserPanel.Helpers;
using UserPanel.Interfaces;
using UserPanel.Models.Camp;
using UserPanel.References;

namespace UserPanel.Services
{
    public class CampaningManager
    {
        private IDataBaseProvider _provider;
        private UserManager _userManager;
        private ISession _session;
        private static string CAMP_PATH = AppReferences.CAMP_LOGO_PATH;
        private static string CAMP_SESSION_PATH = SessionKeysReferences.campsKey;
        public CampaningManager(
            IDataBaseProvider provider,
            IHttpContextAccessor httpContextAccessor,
            UserManager userManager
        )
        {
            _provider = provider;
            _userManager = userManager;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public List<Campaning> GetCampanings()
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return new List<Campaning>();
            int id = _userManager.getUserId();
            return _provider.GetCampaningRepository().GetCampaningsByUser(id);
        }
        public Campaning? GetCampaningById(Guid id)
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return null;
        
            return _provider.GetCampaningRepository().GetCampaningById(id);
        }
        public void SetCampaningSession(Guid id)
        {
            Campaning campaning = _provider
                .GetCampaningRepository()
                .GetCampaningById(id);

            List<Campaning> campaningsSession = _session.GetJson<List<Campaning>>(AppReferences.SessionCamp) ?? new List<Campaning>();

            if (campaningsSession.FirstOrDefault(item => item.id == id) == null)
                campaningsSession.Add(campaning);

            _session.SetJson(AppReferences.SessionCamp, campaningsSession);
        }
        public void UpdateCampaning(Campaning model)
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return; 
            if(model.id == Guid.Empty) return;

            int id = _userManager.getUserId();
            var camps = GetCampanings().Where(c => c.id == model.id).ToArray();
            if (camps.Count() == 0) return;


            if(model.details.logo == null)
            {
                model.details.logo = camps[0].details.logo;
            }
            if(model.details.CampaningFlags == null)
            {
                model.details.CampaningFlags = camps[0].details.CampaningFlags;
            }

            _provider.GetCampaningRepository().UpdateCampaningById(model,id);

        }
        public void CreateCampaning(CreateCampaning model)
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return;

            int id = _userManager.getUserId();
            Campaning campaning = new Campaning();
            campaning.id = Guid.NewGuid();
            campaning.status = false;
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
            WriteLogoCampaning(model.logo, campaning.id.ToString());
            _provider.GetCampaningRepository().CreateCampaning(campaning,id);

        }
        public void DeleteCampaning(Guid id)
        {
            _provider.GetCampaningRepository().DeleteCampaning(id);
        }
        public void WriteLogoCampaning(IFormFile formFile,string campDest)
        {
            FormFileService formFileService = new FormFileService(formFile,AppReferences.BASE_APP_HOST);
            formFileService.WriteFile($"{CAMP_PATH}{FileServices.GetSafeFilename(campDest)}/");
        }
        public static string LoadLogoPath(Campaning campaning, HttpContext httpContext)
        {
            return $"/{AppReferences.CAMP_LOGO_PATH_FETCH}{FileServices.GetSafeFilename(campaning.id.ToString())}/{campaning.details.logo}";
        }
    }
}
