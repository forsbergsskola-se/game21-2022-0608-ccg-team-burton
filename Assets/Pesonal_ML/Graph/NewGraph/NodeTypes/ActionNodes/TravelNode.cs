using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelNode : ActionNode
{
    private Vector3 _nextDestination;
    
    public override void OnStart()
    {
        agent.commandQueue.Dequeue();
        agent.currentDestination = agent.TargetQueue.Dequeue();
    }

    public override void OnExit()
    {
        
    }
    
    private bool CheckIfLookingAtTarget()
    {
        var dirFromAtoB = (agent.enemyTransform.position - agent.currentDestination).normalized;
        var dotProd = Vector3.Dot(dirFromAtoB, agent.enemyTransform.forward);
        return dotProd > 0.9f;
    }

    private bool ArrivedAtTarget()
    {
        return Vector3.Distance(agent.enemyTransform.position, agent.currentDestination) < 1.5f;
    }
    
    
    public override State OnUpdate()
    {
        if (!CheckIfLookingAtTarget())
        {
            var targetDirection = (agent.currentDestination - agent.enemyTransform.position).normalized;
            var singleStep = Time.deltaTime * 1;
            var newDirection = Vector3.RotateTowards(agent.enemyTransform.forward, targetDirection, singleStep, 0.0f);
            agent.enemyTransform.rotation = Quaternion.LookRotation(newDirection);
        }

        agent.enemyTransform.position += agent.enemyTransform.forward * (Time.deltaTime * 1);
        return ArrivedAtTarget() ? State.Success : State.Update;
    }
}
