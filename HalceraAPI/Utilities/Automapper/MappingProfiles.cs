using AutoMapper;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Category;
using HalceraAPI.Models.Requests.Media;

namespace HalceraAPI.Utilities.Automapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Category Request
            CreateMap<CategoryRequest, Category>().ReverseMap();
            CreateMap<Category, CategoryResponse>().ReverseMap();

            // Media
            CreateMap<CreateMediaRequest, Media>().ReverseMap();
            CreateMap<Media, MediaResponse>().ReverseMap();
        }
    }
}
