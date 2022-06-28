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
        private SoundMananger _soundMananger;
        
        private Vector2 _rawMovement { get; set; }
        private Vector2 _velocity;
        private Vector2 _lastPosition;
        private Rigidbody2D _rb;

        [Header("IDLE")] 
        public FMODUnity.EventReference IdleSoundFile;
        private FMOD.Studio.EventInstance _idleSound;

        
        [Header("WALKING")] 
        public float Acceleration = 90;
        public float MoveClamp = 13; 
        public float Deceleration = 60f;
        public float _currentHorizontalSpeed { get; private set; }
        public bool FacingRight;
        public FMODUnity.EventReference WalkingSoundFile;
        private FMOD.Studio.EventInstance _walkingSound;
        
        
        [Header("GRAVITY")] 
        public float FallClamp = -40f;
        public float MinFallSpeed = 80f;
        public float MaxFallSpeed = 120f;
        private float FallSpeed;
        
        
        [Header("JUMPING")] 
        public float JumpHeight = 30;
        public float BounceHeight = 25;
        public float SuperBounceHeight = 40;
        private float _apexPoint;
        public float _currentVerticalSpeed { get; private set; }
        
        public FMODUnity.EventReference JumpSoundFile;
        private FMOD.Studio.EventInstance _jumpSound;
        public FMODUnity.EventReference ShroomBounceSoundFile;
        private FMOD.Studio.EventInstance _shroomBounce;
        
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
            _soundMananger = FindObjectOfType<SoundMananger>();
        }


        
        private void Start()
        {
            _idleSound = FMODUnity.RuntimeManager.CreateInstance(IdleSoundFile);
            _walkingSound = FMODUnity.RuntimeManager.CreateInstance(WalkingSoundFile);
            _jumpSound = FMODUnity.RuntimeManager.CreateInstance(JumpSoundFile);
            _shroomBounce = FMODUnity.RuntimeManager.CreateInstance(ShroomBounceSoundFile);
        }

        
        
        private void Update() { CollectInput(); CheckCollisions(); }

        
        private void FixedUpdate() 
        {
            MovementSound();
            
            CalculateWalking(); 
            CalculateJumpApex();
            CalculateGravity(); 
            FallIfWallOrRoofHit();
            CalculateJumping();
            FlipPlayer(_horizontal != 0 ? _horizontal : _walkCommand);

            if (_animator.runtimeAnimatorController != null)
                AnimatePlayer();
            
            MovePlayer();
        }
        
        
        private void MovementSound()
        {
            if (_currentHorizontalSpeed == 0 && _currentVerticalSpeed == 0)
            {
                _soundMananger.PlaySound(_idleSound);
                    _soundMananger.StopSound(_walkingSound);

            }
            else
            {
                _soundMananger.StopSound(_idleSound);
                if(_groundChecker.IsGrounded)
                    _soundMananger.PlaySound(_walkingSound);
            }
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
                if (_currentVerticalSpeed < 0)
                    _currentVerticalSpeed = 0;
            }

            else
            {
                // Fall
                _currentVerticalSpeed -= FallSpeed * Time.fixedDeltaTime;

                // Clamp
                if (_currentVerticalSpeed < FallClamp)
                    _currentVerticalSpeed = FallClamp;
            }
        }


        
        private void CalculateJumpApex() 
        {
            if (_isGrounded)
            {
                FallSpeed = Mathf.Lerp(MinFallSpeed, MaxFallSpeed, _apexPoint);
            }
            else
                _apexPoint = 0;
        }

        
        
        private void CalculateJumping() 
        {
            if (_isBouncing)
            {
                _currentVerticalSpeed = BounceHeight;
                _soundMananger.PlaySound(_shroomBounce);

            }

            if (_isSuperBouncing)
            {
                _currentVerticalSpeed = SuperBounceHeight;
                _soundMananger.PlaySound(_shroomBounce);
            }
            
            if (!_isGrounded) return;

            if (_jumpDownCommand && _isGrounded || _jumpSpace && _isGrounded)
            {
                _currentVerticalSpeed = JumpHeight;
                _soundMananger.PlaySound(_jumpSound);
            }
            
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
           _animator.SetBool("Attacking",false);
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