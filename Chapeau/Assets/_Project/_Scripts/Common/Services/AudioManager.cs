using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace Seacore.Common.Services
{
    public class AudioManager : MonoBehaviour
    {
        AudioSource _musicAudioSource;
        private IObjectPool<AudioSource> _effectAudioSourcesPool;
        readonly List<AudioSource> _activeEffectAudioSources = new List<AudioSource>();
        
        [SerializeField]
        private int maxPoolEffectAudioSourcesSize = 10;

        private void Awake()
        {
            _musicAudioSource = gameObject.AddComponent<AudioSource>();
            _musicAudioSource.loop = true;

            _effectAudioSourcesPool = new ObjectPool<AudioSource>(
                createFunc: () => 
                {
                    GameObject emitter = new GameObject("EffectAudioSource"); 
                    emitter.transform.parent = gameObject.transform; 
                    return emitter.AddComponent<AudioSource>(); 
                },
                actionOnGet: (source) => 
                {
                    source.gameObject.SetActive(true); 
                    _activeEffectAudioSources.Add(source);
                },
                actionOnRelease: (source) =>
                {
                    source.Stop();
                    source.clip = null;
                    source.gameObject.SetActive(false);
                    _activeEffectAudioSources.Remove(source);
                },
                actionOnDestroy: (source) => Destroy(source.gameObject),
                collectionCheck: true,
                defaultCapacity: maxPoolEffectAudioSourcesSize,
                maxSize: maxPoolEffectAudioSourcesSize
            );
        }

        IEnumerator WaitForSoundToEnd(AudioSource source)
        {
            yield return new WaitUntil(() => !source.isPlaying);
            _effectAudioSourcesPool.Release(source);
        }

        public void PlayMusic(AudioClip clip)
        {
            _musicAudioSource.clip = clip;
            _musicAudioSource.Play();
        }

        public void PlayEffect(AudioClip clip)
        {
            AudioSource source = _effectAudioSourcesPool.Get();
            source.clip = clip;
            source.Play();
            StartCoroutine(WaitForSoundToEnd(_musicAudioSource));
        }


        //    public void Start()
        //    {

        //        _audioSource.playOnAwake = false;
        //        _audioSource.loop = false;

        //        FindObjectOfType<DiceController>().OnAllDiceRolled += OnDieRolled;
        //    }

        //    public void OnEnable()
        //    {
        //        InputManager.Instance.OnDieHoldEnter += OnDiePickupSound;
        //        InputManager.Instance.OnDieHoldExit += OnDiePickupSound;
        //    }


        //    public void OnDisable()
        //    {
        //        InputManager IM = InputManager.Instance;
        //        if (IM)
        //        {
        //            IM.OnDieHoldEnter -= OnDiePickupSound;
        //            IM.OnDieHoldExit -= OnDiePickupSound;
        //        }
        //    }

        //    void OnDieRolled()
        //    {
        //        _audioSource.clip = _dieRollingSound;
        //        _audioSource.Play();
        //    }

        //    void OnDiePickupSound(Die _)
        //    {
        //        _audioSource.clip = _diePickupSound;
        //        _audioSource.Play();
        //    }
        //}
    }
}
