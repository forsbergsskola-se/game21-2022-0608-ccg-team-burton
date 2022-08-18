using UnityEngine;

public class SimpleAnimScript : MonoBehaviour
{
    public GameObject Anim;
    public GameObject Menu;
    
    private Animator _animator;
    private AnimatorStateInfo _animatorStateInfo;
    private float _nTime;

    private void Awake() => _animator = GetComponent<Animator>();

    private void Update()
    {
        _animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _nTime = _animatorStateInfo.normalizedTime;
        
        if (!(_nTime > 1.0f)) return;
        
        Menu.SetActive(true);
        Anim.SetActive(false);
    }
}
