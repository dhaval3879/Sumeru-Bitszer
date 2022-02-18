using UnityEngine;
using Newtonsoft.Json;
using GraphQlClient.Core;
using System.Threading.Tasks;

public class DataProvider : Singleton<DataProvider>
{
    [Header("Scriptable Object")]
    public GraphApi graphApi;

    [Space]
    [Space]
    public GameObject loadingPanel;

    public Profile myProfile;

    public async Task<GetUserAuction> GetUserAuctions(string userId, int limit)
    {
        GraphApi.Query getUserActionsQuery = graphApi.GetQueryByName("getUserAuctions", GraphApi.Query.Type.Query);

        getUserActionsQuery.SetArgs(new { userId, limit, });

        var www = await graphApi.Post(getUserActionsQuery);
        var data = JsonConvert.DeserializeObject<GetUserAuction>(www.downloadHandler.text);
        return data;
    }

    public async void GetMyProfile()
    {
        loadingPanel.SetActive(true);

        var www = await graphApi.Post("getMyProfile", GraphApi.Query.Type.Query);
        var data = JsonConvert.DeserializeObject<Profile>(www.downloadHandler.text);

        if (www.isDone)
        {
            myProfile = data;
            loadingPanel.SetActive(false);
        }
    }

    public async void GetProfile(string screenName)
    {
        GraphApi.Query getProfileQuery = graphApi.GetQueryByName("getProfile", GraphApi.Query.Type.Query);

        getProfileQuery.SetArgs(new { screenName, });

        await graphApi.Post(getProfileQuery);
    }

    public async void GetAuctions(string itemName, int limit)
    {
        GraphApi.Query getAuctionsQuery = graphApi.GetQueryByName("getAuctions", GraphApi.Query.Type.Query);

        getAuctionsQuery.SetArgs(new { itemName, limit, });

        await graphApi.Post(getAuctionsQuery);
    }

    public async void GetInventory(int limit)
    {
        GraphApi.Query getInventoryQuery = graphApi.GetQueryByName("getInventory", GraphApi.Query.Type.Query);

        getInventoryQuery.SetArgs(new { limit, });

        await graphApi.Post(getInventoryQuery);
    }

    public async void GetMyInventoryByGame(int limit, string gameId)
    {
        GraphApi.Query getMyInventorybyGameQuery = graphApi.GetQueryByName("getMyInventorybyGame", GraphApi.Query.Type.Query);

        getMyInventorybyGameQuery.SetArgs(new { limit, gameId, });

        await graphApi.Post(getMyInventorybyGameQuery);
    }

    public async void GetAuctionsByGame(string gameId, int limit)
    {
        GraphApi.Query getAuctionsbyGameQuery = graphApi.GetQueryByName("getAuctionsbyGame", GraphApi.Query.Type.Query);

        getAuctionsbyGameQuery.SetArgs(new { gameId, limit, });

        await graphApi.Post(getAuctionsbyGameQuery);
    }

    public async void GetGameItemsByGame(string gameId, int limit)
    {
        GraphApi.Query getGameItemsbyGameQuery = graphApi.GetQueryByName("getGameItemsbyGame", GraphApi.Query.Type.Query);

        getGameItemsbyGameQuery.SetArgs(new { gameId, limit, });

        await graphApi.Post(getGameItemsbyGameQuery);
    }
}