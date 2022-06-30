using UnityEngine;
using UnityEngine.UI;


public class UnlockLevel : MonoBehaviour, I_Saveable 

{
    [SerializeField] public bool Unlocked;
    public Image LockImage;
    public Image[] Stars;

    public Sprite fullStar;
    SavingWrapper wrapper;

    private void Start()
    {
        wrapper = FindObjectOfType<SavingWrapper>();
    }
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
        wrapper.Save();
    }
    
    private void UpdateLevelImage()
    {
        if (!Unlocked)// if unlock is false, the level is locked
        {
            LockImage.enabled = true;
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].enabled = false;
            }
            //save stats
           
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
        wrapper.Save();
    }
    
}
