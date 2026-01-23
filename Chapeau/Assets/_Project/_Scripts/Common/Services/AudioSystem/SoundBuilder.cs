using UnityEngine;

namespace Seacore.Common.Services
{
    public class SoundBuilder
    {
        readonly AudioManager _audioManager;

        SoundData soundData;
        Vector3 position;
        bool randomPitch;

        public SoundBuilder(AudioManager audioManager, SoundData data)
        {
            _audioManager = audioManager;
            soundData = data;
        }

        public SoundBuilder WithRandomPitch()
        {
            randomPitch = true;
            return this;
        }

        public SoundBuilder AtPosition(Vector3 pos)
        {
            position = pos;
            return this;
        }


        public void Play()
        {
            SoundEmitter emitter = _audioManager.Get();
            emitter.Initialize(soundData);
            emitter.transform.position = position;

            if (randomPitch)
            {
                emitter.SetRandomPitch();
            }

            emitter.Play();
        }
    }
}
