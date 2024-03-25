using System;
using System.Collections.Generic;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;

namespace ConsoleLaunchpad.Core;

public class Authentication
{
    public class Profile
    {
        public Profile() {

        }
    }

    public static List<Profile> ListProfiles() {
        return new List<Profile>();
    }
    public static void AddProfile(Profile profile) {
        return false;
    }
    public static bool RemoveProfile(Profile profile) {
        return false;
    }

    public static async Task<string> GetURL(Profile profile) {
        string issuerName = "";
        string policy = @"{
            ""Statement"": [{
                ""Effect"": ""Allow"",
                ""Action"": ""*"",
                ""Resource"": ""*""
            }]
        }";

        AWSCredentials credentials;
        CredentialProfileStoreChain chain = new CredentialProfileStoreChain();
        if (!chain.TryGetAWSCredentials("", out credentials))
        {
            credentials = new BasicAWSCredentials("", "");
        }

        using (var stsClient = new AmazonSecurityTokenServiceClient(credentials, Amazon.RegionEndpoint.USEast1))
        {
            GetFederationTokenRequest getTokenRequest = new()
            {
                Name = "",
                Policy = policy,
                DurationSeconds = 3600
            };

            GetFederationTokenResponse getTokenResponse = await stsClient.GetFederationTokenAsync(getTokenRequest);

            string sessionToken = getTokenResponse.Credentials.SessionToken;
            string consoleSigninLink = $"https://signin.aws.amazon.com/federation?Action=login&Issuer={Uri.EscapeDataString(issuerName)}&Destination=https%3A%2F%2Fconsole.aws.amazon.com%2F&SigninToken={Uri.EscapeDataString(sessionToken)}";

            return consoleSigninLink;
        }
    }
}
