using AYellowpaper.SerializedCollections;
using Reflex.Attributes;
using Seacore.Common;
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
//using UnityEngine.UIElements;


namespace Seacore.Game
{
    [DisallowMultipleComponent]
    public class SettingsMenu : MonoBehaviour
    {
        [Inject]
        AudioManager audioManager = null;

        [Serializable]
        class VolumeUIElements
        {
            public Slider slider;
            public TMP_Text textPercentage;
            public AudioMixerGroup mixerGroupTarget;

            public bool Validate()
            {
                return slider != null && textPercentage != null && mixerGroupTarget != null;
            }
        }

        [SerializeField]
        private List<VolumeUIElements> _AudioSettings = new List<VolumeUIElements>();

        [SerializeField]
        private Button _quitButton = null;

        private void Awake()
        {
            // Initialize UI with current settings
            foreach (VolumeUIElements volumeSettings in _AudioSettings)
            {
                if (!volumeSettings.Validate())
                {
                    Debug.LogError("VolumeSetting in settingsMenu is not setup correctly", this);
                }
                SetUI(volumeSettings);
            }

            if (_quitButton != null)
            {
                _quitButton.onClick.AddListener(() => { Reflex.Core.Container.RootContainer.Single<GameState>().Value = EGameState.MainMenu; gameObject.SetActive(false); });
            }
        }

        void Start()
        {
            foreach (VolumeUIElements volumeSettings in _AudioSettings)
            {
                volumeSettings.slider.onValueChanged.AddListener((value) => { audioManager.SoundSettings.SetVolume(volumeSettings.mixerGroupTarget, value); SetUIPercentage(volumeSettings.textPercentage, value); });
            }
        }

        private void SetUI(VolumeUIElements Settings)
        {
            float volume = audioManager.SoundSettings.GetVolume(Settings.mixerGroupTarget);
            SetUIPercentage(Settings.textPercentage, volume);
            Settings.slider.value = volume;
        }
        private void SetUIPercentage(TMP_Text textComponent, float value) => textComponent.text = Mathf.RoundToInt(value * 100).ToString() + " %";
    }
}
