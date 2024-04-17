using UserPanel.Interfaces;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;

namespace UserPanel.Services
{
    public class GroupManager
    {
        private IDataBaseProvider _provider;
        private IConfiguration _configuration;
        private IHttpContextAccessor _contextAccessor;

        public GroupManager(IDataBaseProvider provider, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this._provider = provider;
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
        }
        public List<GroupModel> GetGroupsByCampID(Guid idCamp)
        {
            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated || _contextAccessor.HttpContext.User.FindFirst("Id").Value == null) return new List<GroupModel>();
            int id = int.Parse(_contextAccessor.HttpContext.User.FindFirst("Id").Value);

            if(_provider.GetCampaningRepository().getCampaningsByUser(id).Where(camp => camp.id == idCamp).FirstOrDefault() != null)
            {
              return  _provider.GetGroupRepository().GetGroupsByCampId(idCamp);
            }
            return new List<GroupModel>();
        }
    }
}
