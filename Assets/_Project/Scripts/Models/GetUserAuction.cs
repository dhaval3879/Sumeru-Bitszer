using System.Collections.Generic;

public class GetUserAuction
{
    public GetUserAuctionsData data { get; set; }
}

public class GetUserAuctionsData
{
    public GetUserAuctions getUserAuctions { get; set; }
}

public class GetUserAuctions
{
    public string nextToken { get; set; }
    public List<Auction> auctions { get; set; }
}

public class Auction
{
    public double bid { get; set; }
    public double buyout { get; set; }
    public string createdAt { get; set; }
    public string expiration { get; set; }
    public HighBidderProfile highBidderProfile { get; set; }
    public SellerProfile sellerProfile { get; set; }
    public GameItem gameItem { get; set; }
    public string id { get; set; }
    public int quantity { get; set; }
}

public class HighBidderProfile
{
    public int buyCount { get; set; }
    public string createdAt { get; set; }
    public int postedAuctionsCount { get; set; }
    public string title { get; set; }
    public int soldCount { get; set; }
    public double soldAmount { get; set; }
    public string screenName { get; set; }
    public string name { get; set; }
    public string backgroundImageUrl { get; set; }
    public double buyAmount { get; set; }
    public string id { get; set; }
    public string imageUrl { get; set; }
}

public class SellerProfile
{
    public int buyCount { get; set; }
    public string createdAt { get; set; }
    public int postedAuctionsCount { get; set; }
    public string title { get; set; }
    public int soldCount { get; set; }
    public double soldAmount { get; set; }
    public string screenName { get; set; }
    public string name { get; set; }
    public string backgroundImageUrl { get; set; }
    public double buyAmount { get; set; }
    public string id { get; set; }
    public string imageUrl { get; set; }
}