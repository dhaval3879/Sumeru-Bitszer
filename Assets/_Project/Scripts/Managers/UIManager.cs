using System;
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    public TMPro.TMP_Text usernameText, balanceText;

    public GameObject loginPanel, signupPanel, homePanel, myAuctionPanel;
    public Transform auctionItemParent, auctionItemPrefab;

    private int length = 0;

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        homePanel.SetActive(false);
        myAuctionPanel.SetActive(false);
    }

    public void OpenSignupPanel()
    {
        signupPanel.SetActive(true);
        loginPanel.SetActive(false);
        homePanel.SetActive(false);
        myAuctionPanel.SetActive(false);
    }

    public void OpenHomePanel()
    {
        length = 0;
        auctionItemParent.Clear();

        signupPanel.SetActive(false);
        loginPanel.SetActive(false);
        homePanel.SetActive(true);
        myAuctionPanel.SetActive(false);
    }

    public void OpenMyAuctionPanel()
    {
        usernameText.text = DataProvider.Instance.myProfile.data.getMyProfile.name;
        balanceText.text = DataProvider.Instance.myProfile.data.getMyProfile.balance.ToString();

        DataProvider dataProvider = DataProvider.Instance;
        dataProvider.loadingPanel.SetActive(true);

        length = 0;
        var getUserAuction = dataProvider.GetUserAuctions(dataProvider.myProfile.data.getMyProfile.id, 10);

        getUserAuction.ContinueWith(_ =>
        {
            if (_.IsCompleted)
            {
                UnityMainThread.wkr.AddJob(() =>
                {
                    StartCoroutine(GetData(_));
                });
            }
        });
    }

    IEnumerator GetData(Task<GetUserAuction> _)
    {
        var count = _.Result.data.getUserAuctions.auctions.Count;

        var item = _.Result.data.getUserAuctions.auctions[length];
        GameObject go = Instantiate(auctionItemPrefab.gameObject, auctionItemParent);
        var auctionItem = go.GetComponent<AuctionItem>();
        StartCoroutine(GetImageFromUrl(item.gameItem.imageUrl, texture =>
        {
            auctionItem.itemImage.texture = texture;
        }));
        auctionItem.qtyText.text = item.quantity.ToString();
        auctionItem.itemNameText.text = item.gameItem.itemName;
        auctionItem.expirationText.text = item.expiration;
        auctionItem.buyoutText.text = item.buyout.ToString();
        auctionItem.bidText.text = item.bid.ToString();

        yield return null;

        length++;

        if (length < count)
            StartCoroutine(GetData(_));
        else
        {
            signupPanel.SetActive(false);
            loginPanel.SetActive(false);
            homePanel.SetActive(false);
            myAuctionPanel.SetActive(true);

            DataProvider.Instance.loadingPanel.SetActive(false);
        }
    }

    public IEnumerator GetImageFromUrl(string url, Action<Texture> texture)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
        {
            texture(((DownloadHandlerTexture)request.downloadHandler).texture);
        }
    }
}