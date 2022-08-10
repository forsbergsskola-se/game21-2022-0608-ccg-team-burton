﻿using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class JumpNode : ActionNode
    {
        private bool _startedJump;
        
        public override void OnStart()
        {
            
        }

        public override void OnExit()
        {
            
        }

        public override State OnUpdate()
        {
            if (!_startedJump)
            {
                agent.body.AddForce(new Vector2(10f, 10f), ForceMode2D.Impulse);
                _startedJump = true;    
            }

            if (_startedJump && agent.enemyEyes.GroundSeen)
            {
                return State.Success;
            }
            
            return State.Update;
        }
    }
}