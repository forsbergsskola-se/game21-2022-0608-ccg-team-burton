using UnityEngine;

public class MenuAnimations : MonoBehaviour
{
    public GameObject StillMenuImage;
    public GameObject Animation;
    public bool DisableAnimation = true;
    
    private Animator _animator;
    private AnimatorStateInfo _animatorStateInfo;
    private float _nTime;
    [SerializeField] private bool _animationFinished;
    

    private void Awake() => _animator = Animation.GetComponent<Animator>();


    private void Update()
    {
        _animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _nTime = _animatorStateInfo.normalizedTime;

        if (_nTime <= 0) _animationFinished = false;
        else if (_nTime > 1.0f) _animationFinished = true;
        
        if (!DisableAnimation) return;

        if (_animationFinished)
        {
            StillMenuImage.SetActive(true);
            Animation.SetActive(false);
        }
        
    }
}
