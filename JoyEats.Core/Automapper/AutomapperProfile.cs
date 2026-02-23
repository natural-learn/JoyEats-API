using AutoMapper;
using JoyEats.Core.DTO.AddressBook;
using JoyEats.Core.DTO.Category;
using JoyEats.Core.DTO.Dish;
using JoyEats.Core.DTO.Employee;
using JoyEats.Core.DTO.Order;
using JoyEats.Core.DTO.Setmeal;
using JoyEats.Core.DTO.ShoppingCart;
using JoyEats.Core.VO.Dish;
using JoyEats.Core.VO.Order;
using JoyEats.Core.VO.Setmeal;
using JoyEats.Models;

namespace JoyEats.Core.Automapper
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

            CreateMap<Dish, DishDTO>()
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

            CreateMap<AddressBookDTO, AddressBook>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember) => srcMember != null));

            CreateMap<OrdersSubmitDTO, Orders>();
            CreateMap<ShoppingCart, OrderDetail>().ReverseMap();
            CreateMap<Orders, OrderVO>().ReverseMap();
        }
    }
}
