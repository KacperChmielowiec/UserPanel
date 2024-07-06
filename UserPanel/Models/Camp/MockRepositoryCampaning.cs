using AutoMapper;
using System.Data;
using UserPanel.Services;
using UserPanel.Models;
using UserPanel.Models.Camp;
using UserPanel.Interfaces.Abstract;
using UserPanel.Helpers;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Security.Cryptography.Xml;
using UserPanel.References;

namespace UserPanel.Models.User
{
    public class MockRepositoryCampaning : CampaningRepository<Campaning>
    {
        private readonly string PathCamp = MockPathsReferences.campaningsPath;
        private ISession _Session;
        public MockRepositoryCampaning(ISession session)
        {
            _Session = session;
        }
            
        public override void CreateCampaning(Campaning model)
        {
            var curr = _Session.GetJson<List<Campaning>>("campanings") ?? new List<Campaning>();
            curr.Add(model);
            _Session.SetJson("campanings", curr);
            Subjects.dataActionSubject.notify(new Services.observable.DataActionMessage() { actionType = Types.DataActionType.ADD, dataType = DataType.Campaning });
        }

        public override void DeleteCampaning(Guid id)
        {
            var curr = _Session.GetJson<List<Campaning>>("campanings") ?? new List<Campaning>();
            curr.RemoveAll(model => model.id == id);
            _Session.SetJson("campanings", curr);
            Subjects.dataActionSubject.notify(new Services.observable.DataActionMessage() { actionType = Types.DataActionType.REMOVE, dataType = DataType.Campaning });
        }

        public override Campaning? getCampaningById(Guid id)
        {
            var campSess = _Session
              .GetJson<List<Campaning>>("campanings")?
              .Where(c => c.id == id)?
              .FirstOrDefault();

            if (campSess != null)
            {
                return campSess;
            }

            var camp = ConfigManager
                .GetConfig(PathCamp)
                .Parse<List<Campaning>>()
                .Where(c => c.id == id)
                .FirstOrDefault();

            if (camp != null)
            {
                var curr = _Session.GetJson<List<Campaning>>("campanings") ?? new List<Campaning>();
                curr.Add(camp);
                _Session.SetJson("campanings", curr);
            }
            return camp;
        }

        public override List<Campaning>? getCampaningsByUser(int userId)
        {
            var UserCamp = getCampaningsByUserWithGroups(userId);
            UserCamp?.ForEach(camp =>
            {
                camp.groups = null;
            });
            return UserCamp;
        }
        
        public override List<Campaning> getCampaningsByUserWithGroups(int userId)
        {
            var campSess = _Session
                .GetJson<List<Campaning>>("campanings")?
                .Where(c => c.FK_User == userId)?
                .ToList() ?? new List<Campaning>();

            if (campSess != null && campSess.Count > 0)
            {
                return campSess;
            }

            var camp = ConfigManager.GetConfig(PathCamp)
                .Parse<List<Campaning>>()
                .Where(c => c.FK_User == userId).ToList();

            if (camp != null && camp.Count > 0)
            {
                var curr = _Session.GetJson<List<Campaning>>("campanings") ?? new List<Campaning>();
                curr.AddRange(camp);
                _Session.SetJson("campanings", curr);
            }

            return camp;
        }

        public override void UpdateCampaningById(Guid id, Campaning model)
        {
            var curr = _Session.GetJson<List<Campaning>>("campanings") ?? new List<Campaning>();
            curr.RemoveAll(model => model.id == id);
            curr.Add(model);
            _Session.SetJson("campanings", curr);
        }

    }
}
