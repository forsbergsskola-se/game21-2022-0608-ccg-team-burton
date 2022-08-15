using Entity;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
    public int CurrentStarsNum = 0;
    public int LevelIndex;
    public GameObject[] Enemies;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        var currentCoins = ItemCollector._coinCounter;

        if (collision.CompareTag("Player"))
        {
            currentCoins += CoinBonus;

            //Save coins to inventory
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(),
                PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()) + currentCoins);


            ItemCollector._coinCounter += CoinBonus;


            WinScreen.SetActive(true);

            Time.timeScale = 0;
            StarsAchieved();
            UpdateCoinText(currentCoins);
            UpdateTotalCoinText(currentCoins);
            SaveStars(CurrentStarsNum);
        }

    }

    private void StarsAchieved()
    {
        var healthLeft = PlayerHealth.CurrentHealth;

        if (healthLeft == 6)
        {
            CurrentStarsNum = 1;
        }
        // else if (healthLeft <= 4)
        // {
        //     currentStarsNum = 0;
        // }
        // else
        // {
        //     currentStarsNum = 0;
        // }
    }

        private void SaveStars(int starsNum)
        {
            CurrentStarsNum = starsNum;

           

            if (CurrentStarsNum > PlayerPrefs.GetInt("Lv" + LevelIndex))
            {
                PlayerPrefs.SetInt("Lv" + LevelIndex, starsNum);
                Debug.Log("Saved as " + PlayerPrefs.GetInt("Lv" + LevelIndex, starsNum).ToString());
            }

        }

        private void UpdateCoinText(int value) => CoinText.text = $"{value}";

        private void UpdateTotalCoinText(int value) => TotalCoinText.text = $"Total Coins : {value}";
    }
