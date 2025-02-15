using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Domain.Entities;

namespace AIExtensionsCenter.Application.Common.Mappings;
public class LicenseProfile : Profile
{
    public LicenseProfile()
    {
        CreateMap<License, LicenseVM>();
    }
}
