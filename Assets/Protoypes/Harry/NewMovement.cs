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
        private Vector3 _lastPosition;
        private Vector3 RawMovement { get; set; }

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
        private float _currentVerticalSpeed;

        private bool _endedJumpEarly = true;
        private float _apexPoint; 

        

        private void Awake()
        {
            _commandContainer = GetComponent<CommandContainer>();
            _groundChecker = GetComponent<GroundChecker>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        
        
        private void Update() 
        {
            _velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;
            
            CalculateWalking(); // horizontal movement
            CalculateJumpApex(); // vertical apex speed boost
            CalculateGravity(); // fall speed
            CalculateJumping(); // vertical movement
            MoveCharacter(); // after all calculations are done, move the character
            FlipCharacter(); // flip sprite based on where you're moving towards
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
            else // deAccelerate
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
        }

        

        private void CalculateGravity()
        {
            if (_groundChecker.IsGrounded)
            {
                // clip out of ground
                if (_currentVerticalSpeed < 0)
                    _currentVerticalSpeed = 0;
            }

            else
            {
                // Fall Speed calculated
                _currentVerticalSpeed -= _fallSpeed * Time.deltaTime;

                // Clamp it
                if (_currentVerticalSpeed < _fallClamp)
                    _currentVerticalSpeed = _fallClamp;
            }
        }


        
        private void CalculateJumpApex() 
        {
            if (_groundChecker.IsGrounded)
            {
                // Jump speed gets faster closer to apex
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(_velocity.y));
                _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else
                _apexPoint = 0;
        }

        
        
        private void CalculateJumping() 
        {
            if (_commandContainer.JumpDownCommand && _groundChecker.IsGrounded)
                _currentVerticalSpeed = _jumpHeight;

            // start falling if roof is hit
            if (_groundChecker.IsRoofed)
                if (_currentVerticalSpeed > 0)
                    _currentVerticalSpeed = 0;
        }



        private void MoveCharacter()
        {
            // actually move the character
            RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed);
            var move = RawMovement * Time.deltaTime;
            transform.position = transform.position + Vector3.zero + move;
        }

        
        
        private void FlipCharacter()
        {
            // use sprite renderer flipX bool to flip sprite
            _renderer.flipX = _commandContainer.WalkCommand switch
            {
                > 0 => false, // no flip if moving right
                < 0 => true, // flip is moving left
                _ => _renderer.flipX // keep last flip bool if no move input
            };
        }
    }
}