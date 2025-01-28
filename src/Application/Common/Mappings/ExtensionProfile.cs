using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Domain.Entities;

namespace AIExtensionsCenter.Application.Common.Mappings;
public class ExtensionProfile : Profile
{
    public ExtensionProfile()
    {
        CreateMap<Extension, ExtensionVM>();
    }
}
