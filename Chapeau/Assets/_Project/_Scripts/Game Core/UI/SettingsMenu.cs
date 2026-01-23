using AYellowpaper.SerializedCollections;
using Reflex.Attributes;
using Seacore.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Seacore.Game
{
    public class SettingsMenu : MonoBehaviour, ICancelHandler
    {
        [Inject]
        AudioManager audioManager = null;

        [Serializable]
        struct VolumeUIElements
        {
            public Slider slider;
            public TMP_Text textPercentage;
            public bool Validate()
            {
                return slider != null && textPercentage != null;
            }
        }


        [SerializeField]
        private VolumeUIElements _MasterSettings;
        [SerializeField]
        private SerializedDictionary<SoundType, VolumeUIElements> _AudioSettings = new SerializedDictionary<SoundType, VolumeUIElements>(Enum.GetValues(typeof(SoundType)).Cast<SoundType>().Select(e => new KeyValuePair<SoundType, VolumeUIElements>(e, new VolumeUIElements())).ToArray());

        private void Awake()
        {
            if (!_MasterSettings.Validate())
            {
                Debug.LogError("Master VolumeSetting in settingsMenu is not setup correctly", this);
            }




            SetPercentage(_MasterSettings.textPercentage, audioManager.SoundSettings.MasterVolume);
            _MasterSettings.slider.value = audioManager.SoundSettings.MasterVolume;

            foreach (KeyValuePair<SoundType, VolumeUIElements> pair in _AudioSettings)
            {
                VolumeUIElements settings = pair.Value;
                SoundType soundType = pair.Key;

                if (!settings.Validate())
                {
                    Debug.LogError("VolumeSetting in settingsMenu is not setup correctly", this);
                }


                SetPercentage(settings.textPercentage, audioManager.SoundSettings.GetVolume(soundType));
                settings.slider.value = audioManager.SoundSettings.GetVolume(soundType);
            }
        }

        void Start()
        {
            _MasterSettings.slider.onValueChanged.AddListener((value) => { audioManager.SoundSettings.MasterVolume = value; SetPercentage(_MasterSettings.textPercentage, value); });

            foreach (KeyValuePair<SoundType, VolumeUIElements> pair in _AudioSettings)
            {
                SoundType soundType = pair.Key;
                VolumeUIElements settings = pair.Value;
                settings.slider.onValueChanged.AddListener((value) => { audioManager.SoundSettings.SetVolume(soundType, value); SetPercentage(settings.textPercentage, value); });
            }
        }

        private void SetPercentage(TMP_Text textComponent, float value) => textComponent.text = Mathf.RoundToInt(value * 100).ToString() + " %";


        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void OnCancel(BaseEventData eventData)
        {
            gameObject.SetActive(false);
        }
    }
}
