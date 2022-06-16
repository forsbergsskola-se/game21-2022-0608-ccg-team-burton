
using System;
using GamePlay.Entities.Player;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerInputController _playerInputController;

    private void Awake()
    {
        _playerInputController = FindObjectOfType<PlayerInputController>();
    }

    public void OnPointerUpJump()
    {
        _playerInputController.JumpUpInput = true;
        _playerInputController.JumpDownInput = false;
        Debug.Log("Jump Released");
    }

    public void OnPointerDownJump()
    {
        _playerInputController.JumpDownInput = true;
        _playerInputController.JumpUpInput = false;
        
        Debug.Log("Jump Pressed");
    }
}
