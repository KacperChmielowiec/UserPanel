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
        public ISession _session { get; set; }
        public IMapper _mapper { get; set; }

        public IHttpContextAccessor _context { get; set; }   
        public AdvertRepositoryMock(ISession session,IMapper mapper, IHttpContextAccessor contextAccessor) { 
            _session = session;
            _mapper = mapper;
            _context = contextAccessor;
        }
        public override Advert GetAdvertById(Guid id)
        {
            var SessionModel = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey)
               ?.Where(ad => ad.id == id)
               .FirstOrDefault();

            if (SessionModel != null)
            {
                return _mapper.Map<Advert>(SessionModel);
            }

            return null;
        }

        public override List<Advert> GetAdvertGroupId(Guid id)
        {
            var SessionModelList = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey);
               

            if (SessionModelList != null)
            {
                var ads_real = new List<Advert>();
                var ads_mock = SessionModelList.Where(a => a.id_groups.Contains(id)).ToList();
              
                foreach (var item in ads_mock)
                {
                    ads_real.Add(_mapper.Map<Advert>(item));
                }
                return ads_real;
            }
            return new List<Advert>();

        }

        public override void CreateAdvert(Advert entity, Guid idGroup)
        {
            if (entity.Id == Guid.Empty) throw new ArgumentNullException("Empty Id parametr of AdverModel in CreateAdvert Method");
            var SessionModels = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey) ?? new List<AdvertisementMock>();
            var mockModel = _mapper.Map<AdvertisementMock>(entity);
            mockModel.id_groups = new Guid[] { idGroup };
            mockModel.id_user = UserManager.getUserId(_context);
            mockModel.id_camp = PermissionActionManager<Guid>.GetFullPath(idGroup).Camp;

            SessionModels.Add(mockModel);
            _session.SetJson(SessionKeysReferences.advertKey, SessionModels);
            Subjects.dataActionSubject.notify(new DataActionMessage() { id = entity.Id,Parent = idGroup, actionType = Types.DataActionType.ADD, dataType = DataType.Advert });
        }
        public override void UpdateAdvert(Advert model)
        {
            if (model == null) return;
            if (model.Id == Guid.Empty) throw new ArgumentNullException("Empty Guid for Advert model in UpdateAdvert method");

            var SessionModelList = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey).ToList() ?? new List<AdvertisementMock>();
            var SesionModel = SessionModelList.Where(m => m.id == model.Id).FirstOrDefault();

            if (SesionModel == null )
            {
                return;
            }

            SessionModelList.RemoveAll(m => m.id == model.Id);

            AdvertisementMock UpdateModel = _mapper.Map<AdvertisementMock>(model);

            UpdateModel.id_camp = SesionModel.id_camp;
            UpdateModel.id_groups = SesionModel.id_groups;
            UpdateModel.id_user = SesionModel.id_user;

            SessionModelList.Add(UpdateModel);

            _session.SetJson(SessionKeysReferences.advertKey, SessionModelList);

            //Subjects.dataActionSubject.notify(new DataActionMessage() { id = entity.Id, Parent = idGroup, actionType = Types.DataActionType.UPDATE, dataType = DataType.Advert });

        }
        public override List<Advert> GetAdvertByUserId(int id)
        {
           
            var SessionModels = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey) ?? new List<AdvertisementMock>();

            var FilteredModels = SessionModels.Where( model => model.id_user == id).ToList();

            List<Advert> ParsedModels = new List<Advert>();

            foreach(var model in FilteredModels)
            {
                ParsedModels.Add(_mapper.Map<Advert>(model));
            }

            return ParsedModels;
        }

        public override void DeleteAdvertsById(Guid[] ids)
        {
            if (ids.Length == 0) return;
            var SessionModels = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey) ?? new List<AdvertisementMock>();
            return;
        }

        public override void DettachAdvertFromGroup(Guid id, Guid id_group)
        {
            if (id == Guid.Empty || id_group == Guid.Empty) throw new ArgumentNullException("id or id_group is empty in DettachAdvertGroup method");
            var SessionModelsList = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey) ?? new List<AdvertisementMock>();
            AdvertisementMock adMock = SessionModelsList.FirstOrDefault(m => m.id == id);
            if (adMock != null)
            {
                adMock.id_groups = adMock.id_groups.Where(l => l != id_group).ToArray();
                SessionModelsList.RemoveAll(m => m.id == id);
                SessionModelsList.Add(adMock);
                _session.SetJson(SessionKeysReferences.advertKey, SessionModelsList);
            }
            
        }

        public override void DeleteAdvertsById(Guid id)
        {
            if(id == Guid.Empty) throw new ArgumentNullException($"Guid is empty in DeleteAdvertsById method");

            var SessionModelsGroup = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey) ?? new List<AdvertisementMock>();
            int elements = SessionModelsGroup.RemoveAll(m => m.id == id);

            if (elements == 0) return;

            _session.SetJson(SessionKeysReferences.advertKey, SessionModelsGroup);

        }

        public override void ChangeAttachStateAdverts(Guid[] ids, Guid id_group, bool attach)
        {
            if (ids.Length == 0 || id_group == Guid.Empty) throw new ArgumentNullException("id array or id_group is empty in DettachAdvertsGroup method");
            var SessionModelsList = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey) ?? new List<AdvertisementMock>();

            List<AdvertisementMock> adMockList = SessionModelsList.Where(m => ids.Contains(m.id))?.ToList() ?? new List<AdvertisementMock>();

            if (adMockList.Count > 0)
            {
                adMockList.ForEach((ad) =>
                {
                    if (attach)
                    {
                        ad.id_groups = ad.id_groups.Concat(new Guid[] { id_group }).ToArray();
                    }
                    else
                    {
                        ad.id_groups = ad.id_groups.Where(l => l != id_group).ToArray();
                    }
                    SessionModelsList.RemoveAll(m => m.id == ad.id);
                    SessionModelsList.Add(ad);
                    
                });
                _session.SetJson(SessionKeysReferences.advertKey, SessionModelsList);
            }
        }

        public override List<Advert> GetAdvertsByCampId(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException("Empty id in GetAdvertByCampId method");
            var SessionModelList = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey).Where(m => m.id_camp ==  id).ToList() ?? new List<AdvertisementMock>();
            
            return _mapper.Map<List<AdvertisementMock>,List<Advert>>(SessionModelList);
        }
    }
}
