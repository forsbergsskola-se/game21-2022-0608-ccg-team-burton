using UnityEngine;
using UnityEngine.UI;

namespace Scene
{
    public class MainMenuAnimation : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] int frameRate = 60;
        [SerializeField] bool loop;
        int _currentFrame;
        float _elapsedTime;

        Image _image;
        Sprite[] _sprites;
        float _timePerFrame;

        void Start(){
            _image = GetComponent<Image>();
            enabled = false;
            LoadSpriteSheet();
        }

        void Update(){
            _elapsedTime += Time.deltaTime * speed;
            if (_elapsedTime >= _timePerFrame){
                _elapsedTime = 0f;
                _currentFrame++;
                SetSprite();
                if (_currentFrame >= _sprites.Length){
                    if (loop)
                        _currentFrame = 0;
                    else
                        enabled = false;
                }
            }
        }

        void LoadSpriteSheet(){
            _sprites = Resources.LoadAll<Sprite>("Assets/Resources/MainMenu");
            if (_sprites != null && _sprites.Length > 0){
                _timePerFrame = 1f / frameRate;
                Play();
            }
        }

        void Play(){
            enabled = true;
        }

        void SetSprite(){
            if (_currentFrame >= 0 && _currentFrame < _sprites.Length) _image.sprite = _sprites[_currentFrame];
        }
    }
}