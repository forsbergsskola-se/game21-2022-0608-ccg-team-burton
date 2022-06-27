using GamePlay.Entities.Player;
using UnityEngine;

public class LeftButton : MonoBehaviour
{ 
    private PlayerInputController _playerInputController;

    private void Awake()
    {
        _playerInputController = FindObjectOfType<PlayerInputController>();
    }

    public void OnPointerDownLeft()
    {
        _playerInputController.WalkLeftDownInput = true;
        _playerInputController.WalkLeftUpInput = false;
    }

    public void OnPointerUpLeft()
    {
        _playerInputController.WalkLeftUpInput = true;
        _playerInputController.WalkLeftDownInput = false;
    }
}