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
        private Animator _animator;
        
        private Vector2 RawMovement { get; set; }
        private Vector2 _velocity;
        private Vector2 _lastPosition;
        private Rigidbody2D _rb;

        
        [Header("WALKING")] 
        public float _acceleration = 90;
        public float _moveClamp = 13; 
        public float _deAcceleration = 60f;
        public float _currentHorizontalSpeed { get; private set; }
        
        
        [Header("GRAVITY")] 
        public float _fallClamp = -40f;
        public float _minFallSpeed = 80f;
        public float _maxFallSpeed = 120f;
        private float FallSpeed;
        
        
        [Header("JUMPING")] 
        public float _jumpHeight = 30;
        private float _apexPoint;
        public float _currentVerticalSpeed { get; private set; }
        
        
        //Inputs
        private float _walkCommand;
        private bool _jumpDownCommand;
        
        //Collisions
        public bool _isGrounded{ get; private set; }
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
        }



        private void Update() { CollectInput(); CheckCollisions(); }
        
        
        
        private void FixedUpdate() 
        {
          CalculateWalking(); 
            CalculateJumpApex();
            CalculateGravity(); 
            FallIfWallOrRoofHit();
            CalculateJumping();

            FlipPlayer();
            if (_animator.runtimeAnimatorController != null)
                AnimatePlayer();
            
            MovePlayer();
        }


        
        private void CollectInput()
        {
            _walkCommand = _commandContainer.WalkCommand;
            _jumpDownCommand = _commandContainer.JumpDownCommand;
        }

        

        private void CheckCollisions()
        {
            _isGrounded = _groundChecker.IsGrounded;
            _isRoofed = _groundChecker.IsRoofed;
            _leftWallHit = _groundChecker.LeftWallHit;
            _rightWallHit = _groundChecker.RightWallHit;
        }
        


        private void CalculateWalking() 
        {
            if (_walkCommand != 0) 
            {
                // Set horizontal move speed
                _currentHorizontalSpeed += _walkCommand * _acceleration * Time.fixedDeltaTime;

                // clamped by max frame movement
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);
            }
            else 
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.fixedDeltaTime);
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
                // Fall
                _currentVerticalSpeed -= FallSpeed * Time.fixedDeltaTime;

                // Clamp
                if (_currentVerticalSpeed < _fallClamp)
                    _currentVerticalSpeed = _fallClamp;
            }
        }


        
        private void CalculateJumpApex() 
        {
            if (_isGrounded)
            {
                FallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else
                _apexPoint = 0;
        }

        
        
        private void CalculateJumping() 
        {
            if (!_isGrounded) return;
            
            if (_jumpDownCommand && _isGrounded)
                _currentVerticalSpeed = _jumpHeight;
        }

        
        
        private void FallIfWallOrRoofHit()
        {
            if (_currentVerticalSpeed > 0)
            {
                if (!_isRoofed) // if hit roof fall
                    return;

                if (!_isGrounded && (_leftWallHit || _rightWallHit))
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
            RawMovement = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.fixedDeltaTime;
           _rb.MovePosition(_rb.position + RawMovement);
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