using Cinemachine;
using UnityEngine;

namespace Design.Jocke.Jocke_testSceneScripts
{
    public class CameraShake : MonoBehaviour
    {
        private CinemachineVirtualCamera _cineMachine;
        private CinemachineBasicMultiChannelPerlin _noise;

        private float shakeTimer;

        private void Awake()
        {
            _cineMachine = gameObject.GetComponent<CinemachineVirtualCamera>();
            _noise = _cineMachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void ShakeCamera(float intensity, float time)
        {
            _noise.m_AmplitudeGain = intensity;
            shakeTimer = time;
        }

        private void Update()
        {
            if (shakeTimer > 0f)
                shakeTimer -= Time.deltaTime;
        
            if (shakeTimer <= 0f)
                _noise.m_AmplitudeGain = 0f; // time Over!
        }
    }
}
