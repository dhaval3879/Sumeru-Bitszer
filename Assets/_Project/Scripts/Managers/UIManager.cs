using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    public TMPro.TMP_Text titleText;
    public TMPro.TMP_Text usernameText;
    public TMPro.TMP_Text balanceText;

    public Toggle homeToggle, buyToggle, sellToggle, myAuctionsToggle;

    [Space]
    [Space]
    public GameObject loginPanel;
    public GameObject signupPanel;
    public GameObject tabPanel;
    public Transform buyItemParent;
    public Transform buyItemPrefab;
    public Transform sellItemParent;
    public Transform sellItemPrefab;
    public Transform auctionItemParent;
    public Transform auctionItemPrefab;

    private int buyItemsLength = 0;
    private int sellItemsLength = 0;
    private int myAuctionsLength = 0;

    private Profile _profile;

    private void OnEnable()
    {
        Events.OnProfileReceived.AddListener(OnProfileReceived);

        homeToggle.onValueChanged.AddListener(HomeToggleValueChanged);
        buyToggle.onValueChanged.AddListener(BuyToggleValueChanged);
        sellToggle.onValueChanged.AddListener(SellToggleValueChanged);
        myAuctionsToggle.onValueChanged.AddListener(MyAuctionsToggleValueChanged);
    }

    private void OnDisable()
    {
        Events.OnProfileReceived.RemoveListener(OnProfileReceived);

        homeToggle.onValueChanged.RemoveListener(HomeToggleValueChanged);
        buyToggle.onValueChanged.RemoveListener(BuyToggleValueChanged);
        sellToggle.onValueChanged.RemoveListener(SellToggleValueChanged);
        myAuctionsToggle.onValueChanged.RemoveListener(MyAuctionsToggleValueChanged);
    }

    private void OnProfileReceived(Profile profile)
    {
        _profile = profile;

        usernameText.text = _profile.data.getMyProfile.name;
        balanceText.text = _profile.data.getMyProfile.balance.ToString();

        GetBuyData();
        GetSellData();
        GetMyAuctionsData();
    }

    private void HomeToggleValueChanged(bool isOn)
    {
        if (isOn)
            titleText.text = "Home";
    }

    private void BuyToggleValueChanged(bool isOn)
    {
        if (isOn)
            titleText.text = "Buy";
    }

    private void SellToggleValueChanged(bool isOn)
    {
        if (isOn)
            titleText.text = "Sell";
    }

    private void MyAuctionsToggleValueChanged(bool isOn)
    {
        if (isOn)
            titleText.text = "My Auctions";
    }

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        tabPanel.SetActive(false);
    }

    public void OpenSignupPanel()
    {
        signupPanel.SetActive(true);
        loginPanel.SetActive(false);
        tabPanel.SetActive(false);
    }

    public void OpenTabPanel()
    {
        signupPanel.SetActive(false);
        loginPanel.SetActive(false);
        tabPanel.SetActive(true);
    }

    public void GetBuyData()
    {
        DataProvider dataProvider = DataProvider.Instance;
        dataProvider.loadingPanel.SetActive(true);

        buyItemsLength = 0;
        var getAuction = dataProvider.GetAuctions("", 10);

        getAuction.ContinueWith(_ =>
        {
            if (_.IsCompleted)
            {
                UnityMainThread.wkr.AddJob(() =>
                {
                    StartCoroutine(PopulateBuyData(_));
                });
            }
        });
    }

    public void GetSellData()
    {
        DataProvider dataProvider = DataProvider.Instance;
        dataProvider.loadingPanel.SetActive(true);

        sellItemsLength = 0;
        var getInventory = dataProvider.GetInventory(10);

        getInventory.ContinueWith(_ =>
        {
            if (_.IsCompleted)
            {
                UnityMainThread.wkr.AddJob(() =>
                {
                    StartCoroutine(PopulateSellData(_));
                });
            }
        });
    }

    public void GetMyAuctionsData()
    {
        DataProvider dataProvider = DataProvider.Instance;
        dataProvider.loadingPanel.SetActive(true);

        myAuctionsLength = 0;
        var getUserAuction = dataProvider.GetUserAuctions(_profile.data.getMyProfile.id, 10);

        getUserAuction.ContinueWith(_ =>
        {
            if (_.IsCompleted)
            {
                UnityMainThread.wkr.AddJob(() =>
                {
                    StartCoroutine(PopulateMyAuctionsData(_));
                });
            }
        });
    }

    private IEnumerator PopulateBuyData(Task<GetAuction> _)
    {
        var count = _.Result.data.getAuctions.auctions.Count;

        var item = _.Result.data.getAuctions.auctions[buyItemsLength];
        GameObject go = Instantiate(buyItemPrefab.gameObject, buyItemParent);
        var buyItem = go.GetComponent<BuyItem>();
        StartCoroutine(GetImageFromUrl(item.gameItem.imageUrl, texture =>
        {
            buyItem.itemImage.texture = texture;
        }));
        buyItem.qtyText.text = item.quantity.ToString();
        buyItem.itemNameText.text = item.gameItem.itemName;

        if(item.highBidderProfile != null)
            buyItem.usernameText.text = item.highBidderProfile.name;
        else
            buyItem.usernameText.text = item.sellerProfile.name;

        buyItem.expirationText.text = item.expiration;
        buyItem.buyoutText.text = item.buyout.ToString();
        buyItem.bidText.text = item.bid.ToString();

        yield return null;

        buyItemsLength++;

        if (buyItemsLength < count)
            StartCoroutine(PopulateBuyData(_));
        else
        {
            buyItemsLength = 0;
            DataProvider.Instance.loadingPanel.SetActive(false);
        }
    }

    IEnumerator PopulateSellData(Task<GetInventory> _)
    {
        var count = _.Result.data.getInventory.inventory.Count;

        var item = _.Result.data.getInventory.inventory[sellItemsLength];
        GameObject go = Instantiate(sellItemPrefab.gameObject, sellItemParent);
        var sellItem = go.GetComponent<SellItem>();
        StartCoroutine(GetImageFromUrl(item.gameItem.imageUrl, texture =>
        {
            sellItem.itemImage.texture = texture;
        }));
        sellItem.itemNameText.text = item.gameItem.itemName;
        sellItem.availableText.text = item.ItemCount.ToString();

        yield return null;

        sellItemsLength++;

        if (sellItemsLength < count)
            StartCoroutine(PopulateSellData(_));
        else
        {
            sellItemsLength = 0;
            DataProvider.Instance.loadingPanel.SetActive(false);
        }
    }

    private IEnumerator PopulateMyAuctionsData(Task<GetUserAuction> _)
    {
        var count = _.Result.data.getUserAuctions.auctions.Count;

        var item = _.Result.data.getUserAuctions.auctions[myAuctionsLength];
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

        myAuctionsLength++;

        if (myAuctionsLength < count)
            StartCoroutine(PopulateMyAuctionsData(_));
        else
        {
            myAuctionsLength = 0;
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