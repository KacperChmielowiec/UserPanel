using Microsoft.AspNetCore.Http;
using UserPanel.Helpers;
using UserPanel.Interfaces;
using UserPanel.Interfaces.Abstract;
using UserPanel.Models.Camp;
using UserPanel.Services;

namespace UserPanel.Models.Group
{
    public class MockGroupRepository : GroupRepository<GroupModel>
    {
        private ISession _Session;
        private readonly string Path = "appConfig.database.mock.groups";
        private readonly string PathCamp = "appConfig.database.mock.campanings";
        public MockGroupRepository(ISession session) { 
            _Session = session;
        }
        public override List<GroupModel> GetGroupsByCampId(Guid id)
        {
            var curr = _Session.GetJson<List<GroupModel>>("groups")?.Where(group => group.FK_Camp == id).ToList() ?? new List<GroupModel>(); 
            if(curr.Count > 0)
            {
                return curr;
            }
            else
            {
                var group = ConfigManager
                   .GetConfig(Path)
                   .Parse<List<GroupModel>>()
                   .Where(c => c.FK_Camp == id)
                   .ToList();


                if (group != null)
                {
                    curr = _Session.GetJson<List<GroupModel>>("groups") ?? new List<GroupModel>();
                    curr.AddRange(group);
                    _Session.SetJson("groups", curr);
                }
                return group;
            }
        }

        public override List<GroupModel> GetGroupsByUserId(int id)
        {
            List<Campaning> campanings = new List<Campaning>();
            campanings = _Session.GetJson<List<Campaning>>("campanings")?.Where( c => c.FK_User == id).ToList() ?? new List<Campaning>();
            if(campanings.Count == 0 )
            { 
                campanings = ConfigManager
                  .GetConfig(PathCamp)
                  ?.Parse<List<Campaning>>()
                  ?.Where(c => c.FK_User == id)
                  ?.ToList() ?? new List<Campaning>();     
            }

            List<GroupModel> groups = new List<GroupModel>();
            groups = _Session.GetJson<List<GroupModel>>("groups")?.Where( g => campanings.Select(c => c.id).Contains(g.FK_Camp) )?.ToList() ?? new List<GroupModel>();
            if (groups.Count == 0)
            {
                groups = ConfigManager.GetConfig(Path)
                    ?.Parse<List<GroupModel>>()
                    ?.Where(g => campanings.Select(c => c.id).Contains(g.FK_Camp))
                    ?.ToList() ?? new List<GroupModel>();
            }
            return groups;
        }

        public override GroupModel GetGroupById(Guid id)
        {
            var curr = _Session.GetJson<List<GroupModel>>("groups")?.Where(group => group.id == id).FirstOrDefault();
            if (curr != null)
            {
                return curr;
            }
            else
            {
                var group = ConfigManager
                   .GetConfig(Path)
                   .Parse<List<GroupModel>>()
                   .Where(c => c.id == id)
                   .FirstOrDefault();


                if (group != null)
                {
                    var list = _Session.GetJson<List<GroupModel>>("groups") ?? new List<GroupModel>();
                    list.Add(group);
                    _Session.SetJson("groups", curr);
                }
                return group;
            }
        }

        public override void UpdateGroup(Guid id, GroupModel model)
        {
            var curr = _Session.GetJson<List<GroupModel>>("groups") ?? new List<GroupModel>();
            curr.RemoveAll(model => model.id == id);
            curr.Add(model);
            _Session.SetJson("groups", curr);
        }
    }
}
