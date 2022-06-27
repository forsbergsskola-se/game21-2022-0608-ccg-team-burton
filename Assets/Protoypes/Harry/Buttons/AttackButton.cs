using GamePlay.Entities.Player;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    private PlayerInputController _playerInputController;

    private void Awake()
        {
            _playerInputController = FindObjectOfType<PlayerInputController>();
        }
    
        public void OnPointerDownAttack()
        {
            _playerInputController.AttackDownInput = true;
        }
    
        public void OnPointerUpAttack()
        {
            _playerInputController.AttackDownInput = false;
        }
}
