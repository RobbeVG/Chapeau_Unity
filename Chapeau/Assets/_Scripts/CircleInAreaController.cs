using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore
{
    public class CircleInAreaController : MonoBehaviour
    {
        [SerializeField]
        private Material _circleFadeMaterial = null;

        [SerializeField]
        private float _fadeTransitionTime = 2.0f;

        private Vector2 _circleCenterScreen = Vector2.zero;
        private float _radiusOfScreenPercentage;
        private float _fadeTime = 0.0f;
        private float _direction = -1.0f;

        private void Awake()
        {
            Assert.IsNotNull(_circleFadeMaterial, "No Circle Fade Material was added");
        }

        // Start is called before the first frame update
        void Start()
        {
            //Camera cam = Camera.main;
            _circleCenterScreen = _circleFadeMaterial.GetVector("_ScreenPosition");
            _radiusOfScreenPercentage = _circleFadeMaterial.GetFloat("_RadiusOfScreenDiameter");

            //float length = new Vector2(Screen.width, Screen.height).magnitude;

        }

        public bool IsInsideCircle(Vector2 screenPos)
        {
            Vector2 screenRes = new Vector2(Screen.width, Screen.height);
            Vector2 screenClamped = new Vector2(Mathf.Clamp(screenPos.x, 0, 1), Mathf.Clamp(screenPos.y, 0, 1));
            float distanceSqr = ((screenClamped - _circleCenterScreen) * screenRes).sqrMagnitude; //[0, 1,414]
            return distanceSqr < (_radiusOfScreenPercentage * screenRes).sqrMagnitude; 
        }

        public void StartFadeTransition()
        {
            _direction *= -1.0f;
            if (_fadeTime == _fadeTransitionTime || _fadeTime == 0.0f)
                StartCoroutine(FadeTransition());
            else
                Debug.LogError("Do not start fade transition when still running previous transition");
        }

        private IEnumerator FadeTransition()
        {
            while (_fadeTime <= _fadeTransitionTime || _fadeTime >= 0.0f)
            {
                _circleFadeMaterial.SetFloat("_FadeTime", _fadeTime / _fadeTransitionTime);
                _fadeTime += (_direction * Time.deltaTime);
                yield return null;
            }
            if (_direction == -1.0f)
            {
                _fadeTime = 0.0f;
            }
            else if (_direction == 1.0f)
            {
                _fadeTime = _fadeTransitionTime;
            }
            _circleFadeMaterial.SetFloat("_FadeTime", _fadeTime / _fadeTransitionTime);
        }
    }
}
