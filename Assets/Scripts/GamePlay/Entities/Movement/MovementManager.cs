using GamePlay.Entities.Player;
using UnityEngine;

namespace GamePlay.Entities.Movement
{
    public class MovementManager : MonoBehaviour
    {
        private Animator _animator;
        private SpriteRenderer _renderer;
        private Collider2D _collider;
        private Rigidbody2D _rb;
        private CommandContainer _commandContainer;
        private GroundChecker _groundChecker;

        public float baseMovementSpeed;
        public float airborneMovementSpeed;
        public float chargingJumpSpeed;
        public float jumpForce;
        

        private void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
            _renderer = gameObject.GetComponent<SpriteRenderer>();
            _collider = gameObject.GetComponent<CircleCollider2D>();
            
            if (gameObject.GetComponent<Rigidbody2D>())
                _rb = gameObject.GetComponent<Rigidbody2D>();
        }

        
        private void Update()
        {
            HandleWalking();
        }


        private void HandleWalking()
        {
            if (_groundChecker.IsGrounded)
                baseMovementSpeed = airborneMovementSpeed;

            if (_groundChecker.IsGrounded && _commandContainer.JumpCommand)
                baseMovementSpeed = chargingJumpSpeed;

            _rb.velocity = new Vector2(_commandContainer.walkCommand * baseMovementSpeed, _rb.velocity.y);
        }
    }
    
    
    
    
}

