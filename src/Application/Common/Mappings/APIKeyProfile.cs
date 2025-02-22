using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Domain.Entities;

namespace AIExtensionsCenter.Application.Common.Mappings;
public class APIKeyProfile : Profile
{
    public APIKeyProfile()
    {
        CreateMap<APIKey, APIKeyVM>();
    }
}
