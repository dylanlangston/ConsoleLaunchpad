<h1 align="center"><strong><em>ConsoleLaunchpad</em> 🕹️</strong></h1>
<a href="https://github.com/dylanlangston/consolelaunchpad/" title="ConsoleLaunchpad 🕹️">
  <p align="center">
    <img src="./icon.png" alt="ConsoleLaunchpad 🕹️" align="center"></img>
  </p>
</a>
<h4 align="center">An (unofficial) AWS Console Federated Sign-In Tool</h4>


### Overview
This tool simplifies access to the AWS Console by leveraging the [GetFederationToken](https://docs.aws.amazon.com/STS/latest/APIReference/API_GetFederationToken.html) API, allowing users to sign in swiftly and securely using their local AWS credentials. It caters to individuals who need frequent access to the AWS Console without constantly requiring them to enter their 2-Factor Authentication (2FA) details. Built with [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) and [Avalonia](https://www.avaloniaui.net/) to ensure a consistent expierence across different platforms while still using a single unified codebase. This is a community effort and not affiliated with Amazon/AWS, if you're having any problems please open an [issue](#support).

### Features
- **Efficient Sign-In**: Streamlines the sign-in process to the AWS Console by utilizing the GetFederationToken API.
- **Effective Security**: Offers a reasonably secure sign-in method without necessitating 2FA, suitable for users requiring regular access to the AWS Console.
- **Cross-Platform Compatibility**: Single codebase can be built on Desktop, Web, and Mobile.
- **User-Friendly Interface**: Intuitive UI design facilitates easy navigation and usage for both novice and experienced users.

### Demo
`Demo gif showcasing functionality coming soon...`

### Usage
`Detailed usage instructions coming soon...`

### Building
`Build instructions coming soon...`

### Solution Architecture
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

    Main -->|Business Logic| Core
    Imports -->|Application Interfaces| Core
    Core -->|User Interface| Browser
    Core -->|User Interface| Desktop
    Core -->|User Interface| Android
    Main -->|Unit Tests| Tests
    Core -->|Integration Tests| Tests
```

### Minimal Implementation
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

### Resources
Here are some additional resources regarding the GetFederationToken API and its usage:
- https://docs.aws.amazon.com/IAM/latest/UserGuide/id_roles_common-scenarios_federated-users.html#id_roles_common-scenarios_federated-users-idbroker
- https://docs.aws.amazon.com/IAM/latest/UserGuide/id_credentials_temp_request.html#api_getfederationtoken
- https://docs.aws.amazon.com/STS/latest/APIReference/API_GetFederationToken.html
- https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/SecurityToken/TGetFederationTokenRequest.html

### Contributions
Contributions to this project are welcome! Feel free to submit bug reports, feature requests, or pull requests via [GitHub](https://github.com/dylanlangston/consolelaunchpad).

### License
This tool is licensed under the [MIT License](https://opensource.org/licenses/MIT). See the [`LICENSE`](https://github.com/dylanlangston/consolelaunchpad/blob/main/LICENSE) file for details.

### Support
For any inquiries or assistance, please open an [issue](https://github.com/dylanlangston/consolelaunchpad/issues/new/choose).
