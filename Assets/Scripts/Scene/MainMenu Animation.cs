using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace Scene
{
    public class MainMenuAnimation : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] int frameRate = 60;
        [SerializeField] bool Loop;

        Image _image = null;
        Sprite[] _sprites = null;
        float _timePerFrame = 0f;
        float _elapsedTime = 0f;
        int _currentFrame = 0;

        void Start(){
            _image = GetComponent<Image>();
            enabled = false;
            LoadSpriteSheet();
        }

        void LoadSpriteSheet()
        {
            _sprites = Resources.LoadAll<Sprite>("")
        }
    }
}
