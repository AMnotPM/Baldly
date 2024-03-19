namespace Baldly.Data.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Url, GetUrlVm>().ReverseMap();
        CreateMap<AppUser, GetUserVm>().ReverseMap();
    }
}