using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Reflex.Core;
using UnityEngine.Audio;


namespace Seacore.Common.Services
{
    public class AudioManager : MonoBehaviour
    {
        private IObjectPool<SoundEmitter> _soundEmittersPool;
        readonly List<SoundEmitter> _activeEmitters = new List<SoundEmitter>();

        public SoundSettings SoundSettings { get; private set; } = null;

        [SerializeField]
        private int maxPoolSoundEmitters = 10;
        [SerializeField]
        private int defaultCapacityPool = 10;

        [SerializeField]
        private AudioMixer masterMixer = null;

        public SoundEmitter Get() => _soundEmittersPool.Get();
        public void Release(SoundEmitter emitter) => _soundEmittersPool.Release(emitter);
        public SoundBuilder CreateSound(SoundData data) => new SoundBuilder(this, data);

        private void Awake()
        {
            SoundSettings = new SoundSettings(masterMixer);

            _soundEmittersPool = new ObjectPool<SoundEmitter>(
                createFunc: () => 
                {
                    GameObject emitter = new GameObject("EffectAudioSource"); 
                    emitter.transform.parent = gameObject.transform;
                    SoundEmitter soundEmitter = emitter.AddComponent<SoundEmitter>();
                    Reflex.Injectors.AttributeInjector.Inject(soundEmitter, Container.RootContainer);
                    return soundEmitter;
                },
                actionOnGet: (emitter) => 
                {
                    emitter.gameObject.SetActive(true); 
                    _activeEmitters.Add(emitter);
                },
                actionOnRelease: (emitter) =>
                {
                    emitter.gameObject.SetActive(false);
                    _activeEmitters.Remove(emitter);
                },
                actionOnDestroy: (emitter) => Destroy(emitter.gameObject),
                collectionCheck: true,
                defaultCapacity: defaultCapacityPool,
                maxSize: maxPoolSoundEmitters
            );
        }
    }
}
