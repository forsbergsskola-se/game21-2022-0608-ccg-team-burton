using GamePlay.Entities.Movement;
using GamePlay.Entities.Player;
using UnityEngine;

namespace Protoypes.Harry
{
    public class NewMovement : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private Vector3 _velocity;
        private CommandContainer _commandContainer;
        private GroundChecker _groundChecker;
        private Vector2 _lastPosition;
        private Vector3 RawMovement { get; set; }
        private Rigidbody2D _rb;
        private CapsuleCollider2D _collider;

        [Header("WALKING")] 
        public float _acceleration = 90;
        public float _moveClamp = 13; 
        public float _deAcceleration = 60f;
        public float _apexBonus = 2;
        private float _currentHorizontalSpeed;
        
        
        [Header("GRAVITY")] 
        public float _fallClamp = -40f;
        public float _minFallSpeed = 80f;
        public float _maxFallSpeed = 120f;
        public float _fallSpeed;
        
        [Header("JUMPING")] 
        public float _jumpHeight = 30;
        public float _jumpApexThreshold = 10f;
        public float _jumpEndEarlyGravityModifier = 3;
        private float _currentVerticalSpeed;

        private float _timeLeftGrounded;
        private bool _endedJumpEarly = true;
        private float _apexPoint; 

        

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
            _commandContainer = GetComponent<CommandContainer>();
            _groundChecker = GetComponent<GroundChecker>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        
        private void Update() 
        {
            _velocity = (_rb.position - _lastPosition) / Time.deltaTime;
            _lastPosition = _rb.position;
            
            CalculateWalking(); 
            CalculateJumpApex();
            CalculateGravity(); 
            CalculateJumping();
            MoveCharacter();
            FlipCharacter();
        }
        
    

        private void CalculateWalking() 
        {
            if (_commandContainer.WalkCommand != 0) 
            {
                // Set horizontal move speed
                _currentHorizontalSpeed += _commandContainer.WalkCommand * _acceleration * Time.deltaTime;

                // clamped by max frame movement
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

                // Apply bonus at the apex of a jump
                var apexBonus = Mathf.Sign(_commandContainer.WalkCommand) * _apexBonus * _apexPoint;
                _currentHorizontalSpeed += apexBonus * Time.deltaTime;
            }
            else 
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);

        }


        private void CalculateGravity()
        {
            if (_groundChecker.IsGrounded)
            {
                if (_currentVerticalSpeed < 0)
                    _currentVerticalSpeed = 0;
            }

            else
            {
                // Add downward force while ascending if we ended the jump early
                var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0
                    ? _fallSpeed * _jumpEndEarlyGravityModifier
                    : _fallSpeed;

                // Fall
                _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

                // Clamp
                if (_currentVerticalSpeed < _fallClamp)
                    _currentVerticalSpeed = _fallClamp;
            }
        }


        
        private void CalculateJumpApex() 
        {
            if (_groundChecker.IsGrounded)
            {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(_velocity.y));
                _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else
                _apexPoint = 0;
        }

        
        
        private void CalculateJumping() {
            if (_commandContainer.JumpDownCommand && _groundChecker.IsGrounded)
            {
                {
                    _currentVerticalSpeed = _jumpHeight;
                    _endedJumpEarly = false;
                    _timeLeftGrounded = float.MinValue;
                }
                
                // End the jump early if button released
                if (!_groundChecker.IsGrounded && _commandContainer.JumpUpCommand && !_endedJumpEarly && _velocity.y > 0)
                    _endedJumpEarly = true;
            }
            
            if (_groundChecker.IsRoofed)
                if (_currentVerticalSpeed > 0)
                    _currentVerticalSpeed = 0;
        }



        private void MoveCharacter()
        {
            RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
            var move = RawMovement * Time.deltaTime;
            transform.position = transform.position + Vector3.zero + move;
        }

        
        
        private void FlipCharacter()
        {
            _renderer.flipX = _commandContainer.WalkCommand switch
            {
                > 0 => false, 
                < 0 => true,
                _ => _renderer.flipX
            };
        }
    }
}