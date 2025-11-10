using AutoMapper;
using SkyTakeOut.Core.DTO.AddressBook;
using SkyTakeOut.Core.DTO.Category;
using SkyTakeOut.Core.DTO.Dish;
using SkyTakeOut.Core.DTO.Employee;
using SkyTakeOut.Core.DTO.Setmeal;
using SkyTakeOut.Core.DTO.ShoppingCart;
using SkyTakeOut.Core.VO.Dish;
using SkyTakeOut.Core.VO.Setmeal;
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

            CreateMap<Dish, DishVo>().ForMember(dest => dest.Flavors, opt => opt.Ignore()).ReverseMap();

            CreateMap<Setmeal, SetmealDTO>()
                .ReverseMap()
                .ForAllMembers(opt =>
                {
                    opt.Condition((src, dest, srcMember) => srcMember != null);
                });

            CreateMap<Setmeal, SetmealVo>().ForMember(dest => dest.SetmealDishes, opt => opt.Ignore());

            CreateMap<ShoppingCart, ShoppingCartDTO>().ReverseMap();

            CreateMap<AddressBookDTO, AddressBook>();
        }
    }
}
