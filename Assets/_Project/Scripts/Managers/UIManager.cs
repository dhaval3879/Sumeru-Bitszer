using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, homePanel, myAuctionPanel;

    private void OnEnable()
    {
        UserAuth.OnAccessTokenReceived += OnAccessTokenReceived;
    }

    private void OnDisable()
    {
        UserAuth.OnAccessTokenReceived -= OnAccessTokenReceived;
    }

    private void OnAccessTokenReceived()
    {
        OpenHomePanel();
    }

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
        signupPanel.SetActive(false);
        loginPanel.SetActive(false);
        homePanel.SetActive(true);
        myAuctionPanel.SetActive(false);
    }

    public void OpenMyAuctionPanel()
    {
        signupPanel.SetActive(false);
        loginPanel.SetActive(false);
        homePanel.SetActive(false);
        myAuctionPanel.SetActive(true);
    }
}