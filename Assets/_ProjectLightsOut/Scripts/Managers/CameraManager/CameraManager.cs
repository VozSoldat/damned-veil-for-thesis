using ProjectLightsOut.DevUtils;
using UnityEngine;

namespace ProjectLightsOut.Managers
{
    public class CameraManager : Singleton<CameraManager>
    {
        private Camera mainCamera;
        private Vector3 originalPosition;
        private float shakeDuration = 0f;
        private float shakeMagnitude = 0.7f;
        private float dampingSpeed = 1.0f;

        protected override void Awake()
        {
            base.Awake();
            mainCamera = Camera.main;
            originalPosition = mainCamera.transform.localPosition;
        }

        private void OnEnable()
        {
            EventManager.AddListener<CameraShakeEvent>(OnCameraShake);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<CameraShakeEvent>(OnCameraShake);
        }

        private void OnCameraShake(CameraShakeEvent evt)
        {
            shakeDuration = evt.Duration;
            shakeMagnitude = evt.Magnitude;
        }

        private void Update()
        {
            ShakeCamera();
        }

        private void ShakeCamera()
        {
            if (shakeDuration > 0)
            {
                mainCamera.transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
                shakeDuration -= Time.deltaTime * dampingSpeed;
            }
            else
            {
                shakeDuration = 0f;
                mainCamera.transform.localPosition = originalPosition;
            }
        }
    }
}