using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Domain.Entities;

namespace AIExtensionsCenter.Application.Common.Mappings;
public class LicenseProfile : Profile
{
    public LicenseProfile()
    {
        CreateMap<License, LicenseVM>()
            .ForMember(dest => dest.LicenseStatus, opt => opt.MapFrom(src => src.LicenseStatus.ToString()));
    }
}
