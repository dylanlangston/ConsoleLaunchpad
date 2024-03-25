using ConsoleLaunchpad.Core;

namespace ConsoleLaunchpad.Tests;

public class AuthenticationTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetURL()
    {
        ConsoleLaunchpad.Core.Authentication.GetURL();
        Assert.Pass();
    }
}