using ConsoleLaunchpad.Core;

namespace ConsoleLaunchpad.Tests;

public class AuthenticationTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetURL()
    {
        _ = await Authentication.GetURL(new Authentication.Profile());
        Console.WriteLine("foobar");
        Assert.Pass("Foobar");
    }
}