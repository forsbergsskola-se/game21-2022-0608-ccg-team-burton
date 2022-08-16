using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class PlaySoundBehaviour : StateMachineBehaviour
{
    [SerializeField] SoundMananger _soundMananger;
    public EventReference SoundFile;
    EventInstance _sound;

    bool initiated;

    public void OnDisable(){
        _soundMananger.StopSound(_sound);
        Debug.Log("Hej hopp jättesnopp");
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        if (!initiated){
            _sound = RuntimeManager.CreateInstance(SoundFile);
            initiated = true;
        }

        _soundMananger.PlaySound(_sound);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        _soundMananger.StopSound(_sound);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}