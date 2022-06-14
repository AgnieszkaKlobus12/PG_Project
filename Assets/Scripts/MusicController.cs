using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public Slider slider;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            _audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            slider.value = PlayerPrefs.GetFloat("MusicVolume");
            PlayerPrefs.DeleteKey("MusicVolume");
        }
    }

    public void SliderValueChanged()
    {
        _audioSource.volume = slider.value;
        PlayerPrefs.SetFloat("MusicVolume", slider.value);
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("MusicVolume");
    }

}
