using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class JumpNode : ActionNode
    {
        private bool _startedJump;
        private float _delayQuit;
        
        public override void OnStart()
        {
            _startedJump = false;
            _delayQuit = 0;
        }

        public override void OnExit()
        {
         
        }

        public override State OnUpdate()
        {
            if (!_startedJump)
            {
                var est = agent.enemyEyes.estimatedJumpForce;
                var right = agent.enemyTransform.right;
                //agent.body.AddForce( new Vector2(right.x * 10f, 10f), ForceMode2D.Impulse);
                agent.body.AddForce( new Vector2(est.x * right.x, est.y), ForceMode2D.Impulse);
                agent.anim.SetTrigger(Animator.StringToHash("Enemy_Jump"));
                _startedJump = true;    
            }
            _delayQuit += Time.deltaTime;
            
            
            if (_delayQuit < 2f) return State.Update;
            if (_startedJump && agent.enemyEyes.compoundActions.HasFlag(CompoundActions.GroundSeen))
            {
                return State.Success;
            }
            
            return State.Update;
        }
    }
}