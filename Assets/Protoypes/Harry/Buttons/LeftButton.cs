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
        _playerInputController.WalkLeftUpInput = false;
        Debug.Log("Walking left");
    }
    
    
    
    public void OnPointerDownLeft()
    {
        _playerInputController.WalkLeftDownInput = false;
        _playerInputController.WalkLeftUpInput = true;
        Debug.Log("Stopped walking left");
    }
}
