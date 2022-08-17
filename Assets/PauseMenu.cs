using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Buttons;
        
    [Header("Ad Buttons")]
    public TextMeshProUGUI DebugText;
    public Image PauseMenuImage;
    public Sprite PauseMenuChange;
    private Sprite PauseMenuOriginal;
    public GameObject SettingsMenu;
    public GameObject PauseMenuGO;
    #endregion
    
    private LevelCompleted LevelCompleted;


    private void Awake()
    {
        PauseMenuOriginal = PauseMenuImage.sprite;
        LevelCompleted = FindObjectOfType<LevelCompleted>().GetComponent<LevelCompleted>();
    }
    public void OnPointerEnter(PointerEventData eventData) => ChangeSprite();
    public void OnPointerExit(PointerEventData eventData) => ResetSprite();
    private void ResetSprite() => PauseMenuImage.sprite = PauseMenuOriginal;
    public void ResumeGame() => PauseMenuGO.SetActive(false);
    

    
    
    public void OpenSettings()
    {
        SettingsMenu.SetActive(true);
        PauseMenuGO.SetActive(false);
    }


    
    public void ChangeSprite()
    {
        PauseMenuImage.sprite = PauseMenuChange;
        
        if (Application.platform is RuntimePlatform.IPhonePlayer or RuntimePlatform.Android)
            StartCoroutine(WaitToResetSprite());
    }



    private IEnumerator WaitToResetSprite()
    {
        yield return new WaitForSeconds(0.25f);
        ResetSprite();
    }
    
    public void ResumeTimer() => LevelCompleted.PauseTimer = false;
}
