using Reflex.Attributes;
using UnityEngine;

namespace Seacore.Common.Services
{
    public class MusicPlayer : MonoBehaviour
    {
        [Inject]
        AudioManager _audioManager = null;

        [SerializeField]
        SoundData data;

        public void Start()
        {
            _audioManager.CreateSound(data).Play();
        }
    }
}
