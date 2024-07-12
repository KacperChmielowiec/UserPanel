using AutoMapper;
using Microsoft.AspNetCore.Http;
using UserPanel.Helpers;
using UserPanel.Models.Adverts;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;
using UserPanel.Models.User;
using UserPanel.References;

namespace UserPanel.Interfaces.Abstract
{
    public class FullContextCampaning
    {
        public Guid id { get; set; }
        public List<Guid> Children { get; set; }
    }
    public class FullContextGroup
    {
        public Guid id { get; set; }
        public Guid Campaning {  get; set; }
        public List<Guid> Adverts { get; set;  }
    }

    public class FullContextAdvert
    {
        public Guid id { get; set; }
        public List<Guid> Groups { get; set; }

        public Guid Campaning { get; set; }

    }


    public class FullContext
    {
        public FullContext() { }

        public List<FullContextCampaning> f_camp {  get; set; }
        public List<FullContextGroup> f_group { get; set;}

        public List<FullContextAdvert> f_advert { get; set; }
        
    }
    public abstract class FullContextRepository
    {
        public abstract FullContext GetContext(int ID);
      
    }


    public class FullContextRepositoryMock : FullContextRepository
    {
        public ISession _session { get; set; }
        public IMapper _mapper { get; set; }
        public FullContextRepositoryMock(ISession session, IMapper mapper) {
            _session = session;
            _mapper = mapper;
        }
        public override FullContext GetContext(int ID)
        {
            List<CampaningMock> mockCamp = _session.GetJson<List<CampaningMock>>(SessionKeysReferences.campsKey)
                .Where( c => c.FK_User == ID).ToList() ?? new List<CampaningMock>();

            List<GroupModelMock> mockGroup = _session.GetJson<List<GroupModelMock>>(SessionKeysReferences.groupsKey)
                .Where( g => g.id_user == ID ).ToList() ?? new List<GroupModelMock>();

            List<AdvertisementMock> mockAdvert = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey)
                .Where(a => a.id_user == ID).ToList() ?? new List<AdvertisementMock>();

            List<FullContextCampaning> f_camp = mockCamp.Select(m => new FullContextCampaning() { id = m.id}).ToList();

            f_camp.ForEach(m =>
            {
                m.Children = mockGroup.Where(g => g.id_camp == m.id).Select(g => g.id).ToList();
            });

            List<FullContextGroup> f_group = mockGroup.Select(g => new FullContextGroup() { id = g.id, Campaning =  g.id_camp}).ToList();

            f_group.ForEach(m =>
            {
                m.Adverts = mockAdvert.Where(a => a.id_groups.Contains(m.id)).Select(a => a.id).ToList();
            });

            List<FullContextAdvert> f_advert = mockAdvert.Select(g => new FullContextAdvert() { id = g.id, Campaning = g.id_camp, Groups = g.id_groups.ToList() }).ToList();

            return new FullContext() { f_camp = f_camp, f_group = f_group, f_advert = f_advert };
        }
    }
}
