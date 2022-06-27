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
        _playerInputController.WalkRightUpInput = true;
        _playerInputController.WalkRightDownInput = true;
    }
    
    
    
    public void OnPointerDownRight()
    {
        _playerInputController.WalkRightDownInput = false;
        _playerInputController.WalkRightUpInput = false;
    }
}
