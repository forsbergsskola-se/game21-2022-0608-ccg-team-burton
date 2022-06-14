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
        

        private void Awake()
        {
            //_animator = gameObject.GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<CapsuleCollider2D>();
            _commandContainer = GetComponent<CommandContainer>();
            _groundChecker = GetComponent<GroundChecker>();
            _rb = GetComponent<Rigidbody2D>();
        }

        
        private void Update()
        {
            HandleWalking();
            HandleJumping();
        }


        private void HandleJumping()
        {
            if (!_commandContainer.JumpCommand) return;
            if (!_groundChecker.IsGrounded) return;
            
            _rb.AddForce(Vector2.up * jumpForce);
        }


        private void HandleWalking()
        {
            var movementSpeed = 0f;
            if (_groundChecker.IsGrounded)
                movementSpeed = baseMovementSpeed;

            /*if (_groundChecker.IsGrounded && _commandContainer.JumpCommand)
                movementSpeed = chargingJumpSpeed;*/

            else if (!_groundChecker.IsGrounded)
                movementSpeed = airborneMovementSpeed;

            _rb.velocity = new Vector3(_commandContainer.walkCommand * movementSpeed, _rb.velocity.y, 0);
        }
    }
    
    
    
    
}

