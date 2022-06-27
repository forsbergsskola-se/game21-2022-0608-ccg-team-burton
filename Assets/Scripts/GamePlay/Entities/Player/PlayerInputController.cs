using UnityEngine;
namespace GamePlay.Entities.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private CommandContainer _commandContainer;
        public float WalkInput;
        public bool WalkLeftDownInput;
        [HideInInspector] public bool WalkLeftUpInput;
        public bool WalkRightDownInput;
        [HideInInspector] public bool WalkRightUpInput;
        public bool NoWalkInput;
        
        public bool JumpDownInput;
        public bool JumpUpInput;

        public AnimationCurve WalkingCurve;
        public float CurrentWalkSpeed;
        public float InputAcceleration;
        public float InputDeceleration;
        
        
        public bool AttackDownInput;
        [HideInInspector] public bool AttackUpInput;

        
        
        private void Awake() => _commandContainer = GetComponent<CommandContainer>();

        
        
        private void Update()
        {
            HandleHorizontalMovement(); // collect player inputs
            SetCommands(); // assign inputs to commands

            if (WalkLeftDownInput || WalkRightDownInput)
                NoWalkInput = false;

            else if (WalkLeftUpInput && WalkRightUpInput)
                NoWalkInput = true;
        }
        
        

        private void HandleHorizontalMovement()
        {
            if (NoWalkInput && CurrentWalkSpeed != 0 || WalkRightDownInput && WalkLeftDownInput)
                CurrentWalkSpeed = Mathf.MoveTowards(CurrentWalkSpeed, 0, InputDeceleration * Time.deltaTime);


            if (WalkRightDownInput)
                CurrentWalkSpeed += Time.deltaTime * InputAcceleration;

            if (WalkLeftDownInput)
                CurrentWalkSpeed -= Time.deltaTime * InputAcceleration;



            //Mathf.Clamp(CurrentWalkSpeed, -1, 1);
            WalkInput = WalkingCurve.Evaluate(CurrentWalkSpeed);
        }


        
        private void SetCommands()
        {
            _commandContainer.WalkCommand = WalkInput;
            _commandContainer.WalkLeftCommand = WalkLeftDownInput;
            _commandContainer.WalkRightCommand = WalkRightDownInput;
            
            _commandContainer.JumpDownCommand = JumpDownInput;
            _commandContainer.JumpUpCommand = JumpUpInput;
            
            _commandContainer.AttackDownCommand = AttackDownInput;
            _commandContainer.AttackUpCommand = AttackUpInput;
        }
    }
}
