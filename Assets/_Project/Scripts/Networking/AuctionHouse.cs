using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using GraphQlClient.Core;
using System.Threading.Tasks;

namespace Bitszer
{
    public class AuctionHouse : Singleton<AuctionHouse>
    {
        [Header("Scriptable Object")]
        public GraphApi graphApi;
        public Configuration configuration;

        [Space]
        [Space]
        public GameObject bitszer;

        #region PUBLIC METHODS
        public void Open()
        {
            bitszer.SetActive(true);
        }
        #endregion

        #region QUERIES
        public async Task<GetUserAuction> GetUserAuctions(string userId, int limit, string nextToken)
        {
            GraphApi.Query getUserActionsQuery = graphApi.GetQueryByName("getUserAuctions", GraphApi.Query.Type.Query);

            if (string.IsNullOrEmpty(nextToken))
                getUserActionsQuery.SetArgs(new { userId, limit, });
            else
                getUserActionsQuery.SetArgs(new { userId, limit, nextToken, });

            var www = await graphApi.Post(getUserActionsQuery);
            var data = JsonConvert.DeserializeObject<GetUserAuction>(www.downloadHandler.text);
            return data;
        }

        public async Task<Profile> GetMyProfile()
        {
            APIManager.Instance.RaycastBlock(true);

            GraphApi.Query getProfileQuery = graphApi.GetQueryByName("getMyProfile", GraphApi.Query.Type.Query);

            var www = await graphApi.Post(getProfileQuery);
            var data = JsonConvert.DeserializeObject<Profile>(www.downloadHandler.text);

            if (data != null)
                Events.OnProfileReceived.Invoke(data);

            return data;
        }

        public async Task<Profile> GetProfile(string screenName)
        {
            GraphApi.Query getProfileQuery = graphApi.GetQueryByName("getProfile", GraphApi.Query.Type.Query);

            getProfileQuery.SetArgs(new { screenName, });

            var www = await graphApi.Post(getProfileQuery);
            var data = JsonConvert.DeserializeObject<Profile>(www.downloadHandler.text);

            return data;
        }

        public async Task<GetAuction> GetAuctions(string itemName, int limit, string nextToken)
        {
            GraphApi.Query getAuctionsQuery = graphApi.GetQueryByName("getAuctions", GraphApi.Query.Type.Query);

            if (string.IsNullOrEmpty(nextToken))
                getAuctionsQuery.SetArgs(new { itemName, limit, });
            else
                getAuctionsQuery.SetArgs(new { itemName, limit, nextToken, });

            var www = await graphApi.Post(getAuctionsQuery);
            var data = JsonConvert.DeserializeObject<GetAuction>(www.downloadHandler.text);

            if (data != null)
                Events.OnAuctionsReceived.Invoke(data);

            return data;
        }

        public async Task<GetInventory> GetInventory(int limit, string nextToken)
        {
            GraphApi.Query getInventoryQuery = graphApi.GetQueryByName("getInventory", GraphApi.Query.Type.Query);

            if (string.IsNullOrEmpty(nextToken))
                getInventoryQuery.SetArgs(new { limit, });
            else
                getInventoryQuery.SetArgs(new { limit, nextToken, });

            var www = await graphApi.Post(getInventoryQuery);
            var data = JsonConvert.DeserializeObject<GetInventory>(www.downloadHandler.text);

            if (data != null)
                Events.OnInventoryReceived.Invoke(data);

            return data;
        }

        public async Task<GetMyInventoryByGame> GetMyInventoryByGame(int limit, string nextToken)
        {
            GraphApi.Query getMyInventorybyGameQuery = graphApi.GetQueryByName("getMyInventorybyGame", GraphApi.Query.Type.Query);

            if (string.IsNullOrEmpty(nextToken))
                getMyInventorybyGameQuery.SetArgs(new { limit, configuration.gameId, });
            else
                getMyInventorybyGameQuery.SetArgs(new { limit, nextToken, configuration.gameId, });

            var www = await graphApi.Post(getMyInventorybyGameQuery);
            var data = JsonConvert.DeserializeObject<GetMyInventoryByGame>(www.downloadHandler.text);

            if (data != null)
                Events.OnMyInventoryByGameReceived.Invoke(data);

            return data;
        }

