using UnityEngine;

public class ToggleCredits : MonoBehaviour
{
    public GameObject CreditsMenu;
    public GameObject CreditsButton;
    public GameObject StartButton;

    public void ShowCredits(bool show)
    {
        CreditsMenu.SetActive(show);
        CreditsButton.SetActive(!show);
        StartButton.SetActive(!show);
    }
}
