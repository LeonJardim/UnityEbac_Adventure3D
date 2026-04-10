using Leon.Singleton;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FXManager : Singleton<FXManager>
{
    [Header("References")]
    public CinemachineCamera cinemachineCamera;
    public Volume globalVolume;
    
    [Header("Vignette")]
    public float vFlashIntensity = 0f;
    public float vFlashDuration = 1f;
    private Vignette _vignette;

    [Header("Screen Shake")]
    public float shakeAmplitude = 1f;
    public float shakeFrequency = 1f;
    public float shakeDuration = 1f;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;


    private void Start()
    {
        globalVolume.profile.TryGet(out _vignette);
        _cinemachineBasicMultiChannelPerlin = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void FlashVignette()
    {
        StartCoroutine(FlashVignetteCoroutine());
    }
    IEnumerator FlashVignetteCoroutine()
    {
        float time = 0f;
        while (time < vFlashDuration)
        {
            _vignette.intensity.value = (time / vFlashDuration) * vFlashIntensity;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (time > 0f)
        {
            _vignette.intensity.value = (time / vFlashDuration) * vFlashIntensity;
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }


    public void ScreenShake()
    {
        StartCoroutine(ScreenShakeCoroutine());
    }
    IEnumerator ScreenShakeCoroutine()
    {
        _cinemachineBasicMultiChannelPerlin.AmplitudeGain = shakeAmplitude;
        _cinemachineBasicMultiChannelPerlin.FrequencyGain = shakeFrequency;

        float time = 0f;
        while (time < shakeDuration)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0f;
        _cinemachineBasicMultiChannelPerlin.FrequencyGain = 0f;
    }
}
