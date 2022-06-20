using GamePlay.Entities.Player;
using UnityEngine;

public class LeftButton : MonoBehaviour
{
    private PlayerInputController _playerInputController;

    private void Awake()
    {
        _playerInputController = FindObjectOfType<PlayerInputController>();
    }



    public void OnPointerUpLeft()
    {
        _playerInputController.WalkLeftDownInput = true;
        Debug.Log("Walking left");
    }
    
    
    
    public void OnPointerDownLeft()
    {
        _playerInputController.WalkLeftDownInput = false;
        Debug.Log("Stopped walking left");
    }
}
