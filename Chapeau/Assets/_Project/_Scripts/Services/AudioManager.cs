
using UnityEngine;
using Seacore.Game;
using UnityEngine.Assertions;
using System.Collections;

namespace Seacore.Common.Services
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioClip _diePickupSound = null;

        AudioSource _audioSource = null;

        public void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(_audioSource, "No AudioSource found on AudioManager");
            StartCoroutine(WaitUntilSceneLoaded());
        }

        public void Start()
        {
            _audioSource.clip = _diePickupSound;
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
        }

        public void OnEnable()
        {
            InputManager.Instance.OnDieHoldEnter += OnDieSound;
            InputManager.Instance.OnDieHoldExit += OnDieSound;
        }


        public void OnDisable()
        {
            InputManager IM = InputManager.Instance;
            if (IM)
            {
                IM.OnDieHoldEnter -= OnDieSound;
                IM.OnDieHoldExit -= OnDieSound;
            }
        }

        IEnumerator WaitUntilSceneLoaded()
        {
            yield return new WaitUntil(() => InputManager.Instance != null);
            enabled = true;
        }

        void OnDieSound(Die _)
        {
            _audioSource.Play();
        }
    }
}
