using Protoypes.Harry;
using UnityEngine;

public class OneWayUpPlatform : MonoBehaviour
{
   private BoxCollider2D _collider;
   private NewMovement _player;
   public float UnlockTime;
   private float _unlockBuffer = float.MinValue;


   private void Awake() => _collider = GetComponentInParent<BoxCollider2D>();

   
   
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
