using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Seacore.Common.Services
{
    [Serializable]
    public partial class SoundData
    {

        public AudioClip audioClip;
        public AudioMixerGroup mixerGroup = null;
        public bool loop = false;
        public float volume = 1.0f;
    }
}
