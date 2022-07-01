using GamePlay.Entities.Movement;
using GamePlay.Entities.Player;
using UnityEngine;

namespace Protoypes.Harry
{
    public class NewMovement : MonoBehaviour
    {
        private CommandContainer _commandContainer;
        private GroundChecker _groundChecker;
        private Animator _animator;
        
        private Vector2 _rawMovement { get; set; }
        private Vector2 _velocity;
        private Vector2 _lastPosition;
        private Rigidbody2D _rb;

        [Header("IDLE")]
        public bool FacingRight;

        
        [Header("WALKING")] 
        public float Acceleration = 90;
        public float MoveClamp = 13; 
        public float Deceleration = 60f;
        public float _currentHorizontalSpeed { get; private set; }


        [Header("GRAVITY")] 
        public float FallClamp = -40f;
        public float MinFallSpeed = 80f;
        public float MaxFallSpeed = 120f;
        private float FallSpeed;
        
        
        [Header("JUMPING")] 
        public float JumpHeight = 30;
        private float _apexPoint;
        public float _currentVerticalSpeed { get; private set; }
        
        
        [Header("JUMP BUFFER")]
        
        
        [Header("DOUBLE JUMPING")]
        public bool DoubleJumpAbilityActive;
        public bool CanDoubleJump;
        public float DoubleJumpHeight;
        
        [Header("DOUBLE JUMP BUFFER")]
        private int JumpCounter;
        public float AirborneTime;
        public float DoubleJumpBuffer;


            [Header("BOUNCING")]
        public float BounceHeight = 25;
        public float SuperBounceHeight = 40;

        //Inputs
        private float _walkCommand;
        private float _horizontal;
        private bool _jumpDownCommand;
        private bool _jumpSpace;

        //Collisions
        public bool _isGrounded { get; private set; }
        private bool _isBouncing;
        private bool _isSuperBouncing;
        private bool _isRoofed;
        private bool _leftWallHit;
        private bool _rightWallHit;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _commandContainer = GetComponent<CommandContainer>();
            _groundChecker = GetComponent<GroundChecker>();
            _animator = GetComponent<Animator>();
        }
        
        
        private void Update() 
        {
            CollectInput(); 
            CheckCollisions();
            // Animation & Sound
            if (_animator.runtimeAnimatorController != null)
                AnimatePlayer();
            FlipPlayer(_horizontal != 0 ? _horizontal : _walkCommand);
        }

        
        private void FixedUpdate() 
        {
            // Physics
            CalculateWalking(); 
            CalculateGravity();
            CheckForBouncing();
            CalculateJumping();
            //CalculateDoubleJump();
            CalculateJumpApex();
            FallIfWallOrRoofHit();
            
            
            // Execution
            MovePlayer();
        }
        
        private void CollectInput()
        {
            _walkCommand = _commandContainer.WalkCommand;
            _jumpDownCommand = _commandContainer.JumpDownCommand;
            _horizontal = _commandContainer.ArrowCommand;
            _jumpSpace = _commandContainer.SpaceDownCommand;
        }

        

        private void CheckCollisions()
        {
            _isGrounded = _groundChecker.IsGrounded;
            _isBouncing = _groundChecker.IsBouncing;
            _isSuperBouncing = _groundChecker.IsSuperBouncing;
            _isRoofed = _groundChecker.IsRoofed;
            _leftWallHit = _groundChecker.LeftWallHit;
            _rightWallHit = _groundChecker.RightWallHit;
        }
        


        private void CalculateWalking()
        {
            
            if (_walkCommand != 0 || _horizontal != 0) 
            {
                if (_horizontal is > 0 or < 0)
                    _currentHorizontalSpeed += _horizontal * Acceleration * Time.fixedDeltaTime;
                // Set horizontal move speed
                else
                    _currentHorizontalSpeed += _walkCommand * Acceleration * Time.fixedDeltaTime;

                // clamped by max frame movement
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -MoveClamp, MoveClamp);
            }
            else 
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, Deceleration * Time.fixedDeltaTime);
        }


        
        private void CalculateGravity()
        {
            if (_isGrounded)
            {
                CanDoubleJump = true;
                JumpCounter = 0;
                AirborneTime = 0f;

                if (_currentVerticalSpeed < 0)
                    _currentVerticalSpeed = 0;
            }

            else
            {
                AirborneTime += Time.deltaTime;
                _currentVerticalSpeed -= FallSpeed * Time.fixedDeltaTime; // Fall
                
                if (_currentVerticalSpeed < FallClamp) // Clamp
                    _currentVerticalSpeed = FallClamp;
            }
        }


        
        private void CalculateJumpApex() 
        {
            if (_isGrounded)
                FallSpeed = Mathf.Lerp(MinFallSpeed, MaxFallSpeed, _apexPoint);
            
            else
                _apexPoint = 0;
        }



        private void CheckForBouncing()
        {
            if (_isBouncing)
                _currentVerticalSpeed = BounceHeight;

            if (!_isSuperBouncing) return;
            _currentVerticalSpeed = SuperBounceHeight;
        }

        
        
        private void CalculateJumping()
        {
            if (_jumpDownCommand && _isGrounded || _jumpSpace && _isGrounded)
            {
                // calculate jump
                _currentVerticalSpeed = JumpHeight;
                JumpCounter++;
            }
            else
                CalculateDoubleJump();
        }



        private void CalculateDoubleJump()
        {
            // Check if double jump allowed
            if (!DoubleJumpAbilityActive) return;
            if (JumpCounter > 2)
                CanDoubleJump = false;


            // Buffer time wait before can double jump
            if (!(AirborneTime > DoubleJumpBuffer)) return;
            if ((!CanDoubleJump || !_jumpDownCommand) && (!CanDoubleJump || !_jumpSpace)) return;
                
            // calculate double jump
            _currentVerticalSpeed = DoubleJumpHeight;
            JumpCounter++;
        }
        

        
        private void FallIfWallOrRoofHit()
        {
            if (!(_currentVerticalSpeed > 0)) return;
            if (!_isRoofed) return; // if hit roof fall
            if (!_isGrounded && (_leftWallHit || _rightWallHit)) return; // if hit left or right wall fall
            
            _currentVerticalSpeed = 0; // shared fall logic
        }

        

        private void AnimatePlayer()
        {
            _animator.SetFloat("Hspeed", Mathf.Abs(_currentHorizontalSpeed)); 
            _animator.SetFloat("Vspeed", Mathf.Abs(_currentVerticalSpeed));
        }

        

        private void MovePlayer()
        {
            _rawMovement = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + _rawMovement);
        }



        private void FlipPlayer(float input)
        {
            switch (input)
            {
                case > 0 when !FacingRight:
                case < 0 when FacingRight:
                    
                    FacingRight = !FacingRight;
                    transform.Rotate(new Vector2(transform.rotation.x, 180));
                    break;
            }
        }
    }
}