using System.Collections;
using Protoypes.Harry;
using UnityEngine;

public class TimeLineHandler : MonoBehaviour
{
    [SerializeField] private GameObject _timeLine;
    [SerializeField] private NewMovement _playerMovement;

    private GameObject HUD;
    private GameObject LeftButton;
    private GameObject RightButton;
    private GameObject JumpButton;
    private GameObject AttackButton;
    private GameObject Timer;
    private GameObject PauseMenu;
    private GameObject Coin;
    private GameObject Health;
    
    private LevelCompleted _levelCompleted;
    
    [SerializeField] float _secondsToActivate;

    private void Awake()
    {
        HUD = FindObjectOfType<MovementButtonsHitboxManager>().gameObject;
        LeftButton = HUD.transform.Find("Left Button").gameObject;
        RightButton = HUD.transform.Find("Right Button").gameObject;
        AttackButton = HUD.transform.Find("Attack Button").gameObject;
        JumpButton = HUD.transform.Find("Jump Button").gameObject;
        Timer = HUD.transform.Find("Time").gameObject;
        PauseMenu = HUD.transform.Find("Pause Menu").gameObject;
        Coin = HUD.transform.Find("Coin Image").gameObject;
        Health = HUD.transform.Find("Health").gameObject;
        _levelCompleted = FindObjectOfType<LevelCompleted>();
    }

    private void Start()
    {
        //If timeline has run once this level
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.TimeLineActive.ToString()) == 0)
        {
            _timeLine.SetActive(true);
            StartCoroutine(ActivationRoutine());
            return;
        }

        //If timeline has been run on this level previously
        ToggleMovementTimerAndUI(true);
    }
    
    
    
    private IEnumerator ActivationRoutine()
    {
        //Wait for 14 secs. 
        yield return new WaitForSeconds(_secondsToActivate);
        ToggleMovementTimerAndUI(true);
        PlayerPrefs.SetInt(PlayerPrefsKeys.TimeLineActive.ToString(), 1);
    }

    

    private void ToggleMovementTimerAndUI(bool toggle)
    {
        _playerMovement.enabled = true;
        LeftButton.SetActive(toggle);
        RightButton.SetActive(toggle);
        JumpButton.SetActive(toggle);
        AttackButton.SetActive(toggle);
        Timer.SetActive(toggle);
        _levelCompleted.PauseTimer = !toggle;
        PauseMenu.SetActive(toggle);
        Coin.SetActive(toggle);
        Health.SetActive(toggle);
    }
    
    public void ResetTimelineActive() => PlayerPrefs.SetInt(PlayerPrefsKeys.TimeLineActive.ToString(), 0);

}
