using Protoypes.Harry;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    private BoxCollider2D _collider;
    private NewMovement _player;
    
    
    private void Awake() => _collider = GetComponentInParent<BoxCollider2D>();

   
   
    private void Update()
    {
        if (_player == null) return; // only working otherwise
        CheckToLetPlayerThrough(); 
    }

   
   
    private void CheckToLetPlayerThrough()
    {
        if (_player._isGrounded)
            _collider.enabled = false;
    }

   
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out NewMovement player)){}
        _player = player; // override null check
    }

   
   
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out NewMovement player))
        {
            player = null;
            _collider.enabled = true;
        }
    }
}
