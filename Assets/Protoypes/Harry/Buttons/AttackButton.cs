using GamePlay.Entities.Player;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    private PlayerInputController _playerInputController;

    private void Awake()
        {
            _playerInputController = FindObjectOfType<PlayerInputController>();
        }
    
        public void OnPointerUpAttack()
        {
            _playerInputController.AttackUpInput = true;
            _playerInputController.AttackDownInput = false;
            Debug.Log("Givin em the whickity whack");
        }
    
        public void OnPointerDownAttack()
        {
            _playerInputController.AttackDownInput = true;
            _playerInputController.AttackUpInput = false;
        }
}
