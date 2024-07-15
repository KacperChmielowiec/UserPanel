using AutoMapper;
using Elfie.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using UserPanel.Helpers;
using UserPanel.Interfaces;
using UserPanel.Interfaces.Abstract;
using UserPanel.Models.Adverts;
using UserPanel.Models.Camp;
using UserPanel.References;
using UserPanel.Services;
using UserPanel.Services.observable;
using UserPanel.Types;

namespace UserPanel.Models.Group
{
    public class MockGroupRepository : GroupRepository<GroupModel>
    {
        private ISession _Session;
        private IMapper _mapper;
        private readonly string Path = MockPathsReferences.groupsPath;
        private IHttpContextAccessor _contextAccessor;
  
        public MockGroupRepository(
            ISession session, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor)
        {
            _Session = session;
            _mapper = mapper;
            _contextAccessor = httpContextAccessor;
        }
 
        public override List<GroupModel> GetGroupsByCampId(Guid id)
        {
            List<GroupModel> groupModels = new List<GroupModel>();

            var curr = _Session.GetJson<List<GroupModelMock>>(SessionKeysReferences.groupsKey)
                ?.Where(group => group.id_camp == id)
                .ToList() ?? new List<GroupModelMock>();

            if (!curr.IsNullOrEmpty())
            {
                foreach (var item in curr)
                {
                    groupModels.Add(_mapper.Map<GroupModel>(item));
                }
            }
            return groupModels;
        }

        public override List<GroupModel> GetGroupsByUserId(int id)
        {
            List<GroupModel> groupsModels = new List<GroupModel>();
            var curr = _Session.GetJson<List<GroupModelMock>>(SessionKeysReferences.groupsKey)
                ?.Where( c => c.id_user == id)
                .ToList() ?? new List<GroupModelMock>();

            if(!curr.IsNullOrEmpty())
            {
                foreach (var item in curr)
                {
                    groupsModels.Add(_mapper.Map<GroupModel>(item));
                }
            }
            return groupsModels;
        }

        public override GroupModel GetGroupById(Guid id)
        {
            var curr = _Session.GetJson<List<GroupModelMock>>(SessionKeysReferences.groupsKey)
                ?.Where(group => group.id == id)
                .FirstOrDefault();

            if (curr != null)
            {
                return _mapper.Map<GroupModel>(curr);
            }

            return default(GroupModel); 
        }

        public override void UpdateGroup(Guid id, GroupModel model)
        {
            var SessionModels = _Session.GetJson<List<GroupModelMock>>(SessionKeysReferences.groupsKey)
                ?.Where(g => g.id == id)
                ?.ToList() ?? new List<GroupModelMock>();

            if (!SessionModels.IsNullOrEmpty())
            {
                var UpdateModel = SessionModels.Find(x => x.id == id);

                if (UpdateModel == null)
                {
                    return;
                }

                var mock = _mapper.Map<GroupModel, GroupModelMock>(model, UpdateModel);

                SessionModels.RemoveAll(model => model.id == id);
                SessionModels.Add(mock);
                _Session.SetJson(SessionKeysReferences.groupsKey, SessionModels);
            }
        }

        public override void CreateGroup(Guid id, GroupModel model)
        {

            if(id == Guid.Empty) throw new ArgumentNullException("id");
            var SessionModels = _Session.GetJson<List<GroupModelMock>>(SessionKeysReferences.groupsKey) ?? new List<GroupModelMock>();
            var mockModel = _mapper.Map<GroupModelMock>(model);
            SessionModels.Add(mockModel);
            mockModel.id_camp = id;
            mockModel.id_user = UserManager.getUserId(_contextAccessor);

            _Session.SetJson(SessionKeysReferences.groupsKey, SessionModels);


        }

        public override GroupAdvertJoin GroupJoinAdvert(Guid id)
        {

            if (id == Guid.Empty) throw new ArgumentNullException("Empty Guid in GroupAdvertRelation");
            var AdvertListId = _Session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey).Where(m => m.id_groups.Contains(id)).Select(m => m.id).ToList();

            if (AdvertListId.Count > 0)
            {
                var table = new GroupAdvertJoin();
                table.id_group = id;
                table.one_to_many = AdvertListId;
                return table;
            }

            return null;

        }
    }
}
