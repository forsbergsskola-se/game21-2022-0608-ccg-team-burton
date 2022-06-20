using GamePlay.Entities.Player;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    private PlayerInputController _playerInputController;

    private void Awake()
    {
        _playerInputController = FindObjectOfType<PlayerInputController>();
    }

    public void OnPointerUpJump()
    {
        _playerInputController.JumpUpInput = true;
        _playerInputController.JumpDownInput = false;
    }

    public void OnPointerDownJump()
    {
        _playerInputController.JumpDownInput = true;
        _playerInputController.JumpUpInput = false;
    }
}
