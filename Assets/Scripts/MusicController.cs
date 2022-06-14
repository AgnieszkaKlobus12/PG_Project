using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public Slider slider;

    public void SliderValueChanged()
    {
        GetComponent<AudioSource>().volume = slider.value;
    }
}
