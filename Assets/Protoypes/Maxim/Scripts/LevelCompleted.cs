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
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int currentCoins = itemCollector._coinCounter;
        if (collision.tag == "Player")
        {
            currentCoins += coinBonus;
            winScreen.SetActive(true);
            Time.timeScale = 0; 
            StarsAchieved();
            UpdateCoinText(currentCoins);
            UpdateTotalCoinText(currentCoins);
        }
    }

    public void StarsAchieved()
    {
        int healthLeft = playerHealth.CurrentHealth;
        
       
        // float percentage = float.Parse(healthMax.ToString()) / float.Parse(healthLeft.ToString()) * 100f;

        if (healthLeft <= 2)
        {
            stars[0].enabled = true;
        }
        else if (healthLeft <= 4) 
        {
            stars[0].enabled = true;
            stars[1].enabled = true;
        }
        else 
        {
            stars[0].enabled = true;
            stars[1].enabled = true;
            stars[2].enabled = true;
        }
        
    }
    
    public void UpdateCoinText(int value) => coinText.text = $"{value}";

    public void UpdateTotalCoinText(int value) => totalCoinText.text = $"Total Coins: {value}";



}
