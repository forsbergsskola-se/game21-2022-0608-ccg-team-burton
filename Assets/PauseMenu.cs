using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Buttons;
        
    [Header("Ad Buttons")]
    public TextMeshProUGUI DebugText;

    #endregion
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        DebugText.text = $"Hovering over {name}";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DebugText.text = "Nothing Selected";
    }
}
