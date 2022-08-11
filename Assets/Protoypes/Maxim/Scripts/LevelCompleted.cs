using Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleted : MonoBehaviour
{
    public TextMeshProUGUI totalCoinText;
    public TextMeshProUGUI coinText;
    public GameObject winScreen;
    public Image[] stars;
    public Health playerHealth;
    public ItemCollector itemCollector;
    public int coinBonus = 500;

    public int currentStarsNum = 0;
    public int levelIndex;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int currentCoins = itemCollector._coinCounter;
        
        if (collision.tag == "Player")
        {
            currentCoins += coinBonus;
            
            //Save coins to inventory
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()) + currentCoins);

            winScreen.SetActive(true);
            Time.timeScale = 0; 
            StarsAchieved();
            UpdateCoinText(currentCoins);
            UpdateTotalCoinText(currentCoins);
            SaveStars(currentStarsNum);
        }
    }

    

    public void StarsAchieved()
    {
        int healthLeft = playerHealth.CurrentHealth;

        
        if (healthLeft <= 2)
        {
            stars[0].enabled = true;
            stars[1].enabled = false;
            stars[2].enabled = false;
            currentStarsNum = 1;
        }
        else if (healthLeft <= 4) 
        {
            stars[0].enabled = true;
            stars[1].enabled = true;
            stars[2].enabled = false;
            currentStarsNum = 2;
        }
        else 
        {
            stars[0].enabled = true;
            stars[1].enabled = true;
            stars[2].enabled = true;
            currentStarsNum = 3;
        }

    }

    public void SaveStars(int starsNum)
    {
        currentStarsNum = starsNum;
        if (currentStarsNum > PlayerPrefs.GetInt("Lv" + levelIndex))
        {
            PlayerPrefs.SetInt("Lv" + levelIndex, starsNum );
            Debug.Log("saved");
        }
        
    }
    
    public void UpdateCoinText(int value) => coinText.text = $"{value}";

    public void UpdateTotalCoinText(int value) => totalCoinText.text = $"Total Coins: {value}";



}
