using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject loginPanel, signupPanel;

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
    }

    public void OpenSignupPanel()
    {
        signupPanel.SetActive(true);
        loginPanel.SetActive(false);
    }
}