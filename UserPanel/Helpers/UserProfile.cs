using AutoMapper;
using System.Data;
using UserPanel.Models.User;
using UserPanel.References;

namespace UserPanel.Helpers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DataRow, UserModel>()
                .ForMember(x => x.Id, (conf) => conf.MapFrom((src, dest, destMember, context) => src.Field<int>("Id")))
                .ForMember(x => x.Email, (conf) => conf.MapFrom((src, dest, destMember, context) => src.Field<string>("Email")))
                .ForMember(x => x.Name, (conf) => conf.MapFrom((src, dest, destMember, context) => src.Field<string>("Name")))
                .ForMember(x => x.Address, (conf) => conf.MapFrom((src, dest, destMember, context) => src.Field<string>("Address")))
                .ForMember(x => x.Company, (conf) => conf.MapFrom((src, dest, destMember, context) => src.Field<string>("Company")))
                .ForMember(x => x.Password, (conf) => conf.MapFrom((src, dest, destMember, context) => src.Field<string>("Password")))
                .ForMember(x => x.Role, (conf) => conf.MapFrom((src, dest, destMember, context) => AppReferences.RoleMap[src?.Field<string>("Role") ?? "User"] ));

        }
    }
}
