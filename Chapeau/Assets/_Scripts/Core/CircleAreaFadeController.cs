using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore
{
    [RequireComponent(typeof(DragAndDropController))]
    public class CircleAreaFadeController : MonoBehaviour
    {
        [SerializeField]
        private DiceManager diceManager;

        [SerializeField]
        private float speed;

        [SerializeField]
        private Material circleFadeMaterial = null;

        [SerializeField]
        private float fadeTransitionTime = 2.0f;

        private Vector2 _circleCenterScreen = Vector2.zero;
        private float _radiusOfScreenPercentage;
        private float _fadeTime = 0.0f;
        private float _direction = -1.0f;

        private void Awake()
        {
            Assert.IsNotNull(circleFadeMaterial, "No Circle Fade Material was added");
            Assert.IsNotNull(diceManager, "No Dice Manager was added");
        }

        private void OnEnable()
        {
            GetComponent<DragAndDropController>().OnGameObjectDropped += OnGameObjectDropped;
            GetComponent<DragAndDropController>().OnGameObjectSelected += OnGameObjectSelected;
        }

        private void OnDisable()
        {
            GetComponent<DragAndDropController>().OnGameObjectDropped -= OnGameObjectDropped;
            GetComponent<DragAndDropController>().OnGameObjectSelected -= OnGameObjectSelected;
        }

        // Start is called before the first frame update
        void Start()
        {
            _circleCenterScreen = circleFadeMaterial.GetVector("_ScreenPosition");
            _radiusOfScreenPercentage = circleFadeMaterial.GetFloat("_RadiusOfScreenDiameter");
        }

        private void OnGameObjectSelected(GameObject gameObject)
        {

        }

        private void OnGameObjectDropped(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out Die dieComponent))
            {
                SetDiceLocation(dieComponent);
            }
        }

        private void SetDiceLocation(Die die)
        {
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(gameObject.transform.position); //Value [0,1]
            float distanceSqrToCirlce = DistanceSqrToCircle(positionOnScreen);
            if (distanceSqrToCirlce <= 0.0f) // inside circle and inside padding range of circle
                diceManager.DiceContainers[die].Location = RollLocation.Inside; 
            else // outside circle and inside padding range of circle
                diceManager.DiceContainers[die].Location = RollLocation.Outside; 
        }

        public bool IsInsideCircle(Vector2 screenPos)
        {
            return DistanceSqrToCircle(screenPos) < 0.0f; 
        }

        private float DistanceSqrToCircle(Vector2 screenPos)
        {
            Vector2 screenRes = new Vector2(Screen.width, Screen.height);
            float distanceSqr = ((_circleCenterScreen - screenPos) * screenRes).sqrMagnitude; //[0, 1,414]
            return (_radiusOfScreenPercentage * screenRes).sqrMagnitude - distanceSqr;
        }

        public void StartFadeTransition()
        {
            _direction *= -1.0f;
            if (_fadeTime == fadeTransitionTime || _fadeTime == 0.0f)
                StartCoroutine(FadeTransition());
            else
                Debug.LogError("Do not start fade transition when still running previous transition");
        }

        private IEnumerator FadeTransition()
        {
            while (_fadeTime <= fadeTransitionTime || _fadeTime >= 0.0f)
            {
                circleFadeMaterial.SetFloat("_FadeTime", _fadeTime / fadeTransitionTime);
                _fadeTime += (_direction * Time.deltaTime);
                yield return null;
            }
            if (_direction == -1.0f)
            {
                _fadeTime = 0.0f;
            }
            else if (_direction == 1.0f)
            {
                _fadeTime = fadeTransitionTime;
            }
            circleFadeMaterial.SetFloat("_FadeTime", _fadeTime / fadeTransitionTime);
        }
    }
}
