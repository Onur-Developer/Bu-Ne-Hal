using System.Collections;
using UnityEngine;
using Cinemachine;

namespace Scipts.CameraManager
{
    public class CameraManager : MonoBehaviour
    {
        #region Shake Variables
        private CinemachineVirtualCamera _camera;
        private CinemachineBasicMultiChannelPerlin _cbmp;
        private bool _isShake;
        #endregion

        #region Screen Axis Variables

       [HideInInspector] public bool isCollide;

        #endregion
        

        private void Start()
        {
            _camera = GameManager.instance.cinemachine;
            _cbmp = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }


        public void SetScreenAxis(float xAxis,float yAxis)
        {
            if(_camera.GetCinemachineComponent<CinemachineFramingTransposer>()==null) return;
            float xAx = isCollide ? _camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX : xAxis;
            _camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = xAx;
            _camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = yAxis;
        }

        public void SetCollide(bool collide) => isCollide = collide;

        public void SetCameraShake(float duration,float frequency,float amplitude)
        {
            if (_isShake) return;
            _cbmp.m_FrequencyGain = frequency;
            _cbmp.m_AmplitudeGain = amplitude;
            StartCoroutine(ResetShakeTimer(duration));
        }

        void ResetCameraShake()
        {
            _cbmp.m_FrequencyGain = 0;
            _cbmp.m_AmplitudeGain = 0;
        }

        public void ResetScreenAxis()
        {
            if(_camera.GetCinemachineComponent<CinemachineFramingTransposer>()==null) return;
            _camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.25f;
            _camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.5f;
        }


        IEnumerator ResetShakeTimer(float duration)
        {
            _isShake = true;
            yield return new WaitForSeconds(duration);
            ResetCameraShake();
            _isShake = false;
        }
    }
}
