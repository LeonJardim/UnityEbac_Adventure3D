using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXPool : MonoBehaviour
{
    private List<AudioSource> _audioSourceList;
    private int _index = 0;

    public int poolSize = 8;
    public AudioMixerGroup mixerGroup;

    private void Awake()
    {
        CreatePool();
    }
    

    public void Play(SFXSetup sfxSetup)
    {
        _audioSourceList[_index].clip = sfxSetup.audioClip;
        _audioSourceList[_index].Play();

        _index++;
        if (_index >= _audioSourceList.Count) _index = 0;
    }


    private void CreatePool()
    {
        _audioSourceList = new List<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateAudioSourceItem();
        }
    }
    private void CreateAudioSourceItem()
    {
        GameObject go = new GameObject("SFX_Pool");
        go.transform.SetParent(transform);
        _audioSourceList.Add(go.AddComponent<AudioSource>());

        var source = go.GetComponent<AudioSource>();
        source.outputAudioMixerGroup = mixerGroup;
        source.volume = 0.5f;
    }
}
