using Protoypes.Harry;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OneWayUpPlatform : MonoBehaviour
{
   private TilemapCollider2D _collider;
   private NewMovement _player;
   private float UnlockTime;
   private float _unlockBuffer = float.MinValue;


   private void Awake() => _collider = GetComponentInParent<TilemapCollider2D>();

   
   
   private void Update()
   {
      if (_player == null) return; // only working otherwise
      CheckToLetPlayerThrough(); 
   }

   
   
   private void CheckToLetPlayerThrough()
   {
      if (_player._currentVerticalSpeed < 0)
         _unlockBuffer = Time.time + UnlockTime; // time boxCollider will be disabled
      
      _collider.enabled = _player._currentVerticalSpeed <= 0 && Time.time >= _unlockBuffer; // disable collider until condition is met
   }

   
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.TryGetComponent(out NewMovement player)){}
         _player = player; // override null check
   }

   
   
   private void OnTriggerExit2D(Collider2D other)
   {
      if (other.gameObject.TryGetComponent(out NewMovement player))
         _player = null; // reset null check
   }
}
