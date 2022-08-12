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
        var currentCoins = ItemCollector._coinCounter;

        if (!collision.CompareTag("Player")) return;
        currentCoins += CoinBonus;
        ItemCollector._coinCounter += CoinBonus;

        //Save coins to inventory
        PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()) + currentCoins);

        WinScreen.SetActive(true);

        Time.timeScale = 0; 
        StarsAchieved();
        UpdateCoinText(currentCoins);
        UpdateTotalCoinText(currentCoins);
        SaveStars(currentStarsNum);
    }


    private void StarsAchieved()
    {
        var healthLeft = PlayerHealth.CurrentHealth;
        // float percentage = float.Parse(healthMax.ToString()) / float.Parse(healthLeft.ToString()) * 100f;

        switch (healthLeft)
        {
            case <= 2:
                Stars[0].enabled = true;
                break;
            
            case <= 4:
                Stars[0].enabled = true;
                Stars[1].enabled = true;
                break;
            
            default:
                Stars[0].enabled = true;
                Stars[1].enabled = true;
                Stars[2].enabled = true;

                currentStarsNum = 1;
                break;
        }
    }

    private void SaveStars(int starsNum)
    {
        currentStarsNum = starsNum;
        if (currentStarsNum <= PlayerPrefs.GetInt("Lv" + levelIndex)) return;
        
        PlayerPrefs.SetInt("Lv" + levelIndex, starsNum );
        Debug.Log("saved");
    }

    private void UpdateCoinText(int value) => CoinText.text = $"{value}";

    private void UpdateTotalCoinText(int value) => TotalCoinText.text = $"Total Coins : {value}";
}
