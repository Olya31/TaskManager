using AutoMapper;
using Entities.Models;
using Task_Manager.DTO;

namespace Task_Manager
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));

            CreateMap<TaskModelDto, TaskModel>()
               .ForMember(u => u.Name, opt => opt.MapFrom(x => x.Name))
               .ForMember(u => u.Description, opt => opt.MapFrom(x => x.Description))
               .ForMember(u => u.Url, opt => opt.MapFrom(x => x.Url))
               .ForMember(u => u.CronFormat, opt => opt.MapFrom(x => x.CronFormat))
               .ForMember(u => u.Header, opt => opt.MapFrom(x => x.Header))
               .ForMember(u => u.Email, opt => opt.MapFrom(x => x.Email));
        }
    }
}
