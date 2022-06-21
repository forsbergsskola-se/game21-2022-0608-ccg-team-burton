using GamePlay.Entities.Player;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
   private CircleCollider2D _collider;
   private PlayerInputController _playerInputController;

   private void Awake()
   {
      _collider = GetComponent<CircleCollider2D>();
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.TryGetComponent(out PlayerInputController player))
         _playerInputController = player;
   }
}
