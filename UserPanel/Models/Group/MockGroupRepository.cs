using Microsoft.AspNetCore.Http;
using UserPanel.Helpers;
using UserPanel.Interfaces.Abstract;
using UserPanel.Models.Camp;
using UserPanel.Services;

namespace UserPanel.Models.Group
{
    public class MockGroupRepository : GroupRepository<GroupModel>
    {
        private ISession _Session;
        private readonly string Path = "appConfig.database.mock.groups";
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
            throw new NotImplementedException();
        }

        public override GroupModel GetGroupById(Guid id)
        {
            var curr = _Session.GetJson<List<GroupModel>>("groups")?.Where(group => group.id == id).FirstOrDefault() ?? new GroupModel();
            if (curr?.id != null)
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

      
    }
}
