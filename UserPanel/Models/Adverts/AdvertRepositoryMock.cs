﻿using AutoMapper;
using UserPanel.Interfaces.Abstract;
using UserPanel.References;
using UserPanel.Helpers;
using UserPanel.Services;
using UserPanel.Services.observable;
using UserPanel.Types;
namespace UserPanel.Models.Adverts
{
    public class AdvertRepositoryMock : AdvertRepository<AdvertisementMock>
    {
        public ISession _session { get; set; }
        public IMapper _mapper { get; set; }
        public IHttpContextAccessor _context { get; set; }   
        public AdvertRepositoryMock(ISession session,IMapper mapper, IHttpContextAccessor contextAccessor) { 
            _session = session;
            _mapper = mapper;
            _context = contextAccessor;
        }
        public override AdvertisementMock GetAdvertById(Guid id)
        {
            var SessionModel = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey)
               ?.Where(ad => ad.id == id)
               .FirstOrDefault();

            if (SessionModel != null)
            {
                return SessionModel;
            }

            return null;
        }

        public override List<AdvertisementMock> GetAdvertGroupId(Guid id)
        {
            var SessionModelList = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey);
               

            if (SessionModelList != null)
            {
                
                var ads_mock = SessionModelList.Where(a => a.id_groups.Contains(id)).ToList();
              
                return ads_mock;
            }
            return new List<AdvertisementMock>();   

        }

        public override void CreateAdvert(AdvertisementMock entity, Guid idGroup)
        {
            if (entity.id == Guid.Empty) throw new ArgumentNullException("Empty Id parametr of AdverModel in CreateAdvert Method");
            var SessionModels = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey) ?? new List<AdvertisementMock>();
            var mockModel = entity;
            mockModel.id_groups = new Guid[] { idGroup };
            mockModel.id_user = UserManager.getUserId(_context);
            mockModel.id_camp = PermissionActionManager<Guid>.GetFullPath(idGroup).Camp;

            SessionModels.Add(mockModel);
            _session.SetJson(SessionKeysReferences.advertKey, SessionModels);
            Subjects.dataActionSubject.notify(new DataActionMessage() { id = entity.id ,Parent = idGroup, actionType = DataActionType.ADD, dataType = DataType.Advert });
        }
        public override void UpdateAdvert(AdvertisementMock model)
        {
            if (model == null) return;
            if (model.id == Guid.Empty) throw new ArgumentNullException("Empty Guid for Advert model in UpdateAdvert method");

            var SessionModelList = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey).ToList() ?? new List<AdvertisementMock>();
            var SessionModel = SessionModelList.Where(m => m.id == model.id).FirstOrDefault();

            if (SessionModel == null )
            {
                return;
            }

            SessionModelList.RemoveAll(m => m.id == model.id);

            AdvertisementMock UpdateModel = model;

            UpdateModel.id_camp = SessionModel.id_camp;
            UpdateModel.id_groups = SessionModel.id_groups;
            UpdateModel.id_user = SessionModel.id_user;
            UpdateModel.Created = SessionModel.Created;


            SessionModelList.Add(UpdateModel);

            _session.SetJson(SessionKeysReferences.advertKey, SessionModelList);

            //Subjects.dataActionSubject.notify(new DataActionMessage() { id = entity.Id, Parent = idGroup, actionType = Types.DataActionType.UPDATE, dataType = DataType.Advert });

        }
        public override List<AdvertisementMock> GetAdvertByUserId(int id)
        {
           
            var SessionModels = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey) ?? new List<AdvertisementMock>();

            var FilteredModels = SessionModels.Where( model => model.id_user == id).ToList();

            return FilteredModels;
        }

        public override void DeleteAdvertsById(Guid[] ids)
        {
            if (ids.Length == 0) return;
            var SessionModels = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey) ?? new List<AdvertisementMock>();
            int elements = SessionModels.RemoveAll(m => ids.Contains(m.id));

            if(elements == 0)
            {
                throw new KeyNotFoundException();
            }

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
                    Subjects.dataActionSubject.notify(new DataActionMessage() { actionType = attach ? DataActionType.ATTACH : DataActionType.DETACH, dataType = DataType.Advert, id = ad.id, Parent = id_group });
                    
                });
                _session.SetJson(SessionKeysReferences.advertKey, SessionModelsList);
                
            }
        }

        public override List<AdvertisementMock> GetAdvertsByCampId(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException("Empty id in GetAdvertByCampId method");
            var SessionModelList = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey).Where(m => m.id_camp ==  id).ToList() ?? new List<AdvertisementMock>();

            return SessionModelList;
        }

        public override AdvertGroupJoin GetGroupRelation(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException("Empty Guid in GetGroupRelation");
            var AdvertModelMock = _session.GetJson<List<AdvertisementMock>>(SessionKeysReferences.advertKey).Where(m => m.id == id).FirstOrDefault();

            if(AdvertModelMock != null )
            {
                var table = new AdvertGroupJoin();
                table.id_advert = id;
                table.one_to_many = AdvertModelMock.id_groups.ToList();
                return table;
            }

            return null;
            
        }
    }
    public class AdvertRepositoryDynamic : AdvertRepositoryMock
    {
        public AdvertRepositoryDynamic(ISession session, IMapper mapper, IHttpContextAccessor contextAccessor) : base(session, mapper, contextAccessor)
        {
        }

        public Advert<AdvertFormatDynamic> GetAdvertById(Guid id)
        {
            AdvertisementMock mock = base.GetAdvertById(id);
            if(mock != null && mock?.template == AD_TEMPLATE.Dynamic)
            {
                return _mapper.Map<Advert<AdvertFormatDynamic>>(mock);
            }
            return null;
        }

        public List<Advert<AdvertFormatDynamic>> GetAdvertGroupId(Guid id)
        {
            List<AdvertisementMock> mock = base.GetAdvertGroupId(id)?.Where(m => m.template == AD_TEMPLATE.Dynamic).ToList() ?? new List<AdvertisementMock>();
            if(mock.Any())
            {
                return _mapper.Map<List<Advert<AdvertFormatDynamic>>>(mock);  
            }
            return new List<Advert<AdvertFormatDynamic>>();
        }

        public List<Advert<AdvertFormatDynamic>> GetAdvertsByCampId(Guid id)
        {
            var mock = base.GetAdvertsByCampId(id)?.Where(m => m.template == AD_TEMPLATE.Dynamic) ?? new List<AdvertisementMock>();
            if(mock.Any())
            {
                return _mapper.Map<List<Advert<AdvertFormatDynamic>>>(mock);
            }
            return new List<Advert<AdvertFormatDynamic>>();
        }
        public List<Advert<AdvertFormatDynamic>> GetAdvertByUserId(int id)
        {
            var mock = base.GetAdvertByUserId(id)?.Where(m => m.template != AD_TEMPLATE.Dynamic) ?? new List<AdvertisementMock>();
            if(mock.Any())
            {
                return _mapper.Map<List<Advert<AdvertFormatDynamic>>>(mock);
            }
            return new List<Advert<AdvertFormatDynamic>>();    
        }
        public void CreateAdvert(Advert<AdvertFormatDynamic> advert, Guid idGroup)
        {
            base.CreateAdvert(_mapper.Map<AdvertisementMock>(advert), idGroup);
        }

        public void UpdateAdvert(Advert<AdvertFormatDynamic> advert)
        {
            base.UpdateAdvert(_mapper.Map<AdvertisementMock>(advert));
        }

        public void DeleteAdvertsById(Guid[] ids)
        {
            base.DeleteAdvertsById(ids);
        }
        public void DeleteAdvertsById(Guid id)
        {
            base.DeleteAdvertsById(id);
        }

        public void ChangeAttachStateAdverts(Guid[] ids, Guid id_group, bool attach)
        {
            base.ChangeAttachStateAdverts(ids, id_group, attach);
        }
        public AdvertGroupJoin GetGroupRelation(Guid id)
        {
            return base.GetGroupRelation(id);
        }
    }

    public class AdvertRepositoryStatic : AdvertRepositoryMock
    {
        public AdvertRepositoryStatic(ISession session, IMapper mapper, IHttpContextAccessor contextAccessor) : base(session, mapper, contextAccessor)
        {
        }

        public Advert<AdvertFormat> GetAdvertById(Guid id)
        {
            AdvertisementMock mock = base.GetAdvertById(id);
            if (mock != null && mock?.template == AD_TEMPLATE.Static)
            {
                return _mapper.Map<Advert<AdvertFormat>>(mock);
            }
            return null;
        }

        public List<Advert<AdvertFormat>> GetAdvertGroupId(Guid id)
        {
            List<AdvertisementMock> mock = base.GetAdvertGroupId(id)?.Where(m => m.template == AD_TEMPLATE.Static).ToList() ?? new List<AdvertisementMock>();
            if (mock.Any())
            {
                return _mapper.Map<List<Advert<AdvertFormat>>>(mock);
            }
            return new List<Advert<AdvertFormat>>();
        }

        public List<Advert<AdvertFormat>> GetAdvertsByCampId(Guid id)
        {
            var mock = base.GetAdvertsByCampId(id)?.Where(m => m.template == AD_TEMPLATE.Static) ?? new List<AdvertisementMock>();
            if (mock.Any())
            {
                return _mapper.Map<List<Advert<AdvertFormat>>>(mock);
            }
            return new List<Advert<AdvertFormat>>();
        }
        public List<Advert<AdvertFormat>> GetAdvertByUserId(int id)
        {
            var mock = base.GetAdvertByUserId(id)?.Where(m => m.template != AD_TEMPLATE.Static) ?? new List<AdvertisementMock>();
            if (mock.Any())
            {
                return _mapper.Map<List<Advert<AdvertFormat>>>(mock);
            }
            return new List<Advert<AdvertFormat>>();
        }
        public void CreateAdvert(Advert<AdvertFormat> advert, Guid idGroup)
        {
            base.CreateAdvert(_mapper.Map<AdvertisementMock>(advert), idGroup);
        }

        public void UpdateAdvert(Advert<AdvertFormat> advert)
        {
            base.UpdateAdvert(_mapper.Map<AdvertisementMock>(advert));
        }

        public void DeleteAdvertsById(Guid[] ids)
        {
            base.DeleteAdvertsById(ids);
        }
        public void DeleteAdvertsById(Guid id)
        {
            base.DeleteAdvertsById(id);
        }

        public void ChangeAttachStateAdverts(Guid[] ids, Guid id_group, bool attach)
        {
            base.ChangeAttachStateAdverts(ids, id_group, attach);
        }
        public AdvertGroupJoin GetGroupRelation(Guid id)
        {
            return base.GetGroupRelation(id);
        }
    }

    public class AdvertRepositoryFull : AdvertRepositoryMock
    {
        public AdvertRepositoryFull(ISession session, IMapper mapper, IHttpContextAccessor contextAccessor) : base(session, mapper, contextAccessor)
        {
        }

        public Advert<AdvertFormat> GetAdvertById(Guid id)
        {
            AdvertisementMock mock = base.GetAdvertById(id);
            if (mock != null)
            {
                return _mapper.Map<Advert<AdvertFormat>>(mock);
            }
            return null;
        }

        public List<Advert<AdvertFormat>> GetAdvertGroupId(Guid id)
        {
            List<AdvertisementMock> mock = base.GetAdvertGroupId(id);
            if (mock.Any())
            {
                return _mapper.Map<List<Advert<AdvertFormat>>>(mock);
            }
            return new List<Advert<AdvertFormat>>();
        }

        public List<Advert<AdvertFormat>> GetAdvertsByCampId(Guid id)
        {
            var mock = base.GetAdvertsByCampId(id);
            if (mock.Any())
            {
                return _mapper.Map<List<Advert<AdvertFormat>>>(mock);
            }
            return new List<Advert<AdvertFormat>>();
        }
        public List<Advert<AdvertFormat>> GetAdvertByUserId(int id)
        {
            var mock = base.GetAdvertByUserId(id);
            if (mock.Any())
            {
                return _mapper.Map<List<Advert<AdvertFormat>>>(mock);
            }
            return new List<Advert<AdvertFormat>>();
        }
        public void CreateAdvert(Advert<AdvertFormat> advert, Guid idGroup)
        {
            base.CreateAdvert(_mapper.Map<AdvertisementMock>(advert), idGroup);
        }

        public void UpdateAdvert(Advert<AdvertFormat> advert)
        {
            base.UpdateAdvert(_mapper.Map<AdvertisementMock>(advert));
        }

        public void DeleteAdvertsById(Guid[] ids)
        {
            base.DeleteAdvertsById(ids);
        }
        public void DeleteAdvertsById(Guid id)
        {
            base.DeleteAdvertsById(id);
        }

        public void ChangeAttachStateAdverts(Guid[] ids, Guid id_group, bool attach)
        {
            base.ChangeAttachStateAdverts(ids, id_group, attach);
        }
        public AdvertGroupJoin GetGroupRelation(Guid id)
        {
            return base.GetGroupRelation(id);
        }
    }
}
