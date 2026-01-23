using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Seacore.Common.Services
{
    public class SoundSettings
    {
        private Dictionary<int, float> _soundVolumes;
        public float MasterVolume { get { return _soundVolumes[0]; } set { _soundVolumes[0] = value; } }

        public SoundSettings(float masterVolume = 1.0f)
        {
            _soundVolumes = new Dictionary<int, float>(Enum.GetValues(typeof(SoundType)).Cast<SoundType>().Select(e => new KeyValuePair<int, float>((int)e, 1.0f)).ToArray())
            {
                { 0, 1.0f } // Reserved for MasterVolume
            };
        }

        public float GetVolume(SoundType type)
        {
            return MasterVolume * _soundVolumes[(int)type];
        }
        public void SetVolume(SoundType type, float volume)
        {
            _soundVolumes[(int)type] = volume;
        }
    }
}
