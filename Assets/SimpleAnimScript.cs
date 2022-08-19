using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleAnimScript : MonoBehaviour
{
    public GameObject Anim;
    public GameObject Menu;
    
    public Animator _animator;
    private AnimatorStateInfo _animatorStateInfo;
    private float _nTime;

    private void Awake() => _animator = GetComponent<Animator>();
    public bool Intro = false;

    private void Update()
    {
        _animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _nTime = _animatorStateInfo.normalizedTime;
        Menu.SetActive(false);

        
        if (!(_nTime > 1.0f)) return;

        if (!Intro)
        {
            Menu.SetActive(true);
            Anim.SetActive(false);
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
