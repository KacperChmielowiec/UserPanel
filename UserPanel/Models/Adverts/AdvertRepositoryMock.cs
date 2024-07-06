using AutoMapper;
using UserPanel.Interfaces.Abstract;
using UserPanel.References;
using UserPanel.Helpers;
using UserPanel.Services;
using UserPanel.Services.observable;
namespace UserPanel.Models.Adverts
{
    public class AdvertRepositoryMock : AdvertRepository<Advert>
    {
        public ISession Session { get; set; }
        public IMapper Mapper { get; set; }

        public IHttpContextAccessor Context { get; set; }   
        public AdvertRepositoryMock(ISession session,IMapper mapper, IHttpContextAccessor contextAccessor) { 
            Session = session;
            Mapper = mapper;
            Context = contextAccessor;
        }
        public override Advert GetAdvertById(Guid id)
        {
            var curr = Session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey)
               ?.Where(ad => ad.id == id)
               .FirstOrDefault();

            if (curr != null)
            {
                return Mapper.Map<Advert>(curr);
            }

            return null;
        }

        public override List<Advert> GetAdvertGroupId(Guid id)
        {
            var ads_session = Session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey);
               

            if (ads_session != null)
            {
                var ads_real = new List<Advert>();
                var ads_mock = ads_session.Where(a => a.id_group == id).ToList();
              
                foreach (var item in ads_mock)
                {
                    ads_real.Add(Mapper.Map<Advert>(item));
                }
                return ads_real;
            }
            return new List<Advert>();

        }

        public override void CreateAdvert(Advert entity)
        {
            if (entity.Id == Guid.Empty || entity.Parent == Guid.Empty) throw new ArgumentNullException("id");
            var SessionModels = Session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey) ?? new List<AdvertisementMock>();
            var mockModel = Mapper.Map<AdvertisementMock>(entity);

            mockModel.id_user = UserManager.getUserId(Context);
            mockModel.id_camp = PermissionActionManager<Guid>.GetFullPath(mockModel.id_group).Camp;

            SessionModels.Add(mockModel);
            Session.SetJson(SessionKeysReferences.advertKey, SessionModels);
            Subjects.dataActionSubject.notify(new DataActionMessage() { id = entity.Id,Parent = entity.Parent, actionType = Types.DataActionType.ADD, dataType = DataType.Advert });
        }

        public override List<Advert> GetAdvertByUserId(int id)
        {
           
            var SessionModels = Session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey) ?? new List<AdvertisementMock>();

            var FilteredModels = SessionModels.Where( model => model.id_user == id).ToList();

            List<Advert> ParsedModels = new List<Advert>();

            foreach(var model in FilteredModels)
            {
                ParsedModels.Add(Mapper.Map<Advert>(model));
            }

            return ParsedModels;
        }

    }
}
