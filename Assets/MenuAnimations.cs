using UnityEngine;
using UnityEngine.UI;

public class MenuAnimations : MonoBehaviour
{
    public GameObject StillMenuImage;
    public GameObject ForwardAnimation;
    public GameObject ReverseAnimation;
    
    public Button HideButton;
    public Button TriggerButton;
    
    private Animator _forwardAnimator;
    private Animator _reverseAnimator;
    private AnimatorStateInfo _animatorStateInfo;
    
    private float _nTime;
    private bool _reverseAnim = false;
    private bool _playingAnim = false;
    public LevelCompleted LevelCompleted;


    private void Awake()
    {
        _forwardAnimator = ForwardAnimation.GetComponent<Animator>();
        _reverseAnimator = ReverseAnimation.GetComponent<Animator>();
        TriggerButton.onClick.AddListener(PlayForwardAnim);
        HideButton.onClick.AddListener(PlayReverseAnim);
    }

    
    
    private void Update()
    {
        if (!_playingAnim) return;

        if (_reverseAnim)
            ManageAnim(_reverseAnimator, ReverseAnimation);
        else
            ManageAnim(_forwardAnimator, ForwardAnimation);
    }
    
    
    
    private void ManageAnim(Animator animator, GameObject animGO)
    {
        _animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        _nTime = _animatorStateInfo.normalizedTime;

        if (!(_nTime > 1.0f)) return;
        
        StillMenuImage.SetActive(!_reverseAnim);
        animGO.SetActive(false);
        _playingAnim = false;
    }

    

    private void PlayForwardAnim()
    {
        _playingAnim = true;
        _reverseAnim = false;
        ForwardAnimation.SetActive(true);
    }

    
    
    private void PlayReverseAnim()
    {
        _playingAnim = true;
        _reverseAnim = true;
        ReverseAnimation.SetActive(true);
    }



    public void PauseTimer() => LevelCompleted.PauseTimer = true;
}
