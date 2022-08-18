using Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleted : MonoBehaviour
{
    public TextMeshProUGUI BonusCoinsText;
    public TextMeshProUGUI TotalCoinText;
    public TextMeshProUGUI CoinText;
    public GameObject WinScreen;
    public Image[] Stars;
    public Health PlayerHealth;
    public ItemCollector ItemCollector;
    private int _coinBonus = 0;
    public float GoalTime;
    
    public int CurrentStarsNum;
    public int LevelIndex;
    
    public TMP_Text TextTimer;
    private float _timer;
    public bool PauseTimer;


    private void Awake() => PauseTimer = true;

    private void Update()
    {
        if (PauseTimer) return;
        else
        {
            _timer += Time.deltaTime;
            DisplayTimer();
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision){
        var currentCoins = ItemCollector._coinCounter;
        var bonusCoins = _coinBonus;

        if (collision.CompareTag("Player")){
            currentCoins += _coinBonus;

            //Save coins to inventory
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(),
                PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()) + currentCoins);


            ItemCollector._coinCounter += _coinBonus;


            WinScreen.SetActive(true);


            StarsAchieved();
            ShowStars();
            UpdateCoinText(currentCoins);
            UpdateTotalCoinText(currentCoins);
            UpdateBonusCoinsText(bonusCoins);
            SaveStars(CurrentStarsNum);
            collision.gameObject.SetActive(false); //disables the player
        }
    }

    private void DisplayTimer()
    {
        var minutes = Mathf.FloorToInt(_timer / 60.0f);
        var seconds = Mathf.FloorToInt(_timer - minutes * 60);
        TextTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void StarsAchieved(){
        var healthLeft = PlayerHealth.CurrentHealth;

        if (healthLeft >= 6 )
        {
            CurrentStarsNum += 1;
            _coinBonus += 150;
        }

        if (_timer < GoalTime)
        {
            CurrentStarsNum += 1;
            _coinBonus += 150;
        }

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(enemies.Length + " Left to kill");

        if (enemies.Length <= 0)
        {
            CurrentStarsNum += 1;
            _coinBonus += 150;
        }


    }

    void SaveStars(int starsNum){
        CurrentStarsNum = starsNum;


        if (CurrentStarsNum > PlayerPrefs.GetInt("Lv" + LevelIndex)){
            PlayerPrefs.SetInt("Lv" + LevelIndex, starsNum);
            Debug.Log("Saved as " + PlayerPrefs.GetInt("Lv" + LevelIndex, starsNum));
        }
    }

    void ShowStars()
    {
        var starsNum = CurrentStarsNum;

        if (starsNum == 1)
        {

            Stars[0].enabled = true;
            Debug.Log(starsNum);
        }
        else if (starsNum == 2)
        {
            Stars[0].enabled = true;
            Stars[1].enabled = true;
        }
        else if (starsNum == 3)
        {
            Stars[0].enabled = true;
            Stars[1].enabled = true;
            Stars[2].enabled = true;
        }
        Debug.Log("CurrentStars: " + CurrentStarsNum);
    }

    void UpdateCoinText(int value){
        CoinText.text = $"{value}";
    }

    void UpdateTotalCoinText(int value){
        TotalCoinText.text = $"Total Coins : {value}";
    }

    void UpdateBonusCoinsText(int value) {
        BonusCoinsText.text = $"Bonus Coins : {value}";
    }
}