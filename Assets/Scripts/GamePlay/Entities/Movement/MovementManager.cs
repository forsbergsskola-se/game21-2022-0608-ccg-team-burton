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

        #region Walking;
        [Header("WALKING")]
        public float BaseMovementSpeed;
        public float Acceleration = 80f;
        public float DeAcceleration = 55f;
        #endregion

        #region Jumping;
        [Header("JUMPING")]
        public float AirborneMovementSpeed;
        [HideInInspector] public float chargingJumpSpeed;
        public float JumpForce;
        private Vector3 _lastPos;
        public Vector3 _jumpVelocity { get; private set; }
        #endregion
        


        private void Awake()
        {
            //_animator = gameObject.GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<CapsuleCollider2D>();
            _commandContainer = GetComponent<CommandContainer>();
            _groundChecker = GetComponent<GroundChecker>();
            _rb = GetComponent<Rigidbody2D>();
            Acceleration *= 100f;
            DeAcceleration *= 100f;
        }


        private void Update()
        {
            CalculateJumpVelocity();
            HandleWalking();
            HandleJumping();
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
            if (!_groundChecker.IsGrounded) return;
            _rb.AddForce(Vector2.up * JumpForce);
        }


        private void HandleWalking()
        {
            //_rb.velocity = new Vector3(_commandContainer.WalkCommand * movementSpeed, _rb.velocity.y, 0);
            _renderer.flipX = !(_commandContainer.WalkCommand >= 0); // flips sprite based on move direction

            if (_commandContainer.WalkCommand != 0)
            {
                BaseMovementSpeed = _commandContainer.WalkCommand * Acceleration * Time.deltaTime;
                _rb.velocity = new Vector2(BaseMovementSpeed, _rb.velocity.y);
            }


        }
    }
}

