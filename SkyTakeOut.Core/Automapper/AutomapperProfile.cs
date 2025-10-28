using AutoMapper;
using SkyTakeOut.Core.DTO.Category;
using SkyTakeOut.Core.DTO.Dish;
using SkyTakeOut.Core.DTO.Employee;
using SkyTakeOut.Core.DTO.Setmeal;
using SkyTakeOut.Core.VO.Dish;
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

            CreateMap<Dish,DishDTO>()
                .ReverseMap()
                .ForAllMembers(opt =>
                {
                    opt.Condition((src, dest, srcMember) => srcMember != null);
                });

            CreateMap<Dish, DishVo>().ReverseMap();

            CreateMap<Setmeal, SetmealDTO>()
                .ReverseMap()
                .ForAllMembers(opt =>
                {
                    opt.Condition((src, dest, srcMember) => srcMember != null);
                });
        }
    }
}
