using AutoMapper;
using SkyTakeOut.Core.DTO.Category;
using SkyTakeOut.Core.DTO.Employee;
using SkyTakeOut.Models;

namespace SkyTakeOut.Core.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ReverseMap()
                .ForAllMembers(opt =>
                {
                    opt.Condition((src, dest, srcMember) => srcMember != null);
                });

            CreateMap<Category, CategoryDTO>()
                .ReverseMap()
                .ForAllMembers(opt =>
                {
                    opt.Condition((src, dest, srcMember) => srcMember != null);
                });
        }
    }
}