        public async Task<GetAuctionbyGame> GetAuctionsByGame(int limit, string nextToken)
        {
            GraphApi.Query getAuctionsbyGameQuery = graphApi.GetQueryByName("getAuctionsbyGame", GraphApi.Query.Type.Query);

            if (string.IsNullOrEmpty(nextToken))
                getAuctionsbyGameQuery.SetArgs(new { configuration.gameId, limit, });
            else
                getAuctionsbyGameQuery.SetArgs(new { configuration.gameId, limit, nextToken, });

            var www = await graphApi.Post(getAuctionsbyGameQuery);
            var data = JsonConvert.DeserializeObject<GetAuctionbyGame>(www.downloadHandler.text);

            if (data != null)
                Events.OnAuctionsByGameReceived.Invoke(data);

            return data;
        }

        public async Task<GetGameItembyGame> GetGameItemsByGame(int limit, string nextToken)
        {
            GraphApi.Query getGameItemsbyGameQuery = graphApi.GetQueryByName("getGameItemsbyGame", GraphApi.Query.Type.Query);

            if (string.IsNullOrEmpty(nextToken))
                getGameItemsbyGameQuery.SetArgs(new { configuration.gameId, limit, });
            else
                getGameItemsbyGameQuery.SetArgs(new { configuration.gameId, limit, nextToken, });

            var www = await graphApi.Post(getGameItemsbyGameQuery);
            var data = JsonConvert.DeserializeObject<GetGameItembyGame>(www.downloadHandler.text);

            if (data != null)
                Events.OnGameItemByGameReceived.Invoke(data);

            return data;
        }
        #endregion

        #region MUTATIONS
        public IEnumerator CreateAuction(AuctionInput newAuction, Action<Auction> result)
        {
            GraphApi.Query createAuctionMutation = graphApi.GetQueryByName("createAuction", GraphApi.Query.Type.Mutation);

            createAuctionMutation.SetArgs(new { newAuction, });

            var www = graphApi.Post(createAuctionMutation);
            yield return new WaitUntil(() => www.IsCompleted);
            var data = JsonConvert.DeserializeObject<Auction>(www.Result.downloadHandler.text);
            result(data);
        }

        public IEnumerator CancelAuction(string auctionId, Action<CancelAuction> result)
        {
            GraphApi.Query cancelAuctionMutation = graphApi.GetQueryByName("cancelAuction", GraphApi.Query.Type.Mutation);

            cancelAuctionMutation.SetArgs(new { auctionId, });

            var www = graphApi.Post(cancelAuctionMutation);
            yield return new WaitUntil(() => www.IsCompleted);
            var data = JsonConvert.DeserializeObject<CancelAuction>(www.Result.downloadHandler.text);
            result(data);
        }

        public IEnumerator Buyout(string auctionId, Action<Buyout> result)
        {
            GraphApi.Query buyoutMutation = graphApi.GetQueryByName("buyout", GraphApi.Query.Type.Mutation);

            buyoutMutation.SetArgs(new { auctionId, });

            var www = graphApi.Post(buyoutMutation);
            yield return new WaitUntil(() => www.IsCompleted);
            var data = JsonConvert.DeserializeObject<Buyout>(www.Result.downloadHandler.text);
            result(data);
        }

        public IEnumerator Bid(string auctionId, float bid, Action<Bid> result)
        {
            GraphApi.Query bidMutation = graphApi.GetQueryByName("bid", GraphApi.Query.Type.Mutation);

            bidMutation.SetArgs(new { auctionId, bid, });

            var www = graphApi.Post(bidMutation);
            yield return new WaitUntil(() => www.IsCompleted);
            var data = JsonConvert.DeserializeObject<Bid>(www.Result.downloadHandler.text);
            result(data);
        }

        public IEnumerator UpdateInventory(string itemId, int count, Action<UpdateInventory> result)
        {
            GraphApi.Query updateInventoryMutation = graphApi.GetQueryByName("updateInventory", GraphApi.Query.Type.Mutation);

            updateInventoryMutation.SetArgs(new { configuration.gameId, itemId, count });

            var www = graphApi.Post(updateInventoryMutation);
            yield return new WaitUntil(() => www.IsCompleted);
            var data = JsonConvert.DeserializeObject<UpdateInventory>(www.Result.downloadHandler.text);
            result(data);
        }

        public IEnumerator PushInventory(InventoryDelta[] inventoryDelta, Action<PushInventory> result)
        {
            GraphApi.Query pushInventoryMutation = graphApi.GetQueryByName("pushInventory", GraphApi.Query.Type.Mutation);

            pushInventoryMutation.SetArgs(new { configuration.gameId, inventoryDelta });

            var www = graphApi.Post(pushInventoryMutation);
            yield return new WaitUntil(() => www.IsCompleted);
            var data = JsonConvert.DeserializeObject<PushInventory>(www.Result.downloadHandler.text);
            result(data);
        }
        #endregion
    }
}