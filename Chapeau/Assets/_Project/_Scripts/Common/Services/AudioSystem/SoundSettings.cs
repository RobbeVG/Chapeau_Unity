using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using AYellowpaper.SerializedCollections;

namespace Seacore.Common.Services
{
    [CreateAssetMenu(fileName = "SoundSettings", menuName = "ScriptableObjects/SoundSettings")]
    public class SoundSettings : ScriptableObject
    {
        [Serializable]
        private struct MixerVolumeVariable
        {
            public string name;
            public float value;

            public MixerVolumeVariable(AudioMixerGroup group)
            {
                if (group == null)
                    throw new ArgumentNullException(nameof(group));

                name = group.name + "Volume";
                group.audioMixer.GetFloat(name, out value);
            }
        }


        [SerializeField]
        private List<AudioMixerGroup> _mixerGroupsVolumes = new List<AudioMixerGroup>();
        private MixerVolumeVariable[] _mixerVariables = null;

        private void OnValidate()
        {
            if (_mixerGroupsVolumes == null || _mixerGroupsVolumes.Count == 0)
                return;

            if (_mixerVariables == null || _mixerVariables.Length != _mixerGroupsVolumes.Count)
            {
                _mixerVariables = new MixerVolumeVariable[_mixerGroupsVolumes.Count];
                for (int i = 0; i < _mixerGroupsVolumes.Count; i++)
                {
                    if (_mixerGroupsVolumes[i] == null)
                        Debug.LogWarning($"SoundSettings: Mixer Group at index {i} is null.");
                    else
                        _mixerVariables[i] = new MixerVolumeVariable(_mixerGroupsVolumes[i]);
                }
            }
        }

        public void ApplySettings()
        {
            if (_mixerVariables == null)
                return;

            for (int i = 0; i < _mixerVariables.Length; i++)
            {
                MixerVolumeVariable mixerVar = _mixerVariables[i];
                AudioMixerGroup mixerGroup = _mixerGroupsVolumes[i];
                if (mixerGroup == null)
                    Debug.LogError($"SoundSettings: Mixer Group at index {i} is null.");

                mixerGroup.audioMixer.SetFloat(mixerVar.name, mixerVar.value);
            }
        }

        public float GetdBVolume(AudioMixerGroup mixerGroup)
        {
            int index = _mixerGroupsVolumes.IndexOf(mixerGroup);
            return _mixerVariables[index].value;
        }
        public float GetVolume(AudioMixerGroup mixerGroup)
        {
            return Mathf.Pow(10f, GetdBVolume(mixerGroup) / 20f);
        }

        public void SetdBVolume(AudioMixerGroup mixerGroup, float dBvolume)
        {
            int index = _mixerGroupsVolumes.IndexOf(mixerGroup);
            mixerGroup.audioMixer.SetFloat(_mixerVariables[index].name, dBvolume);
            _mixerVariables[index].value = dBvolume;
        }

        public void SetVolume(AudioMixerGroup mixerGroup, float volume)
        {
            float dBvolume = Mathf.Log10(Mathf.Clamp(volume, 0.00001f, 1f)) * 20f;
            SetdBVolume(mixerGroup, dBvolume);
        }
    }
}
