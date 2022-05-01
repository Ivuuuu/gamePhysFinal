using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {
	[SerializeField] private Slider MasterVolumeSlider;
	[SerializeField] private Slider MusicVolumeSlider;
	[SerializeField] private Slider EffectVolumeSlider;

    public void Start() {
		MasterVolumeSlider.value = SoundController.Singleton.MasterVolume.Value;
		MusicVolumeSlider.value = SoundController.Singleton.MusicVolume.Value;
        EffectVolumeSlider.value = SoundController.Singleton.EffectVolume.Value;
	}

	public void ChangeMasterVolume() {
		SoundController.Singleton.MasterVolume.Value = MasterVolumeSlider.value;
	}

	public void ChangeMusicVolume() {
		SoundController.Singleton.MusicVolume.Value = MusicVolumeSlider.value;
	}

	public void ChangeEffectVolume() {
		SoundController.Singleton.EffectVolume.Value = EffectVolumeSlider.value;
	}
}
