using GamePlay.Entities.Movement;
using GamePlay.Entities.Player;
using UnityEngine;

namespace Protoypes.Harry
{
    public class NewMovement : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private CommandContainer _commandContainer;
        private GroundChecker _groundChecker;
        private TrailRenderer _trailRenderer;
        private Animator _animator;
        
        private Vector2 RawMovement { get; set; }
        private Vector2 _velocity;
        private Vector2 _lastPosition;
        private Rigidbody2D _rb;

        
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
        private float FallSpeed;
        
        
        [Header("JUMPING")] 
        public float _jumpHeight = 30;
        public float _jumpApexThreshold = 10f;
        public float _jumpEndEarlyGravityModifier = 3;
        
        private float _currentVerticalSpeed;
        private float _timeLeftGrounded;
        private bool _endedJumpEarly = true;
        private float _apexPoint;
        
        
        //Inputs
        private float _walkCommand;
        private bool _jumpDownCommand;
        private bool _jumpUpCommand;
        
        //Collisions
        private bool _isGrounded;
        private bool _isRoofed;
        private bool _leftWallHit;
        private bool _rightWallHit;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _commandContainer = GetComponent<CommandContainer>();
            _groundChecker = GetComponent<GroundChecker>();
            _renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _trailRenderer = GetComponent<TrailRenderer>();
        }



        private void Update() { CollectInput(); CheckCollisions(); }



        private void CollectInput()
        {
            _walkCommand = _commandContainer.WalkCommand;
            _jumpDownCommand = _commandContainer.JumpDownCommand;
            _jumpUpCommand = _commandContainer.JumpUpCommand;
        }

        

        private void CheckCollisions()
        {
            _isGrounded = _groundChecker.IsGrounded;
            _isRoofed = _groundChecker.IsRoofed;
            _leftWallHit = _groundChecker.LeftWallHit;
            _rightWallHit = _groundChecker.RightWallHit;
        }


        
        private void FixedUpdate() 
        {
            _velocity = (_rb.position - _lastPosition) / Time.deltaTime;
            _lastPosition = _rb.position;
            
            CalculateWalking(); 
            CalculateJumpApex();
            CalculateGravity(); 
            CalculateJumping();
            FallIfWallOrRoofHit();
            
            FlipPlayer();
            AnimatePlayer();
            MovePlayer();
        }
        
    

        private void CalculateWalking() 
        {
            if (_walkCommand != 0) 
            {
                // Set horizontal move speed
                _currentHorizontalSpeed += _walkCommand * _acceleration * Time.deltaTime;

                // clamped by max frame movement
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

                // Apply bonus at the apex of a jump
                var apexBonus = Mathf.Sign(_walkCommand) * _apexBonus * _apexPoint;
                _currentHorizontalSpeed += apexBonus * Time.deltaTime;
            }
            else 
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
        }


        
        private void CalculateGravity()
        {
            if (_isGrounded)
            {
                if (_currentVerticalSpeed < 0)
                    _currentVerticalSpeed = 0;
            }

            else
            {
                // Add downward force while ascending if we ended the jump early
                var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0
                    ? FallSpeed * _jumpEndEarlyGravityModifier
                    : FallSpeed;

                // Fall
                _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

                // Clamp
                if (_currentVerticalSpeed < _fallClamp)
                    _currentVerticalSpeed = _fallClamp;
            }
        }


        
        private void CalculateJumpApex() 
        {
            if (_isGrounded)
            {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(_velocity.y));
                FallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else
                _apexPoint = 0;
        }

        
        
        private void CalculateJumping() 
        {
            if (_jumpDownCommand && _isGrounded)
            {
                {
                    _currentVerticalSpeed = _jumpHeight;
                    _endedJumpEarly = false;
                    _timeLeftGrounded = float.MinValue;
                }
                
                // End the jump early if button released
                if (!_isGrounded && _jumpUpCommand && !_endedJumpEarly && _velocity.y > 0)
                    _endedJumpEarly = true;
            }
        }

        
        
        private void FallIfWallOrRoofHit()
        {
            if (_currentVerticalSpeed > 0)
            {
                if (!_isRoofed) // if hit roof fall
                    return;

                if (_isGrounded && (_leftWallHit || _rightWallHit))
                    return; // if hit left or right wall fall
 
                _currentVerticalSpeed = 0; // shared fall logic
            }
        }


        private void AnimatePlayer()
        {
            
                _animator.SetFloat("Hspeed", Mathf.Abs(_currentHorizontalSpeed));
            
            
                _animator.SetFloat("Vspeed", Mathf.Abs(_currentVerticalSpeed));
            
        }

        

        private void MovePlayer()
        {
            RawMovement = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.deltaTime; // Used externally
            _rb.MovePosition(_rb.position + Vector2.zero + RawMovement);
        }



        private void FlipPlayer()
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