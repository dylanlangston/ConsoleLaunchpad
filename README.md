<h1 align="center"><strong><em>ConsoleLaunchpad</em> 🕹️</strong></h1>
<a href="https://github.com/dylanlangston/consolelaunchpad/" title="ConsoleLaunchpad 🕹️">
  <p align="center">
    <img src="./icon.png" alt="ConsoleLaunchpad 🕹️" align="center"></img>
  </p>
</a>
<p align="center">
  <strong>An (unofficial) AWS Console Federated Sign-In Tool</strong>
</p>

<p align="center">
  <a href="https://dotnet.microsoft.com/en-us/"><img alt="C#" src="https://img.shields.io/badge/C%23-8.0-AC99EA.svg"></a>
  <a href="https://www.avaloniaui.net/"><img alt="Avalonia" src="https://img.shields.io/nuget/v/Avalonia?label=Avalonia&color=8b44ac"></a>
  <a href="https://github.com/dylanlangston/ConsoleLaunchpad/actions/workflows/build.yml"><img alt="GitHub Workflow CI/CD" src="https://img.shields.io/github/actions/workflow/status/dylanlangston/ConsoleLaunchpad/build.yml?label=CI%2FCD"></a>
  <a href="https://github.com/dylanlangston/ConsoleLaunchpad/blob/main/LICENSE"><img alt="GitHub License" src="https://img.shields.io/github/license/dylanlangston/ConsoleLaunchpad"></a>
  <a href="https://github.com/dylanlangston/ConsoleLaunchpad/releases/latest"><img alt="Latest Build" src="https://img.shields.io/badge/dynamic/json?url=https%3A%2F%2Fapi.github.com%2Frepos%2Fdylanlangston%ConsoleLaunchpad%2Freleases&query=%24%5B%3A1%5D.tag_name&label=Latest%20Build&color=%234c1"></a>
  <a href="https://api.github.com/repos/dylanlangston/ConsoleLaunchpad"><img alt="GitHub repo size" src="https://img.shields.io/github/repo-size/dylanlangston/ConsoleLaunchpad"></a>
</p>

