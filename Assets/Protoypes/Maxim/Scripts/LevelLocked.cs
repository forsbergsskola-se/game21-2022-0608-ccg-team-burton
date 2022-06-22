using UnityEngine;
using UnityEngine.UI;

public class LevelLocked : MonoBehaviour
{
    [SerializeField] public bool Unlocked;
    public Image LockImage;
    public Image[] Stars;

    private void Update() => UpdateLevelImage();  //maybe move later
    
    private void UpdateLevelImage()
    {
        if (!Unlocked)
        {
            LockImage.enabled = true;
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].enabled = false;
            }
        }
        else
        {
            LockImage.enabled = false;
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].enabled = true;
            }
        }
    }
}
