using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioChangeVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer group;
    [SerializeField] private string floatParam = "MyExposedParam";
    [SerializeField] private Slider slider;

    public void ChangeValue()
    {
        group.SetFloat(floatParam, slider.value);
    }
}
