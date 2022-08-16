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
    
    public int CurrentStarsNum;
    public int LevelIndex;
    
    public TMP_Text TextTimer;
    float _timer;

    void Update(){
        _timer += Time.deltaTime;
        DisplayTimer();
    }

    void OnTriggerEnter2D(Collider2D collision){
        var currentCoins = ItemCollector._coinCounter;

        if (collision.CompareTag("Player")){
            currentCoins += CoinBonus;

            //Save coins to inventory
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(),
                PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()) + currentCoins);


            ItemCollector._coinCounter += CoinBonus;


            WinScreen.SetActive(true);


            StarsAchieved();
            UpdateCoinText(currentCoins);
            UpdateTotalCoinText(currentCoins);
            SaveStars(CurrentStarsNum);
            collision.gameObject.SetActive(false); //disables the player
        }
    }

    public void DisplayTimer(){
        var minutes = Mathf.FloorToInt(_timer / 60.0f);
        var seconds = Mathf.FloorToInt(_timer - minutes * 60);
        TextTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void StarsAchieved(){
        var healthLeft = PlayerHealth.CurrentHealth;
       

        if (healthLeft >= 6 )
        {
            CurrentStarsNum += 1;
        }

        if (_timer < 120f)
        {
            CurrentStarsNum += 1;
        }

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(enemies.Length);

        if (enemies.Length <= 0)
        {
            CurrentStarsNum += 1;
        }


    }

    void SaveStars(int starsNum){
        CurrentStarsNum = starsNum;


        if (CurrentStarsNum > PlayerPrefs.GetInt("Lv" + LevelIndex)){
            PlayerPrefs.SetInt("Lv" + LevelIndex, starsNum);
            Debug.Log("Saved as " + PlayerPrefs.GetInt("Lv" + LevelIndex, starsNum));
        }
    }

    void UpdateCoinText(int value){
        CoinText.text = $"{value}";
    }

    void UpdateTotalCoinText(int value){
        TotalCoinText.text = $"Total Coins : {value}";
    }
}