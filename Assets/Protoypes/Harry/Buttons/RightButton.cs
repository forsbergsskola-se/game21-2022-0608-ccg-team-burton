using GamePlay.Entities.Player;
using UnityEngine;

public class RightButton : MonoBehaviour
{
    private PlayerInputController _playerInputController;

    private void Awake()
    {
        _playerInputController = FindObjectOfType<PlayerInputController>();
    }



    public void OnPointerUpRight()
    {
        _playerInputController.WalkRightDownInput = true;
        _playerInputController.WalkRightUpInput = false;
        Debug.Log("Walking right");
    }
    
    
    
    public void OnPointerDownRight()
    {
        _playerInputController.WalkRightDownInput = false;
        _playerInputController.WalkRightUpInput = true;
        Debug.Log("Stopped walking right");
    }
}
