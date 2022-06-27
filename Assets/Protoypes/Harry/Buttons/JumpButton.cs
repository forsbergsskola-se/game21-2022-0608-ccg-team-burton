using GamePlay.Entities.Player;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    private PlayerInputController _playerInputController;

    private void Awake()
    {
        _playerInputController = FindObjectOfType<PlayerInputController>();
    }

    public void OnPointerDownJump()
    {
        _playerInputController.JumpDownInput = true;
    }
    
    public void OnPointerUpJump()
    {
        _playerInputController.JumpDownInput = false;
    }
}
