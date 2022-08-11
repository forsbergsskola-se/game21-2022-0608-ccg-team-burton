using Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleted : MonoBehaviour
{

    public TextMeshProUGUI TotalCoinText;
    public TextMeshProUGUI CoinText;
    public GameObject WinScreen;
    public Image[] Stars;
    public Health PlayerHealth;
    public ItemCollector ItemCollector;
    public int CoinBonus = 500;
    public int currentStarsNum = 0;
    public int levelIndex;
    
   
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int currentCoins = ItemCollector._coinCounter;
        
        if (collision.tag == "Player")
        {
            currentCoins += CoinBonus;

            //Save coins to inventory
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()) + currentCoins);

            WinScreen.SetActive(true);

            Time.timeScale = 0; 
            StarsAchieved();
            UpdateCoinText(currentCoins);
            UpdateTotalCoinText(currentCoins);
            SaveStars(currentStarsNum);
        }
    }

    
    public void StarsAchieved()
    {
        int healthLeft = PlayerHealth.CurrentHealth;
        
       
        // float percentage = float.Parse(healthMax.ToString()) / float.Parse(healthLeft.ToString()) * 100f;
        
        if (healthLeft <= 2)
        {
            Stars[0].enabled = true;
        }
        else if (healthLeft <= 4) 
        {
            Stars[0].enabled = true;
            Stars[1].enabled = true;
        }
        else 
        {
            Stars[0].enabled = true;
            Stars[1].enabled = true;
            Stars[2].enabled = true;

            currentStarsNum = 1;
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
    
    public void UpdateCoinText(int value) => CoinText.text = $"{value}";

    public void UpdateTotalCoinText(int value) => TotalCoinText.text = $"Total Coins: {value}";
}
