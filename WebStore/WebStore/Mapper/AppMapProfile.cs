using AutoMapper;
using WebStore.Data.Entities;
using WebStore.Models.Category;

namespace WebStore.Mapper
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile() 
        {
            CreateMap<CategoryEntity, CategoryItemViewModel>();
            
            CreateMap<CategoryCreateViewModel, CategoryEntity>();
        }
    }
}
