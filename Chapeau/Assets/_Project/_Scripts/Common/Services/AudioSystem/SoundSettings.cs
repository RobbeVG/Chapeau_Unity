using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Seacore.Common.Services
{
    public class SoundSettings
    {
        private Dictionary<AudioMixerGroup, string> _soundVolumeVariables;
        
        public SoundSettings(AudioMixer mixer)
        {
            if (mixer == null)
            {
                throw new ArgumentNullException(nameof(mixer), "AudioMixer cannot be null when initializing SoundSettings.");
            }

            _soundVolumeVariables = new Dictionary<AudioMixerGroup, string>(mixer.FindMatchingGroups(string.Empty).Select(group => new KeyValuePair<AudioMixerGroup, string>(group, group.name + "Volume")));
        }

        public float GetdBVolume(AudioMixerGroup mixerGroup)
        {
            mixerGroup.audioMixer.GetFloat(_soundVolumeVariables[mixerGroup], out float volume);
            return volume;
        }
        public float GetVolume(AudioMixerGroup mixerGroup)
        {
            return Mathf.Pow(10f, GetdBVolume(mixerGroup) / 20f);
        }

        public void SetdBVolume(AudioMixerGroup mixerGroup, float dBvolume)
        {
            mixerGroup.audioMixer.SetFloat(_soundVolumeVariables[mixerGroup], dBvolume);
        }

        public void SetVolume(AudioMixerGroup mixerGroup, float volume)
        {
            float dBvolume = Mathf.Log10(Mathf.Clamp(volume, 0.00001f, 1f)) * 20f;
            SetdBVolume(mixerGroup, dBvolume);
        }
    }
}
