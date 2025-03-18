
using AIExtensionsCenter.Application.Email.Commands.SendLicenseEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AIExtensionsCenter.Web.Endpoints;

public class Email : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(SendLicenseEmail, "send-license");
    }
    public async Task<IResult> SendLicenseEmail(ISender sender, [FromBody] SendLicenseEmailCommand command)
    {
        await sender.Send(command);
        return Results.Ok();
    }
}
