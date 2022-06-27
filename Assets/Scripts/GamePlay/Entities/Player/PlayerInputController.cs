using UnityEngine;
namespace GamePlay.Entities.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private CommandContainer _commandContainer;
        public float WalkInput;
        public bool WalkLeftDownInput;
        public bool WalkRightDownInput;
        public bool NoWalkInput;
        
        public bool JumpDownInput;
        public bool JumpUpInput;

        //public AnimationCurve WalkingCurve;
        public float CurrentWalkSpeed;
        public float InputAcceleration;
        //public float InputDeceleration;
        
        
        public bool AttackDownInput;
        [HideInInspector] public bool AttackUpInput;

        
        
        private void Awake() => _commandContainer = GetComponent<CommandContainer>();

        
        
        private void Update()
        {
            GatherHorizontalMovement(); // collect player inputs
            SetHorizontalMovment(); // assign value between -1 & 1
            SetCommands(); // assign inputs to commands

            if (WalkLeftDownInput || WalkRightDownInput)
                NoWalkInput = false;

            else if (!WalkLeftDownInput && !WalkRightDownInput)
                NoWalkInput = true;
        }
        
        
        
        private void GatherHorizontalMovement()
        {
            if (NoWalkInput || WalkLeftDownInput && WalkRightDownInput)
                CurrentWalkSpeed = 0;
                //Mathf.MoveTowards(CurrentWalkSpeed, 0, InputDeceleration * Time.deltaTime);
            
            if (WalkRightDownInput)
                CurrentWalkSpeed += Time.deltaTime * InputAcceleration;

            if (WalkLeftDownInput)
                CurrentWalkSpeed -= Time.deltaTime * InputAcceleration;
        }

        
        
        
        private void SetHorizontalMovment()
        {
            CurrentWalkSpeed = Mathf.Clamp(CurrentWalkSpeed, -1, 1);
            WalkInput = CurrentWalkSpeed;
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