### Overview 👀
This tool simplifies access to the AWS Console by leveraging the [GetFederationToken](https://docs.aws.amazon.com/STS/latest/APIReference/API_GetFederationToken.html) API, allowing users to sign in swiftly and securely using their local AWS credentials. It caters to individuals who need frequent access to the AWS Console without constantly requiring them to enter their 2-Factor Authentication (2FA) details. Built with [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) and [Avalonia](https://www.avaloniaui.net/) to ensure a consistent expierence across different platforms while still using a single unified codebase. This is a community effort and not affiliated with Amazon/AWS, if you're having any problems please open an [issue](#support).

### Features ✨
- [x] 🔑 **Efficient Sign-In**: Streamlines the sign-in process to the AWS Console by utilizing the GetFederationToken API.
- [x] 🛡️ **Effective Security**: Offers a reasonably secure sign-in method without necessitating 2FA, suitable for users requiring regular access to the AWS Console.
- [x] 🌐 **Cross-Platform Compatibility**: Single codebase can be built for Desktop, Web, and Mobile.
- [x] 🎨 **User-Friendly Interface**: Intuitive UI design facilitates easy navigation and usage for both novice and experienced users.

### Demo 🎬
`Demo gif showcasing functionality coming soon...`

### Usage ⏯️
`Detailed usage instructions coming soon...`

### Building 🏗️
`Build instructions coming soon...`

### Dev Environment 💻
This repository includes a *[devcontainer.json](.devcontainer/devcontainer.json)* to get up and running quickly with a full-featured development environment in the cloud![^local-development]

[![Open in GitHub Codespaces](https://img.shields.io/static/v1?style=flat&label=GitHub+Codespaces&message=Open&color=lightgrey&logo=github)](https://codespaces.new/dylanlangston/ConsoleLaunchpad)
[![Open in Dev Container](https://img.shields.io/static/v1?style=flat&label=Dev+Container&message=Open&color=blue&logo=visualstudiocode)](https://vscode.dev/redirect?url=vscode://ms-vscode-remote.remote-containers/cloneInVolume?url=https://github.com/dylanlangston/ConsoleLaunchpad)

### Solution Architecture 🏰
```mermaid
graph TD
    subgraph " "
        direction LR
        Core["ConsoleLaunchpad.Core"]
        Tests["ConsoleLaunchpad.Tests"]
        Main["ConsoleLaunchpad"]
        Browser["ConsoleLaunchpad.Browser"]
        Desktop["ConsoleLaunchpad.Desktop"]
        Imports["ConsoleLaunchpad.Imports"]
        Android["ConsoleLaunchpad.Android"]
    end

    Core -->|Business Logic| Main
    Imports -->|Application Interfaces| Main
    Main -->|User Interface| Browser
    Main -->|User Interface| Desktop
    Main -->|User Interface| Android
    Core -->|Unit Tests| Tests
    Main -->|Integration Tests| Tests
```

### Minimal Implementation 🛠️
For anyone asking "how does this all works", here's a minimal implementation in C#:
```csharp
using System;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;

class Program
{
    static async Task Main()
    {
        string policy = @"{
            ""Statement"": [{
                ""Effect"": ""Allow"",
                ""Action"": ""*"",
                ""Resource"": ""*""
            }]
        }"; // Your desired policy

        AWSCredentials credentials;
        CredentialProfileStoreChain chain = new CredentialProfileStoreChain();
        if (chain.TryGetAWSCredentials("profile_name", out credentials)) // Your AWS profile name
        {
            Console.WriteLine("Using local AWS profile credentials.");
        }
        else
        {
            credentials = new BasicAWSCredentials("YOUR_ACCESS_KEY_ID", "YOUR_SECRET_ACCESS_KEY"); // Your AWS credentials
        }

        using (var stsClient = new AmazonSecurityTokenServiceClient(credentials, Amazon.RegionEndpoint.USEast1)) // Replace the region if desired
        {
            GetFederationTokenRequest getTokenRequest = new()
            {
                Name = "Username",
                Policy = policy,
                DurationSeconds = 3600 // Set the duration for which the temporary credentials are valid
            };

            GetFederationTokenResponse getTokenResponse = await stsClient.GetFederationTokenAsync(getTokenRequest);

            string sessionToken = getTokenResponse.Credentials.SessionToken;
            string consoleSigninLink = $"https://signin.aws.amazon.com/federation?Action=login&Issuer=ExampleCorp&Destination=https%3A%2F%2Fconsole.aws.amazon.com%2F&SigninToken={Uri.EscapeDataString(sessionToken)}";

            Console.WriteLine($"Signed URL for AWS Console: {consoleSigninLink}");
        }
    }
}
```

### Resources ℹ️
Here are some additional resources regarding the GetFederationToken API and its usage:
- https://docs.aws.amazon.com/IAM/latest/UserGuide/id_roles_common-scenarios_federated-users.html#id_roles_common-scenarios_federated-users-idbroker
- https://docs.aws.amazon.com/IAM/latest/UserGuide/id_credentials_temp_request.html#api_getfederationtoken
- https://docs.aws.amazon.com/STS/latest/APIReference/API_GetFederationToken.html
- https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/SecurityToken/TGetFederationTokenRequest.html

### Contributions 🙌
Contributions to this project are welcome! Feel free to submit bug reports, feature requests, or pull requests via [GitHub](https://github.com/dylanlangston/consolelaunchpad).

### License 📜
This tool is licensed under the [MIT License](https://opensource.org/licenses/MIT). See the [`LICENSE`](https://github.com/dylanlangston/consolelaunchpad/blob/main/LICENSE) file for details.

### Support 🆘
For any inquiries or assistance, please open an [issue](https://github.com/dylanlangston/consolelaunchpad/issues/new/choose).

[^local-development]: For local development check out [Dev Containers](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers) and [DevPod](https://devpod.sh/).
