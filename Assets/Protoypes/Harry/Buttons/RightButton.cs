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
        Debug.Log("Walking right");
    }
    
    
    
    public void OnPointerDownRight()
    {
        _playerInputController.WalkRightDownInput = false;
        Debug.Log("Stopped walking right");
    }
}
