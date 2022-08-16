using System;
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

    public TMP_Text TextTimer;
    private float _timer = 0.0f;

    private void Update()
    {
        _timer += Time.deltaTime;
        DisplayTimer();
    }

    public void DisplayTimer()
    {
        int minutes = Mathf.FloorToInt(_timer / 60.0f);
        int seconds = Mathf.FloorToInt(_timer - minutes * 60);
        TextTimer.text = String.Format("{0:00}:{1:00}", minutes, seconds);
    }

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

        if (healthLeft >= 6)
        {
            CurrentStarsNum = 1;
        }

        if (_timer < 120f)
        {
            CurrentStarsNum = 2;
        }
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
