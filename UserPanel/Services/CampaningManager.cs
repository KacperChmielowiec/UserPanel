using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserPanel.Helpers;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;
using UserPanel.Providers;
using UserPanel.References;

namespace UserPanel.Services
{
    public class CampaningManager
    {
        private IDataBaseProvider _provider;
        private IConfiguration _configuration;
        private IHttpContextAccessor _contextAccessor;
        private UserManager _userManager;
        private ISession session;
        private PermissionContext _permissionContext;
        private static string CAMP_PATH = AppReferences.CAMP_LOGO_PATH;
        public CampaningManager(
            IDataBaseProvider provider,
            IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor,
            UserManager userManager,
            PermissionContext permissionContext
        )
        {
            _provider = provider;
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
            _userManager = userManager;
            _permissionContext = permissionContext;
            session = httpContextAccessor.HttpContext.Session;
        }

        public List<Campaning> GetCampanings()
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return new List<Campaning>();
            int id = _userManager.getUserId();
            return _provider.GetCampaningRepository().getCampaningsByUser(id);
        }
        public Campaning? GetCampaningById(Guid id)
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return default(Campaning);
            if (!_permissionContext.CampIsAllowed(id)) return default(Campaning);

            return _provider.GetCampaningRepository().getCampaningById(id);
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
            model.FK_User = id;

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
            WriteLogoCampaning(model.logo, campaning.id.ToString());
            _provider.GetCampaningRepository().CreateCampaning(campaning);

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
