using AutoMapper;
using UserPanel.Models.Camp;

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
        }
    }
}
