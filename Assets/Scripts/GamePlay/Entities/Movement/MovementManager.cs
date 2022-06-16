using GamePlay.Entities.Player;
using UnityEngine;

namespace GamePlay.Entities.Movement
{
    public class MovementManager : MonoBehaviour
    {
        private Animator _animator;
        private SpriteRenderer _renderer;
        private Rigidbody2D _rb;
        private CommandContainer _commandContainer;
        private GroundChecker _groundChecker;

        #region Walking;
        [Header("WALKING")]
        private float _baseMovementSpeed;
        public float Acceleration = 80f;
        public float DeAcceleration = 55f;
        public float WalkClamp = 12f;
        // public bool IsWalkingLeft;
        // public bool IsWalkingRight;
        #endregion

        #region Jumping;

        [Header("JUMPING")] 
        public float BaseJumpSpeed;
        private float _jumpHeight;
        public bool Jumping;


        private Vector3 _lastPos;
        public Vector3 _jumpVelocity { get; private set; }
        #endregion
        


        private void Awake()
        {
            //_animator = gameObject.GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            _commandContainer = GetComponent<CommandContainer>();
            _groundChecker = GetComponent<GroundChecker>();
            _rb = GetComponent<Rigidbody2D>();
            Acceleration *= 100f;
            DeAcceleration *= 100f;
        }


        private void Update()
        {
            CalculateJumpVelocity();
            HandleJumping();
            HandleWalking();
        }

        private void CalculateJumpVelocity()
        {
            var pos = transform.position;
            _jumpVelocity = (pos - _lastPos) / Time.deltaTime;
            _lastPos = pos;
        }


        private void HandleJumping()
        {

            if (!_commandContainer.JumpCommand) return;
            if (_groundChecker.IsGrounded)
            {
                _jumpHeight = BaseJumpSpeed * Time.deltaTime;
                Jumping = true;
                _rb.velocity = new Vector2(_baseMovementSpeed, _jumpHeight);
            }
            else
            {
                _jumpHeight = _baseMovementSpeed;
                Jumping = false;
            }
        }


        private void HandleWalking()
        {
            _renderer.flipX = _commandContainer.WalkCommand switch
            {
                > 0 => false, 
                < 0 => true,
                _ => _renderer.flipX
            };

            if (_commandContainer.WalkCommand != 0)
            {
                // set walk speed
                _baseMovementSpeed = _commandContainer.WalkCommand * Acceleration * Time.deltaTime;
                // clamp walk speed
                _baseMovementSpeed = Mathf.Clamp(_baseMovementSpeed, -WalkClamp, WalkClamp);
                
            }

            else
                _baseMovementSpeed = Mathf.MoveTowards // slow down when no input
                    (_baseMovementSpeed, 0, DeAcceleration * Time.deltaTime);
            
            _rb.velocity = new Vector2(_baseMovementSpeed, _rb.velocity.y);
            
            
            
        }

        
    }
}

