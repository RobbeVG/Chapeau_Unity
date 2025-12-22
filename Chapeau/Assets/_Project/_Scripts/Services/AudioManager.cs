
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
        [SerializeField] AudioClip _dieRollingSound = null;

        private DiceController _diceController = null;

        AudioSource _audioSource = null;

        public void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            Assert.IsNotNull(_audioSource, "No AudioSource found on AudioManager");
        }

        public void Start()
        {

            _audioSource.playOnAwake = false;
            _audioSource.loop = false;

            FindObjectOfType<DiceController>().OnAllDiceRolled += OnDieRolled;
        }

        public void OnEnable()
        {
            InputManager.Instance.OnDieHoldEnter += OnDiePickupSound;
            InputManager.Instance.OnDieHoldExit += OnDiePickupSound;
        }


        public void OnDisable()
        {
            InputManager IM = InputManager.Instance;
            if (IM)
            {
                IM.OnDieHoldEnter -= OnDiePickupSound;
                IM.OnDieHoldExit -= OnDiePickupSound;
            }
        }

        void OnDieRolled()
        {
            _audioSource.clip = _dieRollingSound;
            _audioSource.Play();
        }

        void OnDiePickupSound(Die _)
        {
            _audioSource.clip = _diePickupSound;
            _audioSource.Play();
        }
    }
}
