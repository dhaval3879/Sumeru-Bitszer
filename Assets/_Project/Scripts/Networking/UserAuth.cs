using TMPro;
using System;
using UnityEngine;
using GraphQlClient.Core;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;

public class UserAuth : MonoBehaviour
{
    [Header("Scriptable Object")]
    public GraphApi graphApi;

    [Header("UIManager")]
    public UIManager uiManager;

    [Header("Login UI")]
    public TMP_InputField emailLoginInputField;
    public TMP_InputField passwordLoginInputField;

    [Header("Signup UI")]
    public TMP_InputField fullNameSignupInputField;
    public TMP_InputField emailSignupInputField;
    public TMP_InputField passwordSignupInputField;
    public TMP_InputField confirmPasswordSignupInputField;

    private string poolId = "us-west-2_wItToCbsB";
    private string clientId = "553o5tjm99c10p22m6aopmtaat";

    public static Action OnAccessTokenReceived;

    public void SignUpUser()
    {
        RegisterUser();
    }

    public void SignInUser()
    {
        LoginUser();
        OnAccessTokenReceived?.Invoke();
    }

    public void GetData()
    {
        GetUserAuctions("70de85fa-f5aa-4c4e-b6be-415644096355", 10);
        GetMyProfile();
        GetProfile("Hannah");
        GetAuctions("", 10);
        GetInventory(10);
        GetMyInventoryByGame(10, "");
        GetAuctionsByGame("", 10);
        GetGameItemsByGame("", 10);
    }

    private async Task LoginUser()
    {
        AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Amazon.RegionEndpoint.USWest2);

        CognitoUserPool userPool = new CognitoUserPool(poolId, clientId, provider);

        CognitoUser user = new CognitoUser(emailLoginInputField.text, clientId, userPool, provider);

        InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
        {
            Password = passwordLoginInputField.text,
        };

        try
        {
            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);

            GetUserRequest getUserRequest = new GetUserRequest();
            getUserRequest.AccessToken = authResponse.AuthenticationResult.AccessToken;

            graphApi.SetAuthToken(getUserRequest.AccessToken);

            Debug.Log("User Access Token: " + getUserRequest.AccessToken);
        }
        catch(Exception e)
        {
            Debug.Log("EXCEPTION" + e);
            return;
        }
    }

    private async Task RegisterUser()
    {
        AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Amazon.RegionEndpoint.USWest2);

        SignUpRequest signUpRequest = new SignUpRequest()
        {
            ClientId = clientId,
            Username = emailSignupInputField.text,
            Password = passwordSignupInputField.text,
        };

        List<AttributeType> attributes = new List<AttributeType>()
        {
            new AttributeType() { Name = "fullName", Value = fullNameSignupInputField.text },
            new AttributeType() { Name = "email", Value = emailSignupInputField.text },
        };

        signUpRequest.UserAttributes = attributes;

        try
        {
            SignUpResponse request = await provider.SignUpAsync(signUpRequest);
            Debug.Log("Signed up");
        }
        catch(Exception e)
        {
            Debug.Log("Exception" + e);
            return;
        }
    }

    private async void GetUserAuctions(string userId, int limit)
    {
        GraphApi.Query getUserActionsQuery = graphApi.GetQueryByName("getUserAuctions", GraphApi.Query.Type.Query);

        getUserActionsQuery.SetArgs(new { userId, limit, });

        await graphApi.Post(getUserActionsQuery);
    }

    private async void GetMyProfile()
    {
        await graphApi.Post("getMyProfile", GraphApi.Query.Type.Query);
    }

    private async void GetProfile(string screenName)
    {
        GraphApi.Query getProfileQuery = graphApi.GetQueryByName("getProfile", GraphApi.Query.Type.Query);

        getProfileQuery.SetArgs(new { screenName, });

        await graphApi.Post(getProfileQuery);
    }

    private async void GetAuctions(string itemName, int limit)
    {
        GraphApi.Query getAuctionsQuery = graphApi.GetQueryByName("getAuctions", GraphApi.Query.Type.Query);

        getAuctionsQuery.SetArgs(new { itemName, limit, });

        await graphApi.Post(getAuctionsQuery);
    }

    private async void GetInventory(int limit)
    {
        GraphApi.Query getInventoryQuery = graphApi.GetQueryByName("getInventory", GraphApi.Query.Type.Query);

        getInventoryQuery.SetArgs(new { limit, });

        await graphApi.Post(getInventoryQuery);
    }

    private async void GetMyInventoryByGame(int limit, string gameId)
    {
        GraphApi.Query getMyInventorybyGameQuery = graphApi.GetQueryByName("getMyInventorybyGame", GraphApi.Query.Type.Query);

        getMyInventorybyGameQuery.SetArgs(new { limit, gameId, });

        await graphApi.Post(getMyInventorybyGameQuery);
    }

    private async void GetAuctionsByGame(string gameId, int limit)
    {
        GraphApi.Query getAuctionsbyGameQuery = graphApi.GetQueryByName("getAuctionsbyGame", GraphApi.Query.Type.Query);

        getAuctionsbyGameQuery.SetArgs(new { gameId, limit, });

        await graphApi.Post(getAuctionsbyGameQuery);
    }

    private async void GetGameItemsByGame(string gameId, int limit)
    {
        GraphApi.Query getGameItemsbyGameQuery = graphApi.GetQueryByName("getGameItemsbyGame", GraphApi.Query.Type.Query);

        getGameItemsbyGameQuery.SetArgs(new { gameId, limit, });

        await graphApi.Post(getGameItemsbyGameQuery);
    }
}