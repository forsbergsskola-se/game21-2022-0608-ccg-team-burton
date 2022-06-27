using UnityEngine;
using UnityEngine.UI;

public class UnlockLevel : MonoBehaviour
{
    [SerializeField] public bool Unlocked;
    public Image LockImage;
    public Image[] Stars;

    public Sprite fullStar;

    private void Update()
    {
        UpdateLevelImage(); 
        UpdateLevelStatus();
    } 

    private void UpdateLevelStatus()
    {
        int previousLevelIndex = int.Parse(gameObject.name) - 1;
        if (PlayerPrefs.GetInt("Lv" + previousLevelIndex) > 0)
        {
            Unlocked = true;
            Debug.Log("Popped off");
        }
    }
    
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

            for (int i = 0; i < PlayerPrefs.GetInt("Lv" + gameObject.name); i++)
            {
                Stars[i].gameObject.GetComponent<Image>().sprite = fullStar;
            }
        }
    }
}
