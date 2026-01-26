using Reflex.Attributes;

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Seacore.Common.Services
{
    public class SoundEmitter : MonoBehaviour
    {
        [Inject]
        AudioManager _audioManager;

        AudioSource audioSource = null;
        Coroutine playingRoutine;

        private void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void Play()
        {
            if (playingRoutine != null)
            {
                StopCoroutine(playingRoutine);
            }

            audioSource.Play();
            playingRoutine = StartCoroutine(WaitForSoundToEnd(audioSource));
        }

        public void Play(SoundData data)
        {
            Initialize(data);
            Play();
        }

        public void Stop()
        {
            if (playingRoutine != null)
            {
                StopCoroutine(playingRoutine);
                playingRoutine = null;
            }

            audioSource.Stop();
            _audioManager.Release(this);
        }

        IEnumerator WaitForSoundToEnd(AudioSource source)
        {
            yield return new WaitUntil(() => !source.isPlaying);
            _audioManager.Release(this);
        }

        public void Initialize(SoundData data)
        {
            audioSource.clip = data.audioClip;
            audioSource.outputAudioMixerGroup = data.mixerGroup;
            audioSource.volume = data.volume;
            audioSource.loop = data.loop;
        }

        public void SetRandomPitch(float min = -0.05f, float max = 0.05f)
        {
            audioSource.pitch += Random.Range(min, max);
        }
    }

    //public static class SoundEmitterExtensions
    //{
    //    public static SoundEmitter SetRandomPitch(this SoundEmitter emitter, float min = -0.05f, float max = 0.05f)
    //    {
    //        emitter.SetRandomPitch(min, max);
    //        return emitter;
    //    }
    //}
}
