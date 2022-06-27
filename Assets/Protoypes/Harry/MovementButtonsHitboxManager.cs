using UnityEngine;
using UnityEngine.UI;

public class MovementButtonsHitboxManager : MonoBehaviour
{
    private GameObject LeftHitbox;
    private GameObject RightHitbox;
    private GameObject AttackHitbox;
    private GameObject JumpHitbox;

    
    [Range(0.0f, 1.0f)]
    public float OpacityScale;
    [Range (1.0f, 5.0f)]
    public float FixedSize;

    private Image _leftImage;
    private Image _rightImage;
    private Image _attackImage;
    private Image _jumpImage;
    private Color _newHitboxAlpha;

    private void Awake()
    {
        LeftHitbox = gameObject.transform.Find("Left Button/Left Hitbox").gameObject;
        RightHitbox = transform.Find("Right Button/Right Hitbox").gameObject;
        AttackHitbox = transform.Find("Attack Button/Attack Hitbox").gameObject;
        JumpHitbox = transform.Find("Jump Button/Jump Hitbox").gameObject;
        
        _leftImage = LeftHitbox.GetComponent<Image>();
        _rightImage = RightHitbox.GetComponent<Image>();
        _attackImage = AttackHitbox.GetComponent<Image>();
        _jumpImage = JumpHitbox.GetComponent<Image>();
    }

    
    
    private void Update()
    {
        SetAlpha();
        ChangeAlphaValues();
        ChangeHitboxSizes();
    }


    
    private void SetAlpha()
    {
        _newHitboxAlpha = _leftImage.color;
        _newHitboxAlpha.a = OpacityScale;
    }


    
    private void ChangeAlphaValues()
    {
        _leftImage.color = _newHitboxAlpha;
        _rightImage.color  = _newHitboxAlpha;
        _attackImage.color  = _newHitboxAlpha;
        _jumpImage.color  = _newHitboxAlpha;
    }


    
    private void ChangeHitboxSizes()
    {
        var newSize = new Vector3(FixedSize, FixedSize, 0);
        LeftHitbox.transform.localScale = newSize;
        RightHitbox.transform.localScale = newSize;
        AttackHitbox.transform.localScale = newSize;
        JumpHitbox.transform.localScale = newSize;
    }
}
