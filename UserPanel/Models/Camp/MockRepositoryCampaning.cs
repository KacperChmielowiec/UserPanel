using AutoMapper;
using System.Data;
using UserPanel.Models.Camp;
using UserPanel.Interfaces.Abstract;
using UserPanel.Helpers;
using UserPanel.References;

namespace UserPanel.Models.User
{
    public class MockRepositoryCampaning : CampaningRepository<Campaning>
    {
        private readonly string PathCamp = SessionKeysReferences.campsKey;
        private ISession _session;
        private IMapper _mapper;
        public MockRepositoryCampaning(ISession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;
        }

        public override void CreateCampaning(Campaning model, int userId)
        {
            if (model == null) return;
            var SessionCampList = _session.GetJson<List<CampaningMock>>(PathCamp) ?? new List<CampaningMock>();

            CampaningMock CampaningMock = _mapper.Map<CampaningMock>(model);
            CampaningMock.FK_User = userId;

            SessionCampList.Add(CampaningMock);
            _session.SetJson(PathCamp, SessionCampList);

            Subjects.dataActionSubject.notify(new Services.observable.DataActionMessage() { actionType = Types.DataActionType.ADD, dataType = DataType.Campaning, id = model.id });
        }

        public override void DeleteCampaning(Guid id)
        {
            var SessionCampList = _session.GetJson<List<CampaningMock>>(PathCamp) ?? new List<CampaningMock>();
            int elements = SessionCampList.RemoveAll(model => model.id == id);
            if (elements > 0)
            {
                _session.SetJson(PathCamp, SessionCampList);
                Subjects.dataActionSubject.notify(new Services.observable.DataActionMessage() { actionType = Types.DataActionType.REMOVE, dataType = DataType.Campaning, id = id });
            }
            else
            {
                throw new KeyNotFoundException("W bazie danych nie znaleziono kampanii o takim Guid");
            }
        }

        public override Campaning? GetCampaningById(Guid id)
        {
            var SessionCamp = _session
              .GetJson<List<CampaningMock>>(PathCamp)?
              .Where(c => c.id == id)?
              .FirstOrDefault();

            if (SessionCamp != null)
            {
                return _mapper.Map<Campaning>(SessionCamp);
            }

            return null;   
        }

        public override List<Campaning>? GetCampaningsByUser(int userId)
        {
            var SessionCampList = _session.GetJson<List<CampaningMock>>(PathCamp).Where(c => c.FK_User == userId).ToList() ?? new List<CampaningMock>();
            if (SessionCampList.Count == 0) return new List<Campaning>();
            return _mapper.Map<List<CampaningMock>,List<Campaning>>(SessionCampList);
        }
        
        public override void UpdateCampaningById(Campaning model, int userId)
        {
            if (model == null) return;
            if (model.id == null) return;

            var SessionCampList = _session.GetJson<List<CampaningMock>>(PathCamp) ?? new List<CampaningMock>();
            int elements = SessionCampList.RemoveAll(m => m.id == model.id );

            if(elements == 0) return;

            CampaningMock campaningMock = _mapper.Map<CampaningMock>(model);
            campaningMock.FK_User = userId;

            SessionCampList.Add(campaningMock);

            _session.SetJson(PathCamp, SessionCampList);

            Subjects.dataActionSubject.notify(new Services.observable.DataActionMessage() { actionType = Types.DataActionType.UPDATE, dataType = DataType.Campaning });
        }

    }
}
