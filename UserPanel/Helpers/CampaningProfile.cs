using AutoMapper;
using UserPanel.Interfaces.Abstract;
using UserPanel.Models.Adverts;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;
using UserPanel.Models.User;

namespace UserPanel.Helpers
{
    public class CampaningProfile : Profile
    {
        public CampaningProfile() {

            CreateMap<Campaning, EditCampaning>()
                .ForMember(e => e.country, (conf) => conf.MapFrom((src, dest, destMember, context) => src.details.Country))
                .ForMember(e => e.currency, (conf) => conf.MapFrom((src, dest, destMember, context) => src.details.Currency))
                .ForMember(e => e.notify, (conf) => conf.MapFrom((src, dest, destMember, context) => src.details.EmailNotify))
                .ForMember(e => e.totalBudget, (conf) => conf.MapFrom((src, dest, destMember, context) => src.budget.totalBudget))
                .ForMember(e => e.dayBudget, (conf) => conf.MapFrom((src, dest, destMember, context) => src.budget.dayBudget))
                .ForMember(e => e.Utm_Source, (conf) => conf.MapFrom((src, dest, destMember, context) => src.details.Utm_Source))
                .ForMember(e => e.Utm_Medium, (conf) => conf.MapFrom((src, dest, destMember, context) => src.details.Utm_Medium));

            CreateMap<EditCampaning,Campaning>()
                .ForPath(e => e.details.Country, (conf) => conf.MapFrom((src) => src.country))
                .ForPath(e => e.details.Currency, (conf) => conf.MapFrom((src) => src.currency))
                .ForPath(e => e.details.EmailNotify, (conf) => conf.MapFrom((src) => src.notify))
                .ForPath(e => e.budget.totalBudget, (conf) => conf.MapFrom((src) => src.totalBudget))
                .ForPath(e => e.budget.dayBudget, (conf) => conf.MapFrom((src) => src.dayBudget))
                .ForPath(e => e.details.Utm_Source, (conf) => conf.MapFrom((src) => src.Utm_Source))
                .ForPath(e => e.details.Utm_Medium, (conf) => conf.MapFrom((src) => src.Utm_Medium));

            CreateMap<CampaningMock, Campaning>();
            CreateMap<Campaning, CampaningMock>();

            CreateMap<GroupModel, EditGroup>()
                .ForMember(e => e.Billing, (conf) => conf.MapFrom((src) => src.details.Billing))
                .ForMember(e => e.Devices, (conf) => conf.MapFrom((src) => src.details.Devices))
                .ForMember(e => e.startTime, (conf) => conf.MapFrom((src) => src.details.startTime))
                .ForMember(e => e.endTime, (conf) => conf.MapFrom((src) => src.details.endTime))
                .ForMember(e => e.Utm_Camp, (conf) => conf.MapFrom((src) => src.details.Utm_Camp))
                .ForMember(e => e.Utm_Source, (conf) => conf.MapFrom((src) => src.details.Utm_Source))
                .ForMember(e => e.Utm_Medium, (conf) => conf.MapFrom((src) => src.details.Utm_Medium))
                .ForMember(e => e.totalBudget, (conf) => conf.MapFrom((src) => src.budget.totalBudget))
                .ForMember(e => e.dayBudget, (conf) => conf.MapFrom((src) => src.budget.dayBudget));

            CreateMap<EditGroup, GroupModel>()
                .ForPath(e => e.details.Billing, (conf) => conf.MapFrom((src) => src.Billing))
                .ForPath(e => e.details.Devices, (conf) => conf.MapFrom((src) => src.Devices))
                .ForPath(e => e.details.startTime, (conf) => conf.MapFrom((src) => src.startTime))
                .ForPath(e => e.details.endTime, (conf) => conf.MapFrom((src) => src.endTime))
                .ForPath(e => e.budget.totalBudget, (conf) => conf.MapFrom((src) => src.totalBudget))
                .ForPath(e => e.budget.dayBudget, (conf) => conf.MapFrom((src) => src.dayBudget))
                .ForPath(e => e.details.Utm_Source, (conf) => conf.MapFrom((src) => src.Utm_Source))
                .ForPath(e => e.details.Utm_Medium, (conf) => conf.MapFrom((src) => src.Utm_Medium))
                .ForPath(e => e.details.Utm_Camp, (conf) => conf.MapFrom((src) => src.Utm_Camp));

            CreateMap<GroupModel, GroupModelMock>();
            CreateMap<GroupModelMock, GroupModel>()
                .ForMember(e => e.Parent, (conf) => conf.MapFrom((src) => src.id_camp));
            CreateMap<GroupLists,GroupListMock>();
            CreateMap<GroupListMock, GroupLists>();

            CreateMap<CreateGroup, GroupModel>()
                .ForPath(e => e.details.Billing, (conf) => conf.MapFrom((src) => src.Billing))
                .ForPath(e => e.details.Devices, (conf) => conf.MapFrom((src) => src.Devices))
                .ForPath(e => e.details.startTime, (conf) => conf.MapFrom((src) => src.startTime))
                .ForPath(e => e.details.endTime, (conf) => conf.MapFrom((src) => src.endTime))
                .ForPath(e => e.budget.totalBudget, (conf) => conf.MapFrom((src) => src.totalBudget))
                .ForPath(e => e.budget.dayBudget, (conf) => conf.MapFrom((src) => src.dayBudget))
                .ForPath(e => e.details.Utm_Source, (conf) => conf.MapFrom((src) => src.Utm_Source))
                .ForPath(e => e.details.Utm_Medium, (conf) => conf.MapFrom((src) => src.Utm_Medium))
                .ForPath(e => e.details.Utm_Camp, (conf) => conf.MapFrom((src) => src.Utm_Camp));


            CreateMap<Advert, AdvertisementMock>();
            CreateMap<AdvertisementMock, Advert>();
            CreateMap<AdvertForm, Advert>();
            CreateMap<AdvertFormatForm, AdvertFormat>();
            CreateMap<Advert, AdvertForm>();
            CreateMap<AdvertForm, Advert>();
            CreateMap<AdvertFormat, AdvertFormatForm>();
            CreateMap<AdvertFormatForm, AdvertFormat>();


            CreateMap<Campaning, FullContextCampaning>();
              

        }
    }
}
