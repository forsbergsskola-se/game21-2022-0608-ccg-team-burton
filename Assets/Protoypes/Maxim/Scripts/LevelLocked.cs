using UnityEngine;
using UnityEngine.UI;

public class LevelLocked : MonoBehaviour
{
    [SerializeField] public bool Unlocked;
    public Image LockImage;
    public Image[] Stars;
    
    

    private void Update()
    {
        UpdateLevelImage(); 
        UpdateLevelStatus();
    } 

    private void UpdateLevelStatus()
    {
        int previousLevelNum = int.Parse(gameObject.name) - 1;
        if (PlayerPrefs.GetInt("Lv" + previousLevelNum.ToString()) > 0)
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
        }
    }
}
