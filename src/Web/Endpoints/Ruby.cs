using AIExtensionsCenter.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AIExtensionsCenter.Web.Endpoints;

public class Ruby : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(DownloadRubyFile, "download");
    }
    private async Task<IResult> DownloadRubyFile(ISender sender, [FromQuery] string extensionId, [FromQuery] string moduleName)
    {
        var fileBytes = await sender.Send(new GenerateRubyFileCommand(extensionId,moduleName));
        return Results.File(fileBytes, "application/zip", "AIExtensionCenter.zip");
    }
}
