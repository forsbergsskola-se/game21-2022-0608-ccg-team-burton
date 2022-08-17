using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleAnimScript : MonoBehaviour
{
    private Animator _animator;
    private AnimatorStateInfo _animatorStateInfo;
    public float _nTime;

    private void Awake() => _animator = GetComponent<Animator>();

    private void Update()
    {
        _animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _nTime = _animatorStateInfo.normalizedTime;
        
        if (!(_nTime > 1.0f)) return;
        SceneManager.LoadScene(1);
    }
}
