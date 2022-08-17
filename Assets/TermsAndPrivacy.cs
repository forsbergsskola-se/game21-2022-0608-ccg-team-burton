using UnityEngine;

public class TermsAndPrivacy : MonoBehaviour
{
    public void OpenTOS() => Application.OpenURL("https://www.termsfeed.com/live/6ea43874-a19b-407d-bfd9-6d83b30679bf");
    public void OpenPrivacy() => Application.OpenURL("https://www.freeprivacypolicy.com/live/154a6107-58ca-4482-bcdb-44488ac064b9");
}
