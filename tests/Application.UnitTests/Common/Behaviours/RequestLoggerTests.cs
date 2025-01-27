using AIExtensionsCenter.Application.Common.Interfaces;
using Moq;
using NUnit.Framework;

namespace AIExtensionsCenter.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<IUser> _user = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        _user = new Mock<IUser>();
        _identityService = new Mock<IIdentityService>();
    }

    [Test]
    public Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        _user.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());
        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
        return Task.CompletedTask;
    }

    [Test]
    public Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
        return Task.CompletedTask;
    }
}
