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
    #endregion

    private void Awake() => PauseMenuOriginal = PauseMenuImage.sprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        DebugText.text = $"Hovering over {name}";
        PauseMenuImage.sprite = PauseMenuChange;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DebugText.text = "Nothing Selected";
        PauseMenuImage.sprite = PauseMenuOriginal;

    }
}
