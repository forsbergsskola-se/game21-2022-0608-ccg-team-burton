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

        if (_playingAnim && !_reverseAnim)
            _animatorStateInfo = _forwardAnimator.GetCurrentAnimatorStateInfo(0);
        
        
        else if (_playingAnim && _reverseAnim)
            _animatorStateInfo = _reverseAnimator.GetCurrentAnimatorStateInfo(0);

        _nTime = _animatorStateInfo.normalizedTime; 

        
        switch (_nTime)
        {
            case > 1.0f when !_reverseAnim:
                StillMenuImage.SetActive(true);
                ForwardAnimation.SetActive(false);
                _playingAnim = false;
                break;
            case > 1.0f when _reverseAnim:
                StillMenuImage.SetActive(false);
                ReverseAnimation.SetActive(false);
                _playingAnim = false;
                break;
        }
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
}
