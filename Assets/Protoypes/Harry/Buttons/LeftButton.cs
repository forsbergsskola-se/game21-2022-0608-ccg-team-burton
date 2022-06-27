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
    }

    public void OnPointerUpLeft()
    {
        _playerInputController.WalkLeftDownInput = false;
    }
}