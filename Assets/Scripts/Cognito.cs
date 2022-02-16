using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Threading.Tasks;
using Amazon.Extensions.CognitoAuthentication;

public class Cognito : MonoBehaviour
{
    //UI elements 
    public Button SignupButton;
    public Button SignInButton;
    public InputField EmailField;
    public InputField PasswordField;
    public InputField UsernameField;
    public Text StatusText;

    const string PoolID = ""; //insert your Cognito User Pool ID, found under General Settings
    const string AppClientID = ""; //insert App client ID, found under App Client Settings
    static Amazon.RegionEndpoint Region = Amazon.RegionEndpoint.USEast1; //insert region user pool was created in, if it is different than US EAST 1

    bool signInSuccessful;


    void Start()
    {
        SignupButton.onClick.AddListener(on_signup_click);
        SignInButton.onClick.AddListener(on_signin_click);
        signInSuccessful = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (signInSuccessful)
            SceneManager.LoadScene(1);
    }

    public void on_signup_click()
    {
        Debug.Log("sign up button clicked");
        _ = SignUpMethodAsync();
    }

    public void on_signin_click()
    {
        Debug.Log("sign in button clicked");
        _ = SignInUser();
    }

    //Method that creates a new Cognito user
    private async Task SignUpMethodAsync()
    {

    }

    //Method that signs in Cognito user 
    private async Task SignInUser()
    {

    }
}