using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("�¼�����")]
    public AudioPlayEventSO BGMEvent;
    public AudioPlayEventSO FXEvent;

    [Header("���")]
    public AudioSource BGMSource;
    public AudioSource FXSource;

    private void OnEnable()
    {
        BGMEvent.OnAudioPlayEventRaised += OnBGMPlayEvent;
        FXEvent.OnAudioPlayEventRaised += OnFXPlayEvent;
    }

    private void OnDisable()
    {
        BGMEvent.OnAudioPlayEventRaised -= OnBGMPlayEvent;
        FXEvent.OnAudioPlayEventRaised -= OnFXPlayEvent;
    }

    private void OnBGMPlayEvent(AudioClip clip)
    {
        BGMSource.clip = clip;

        BGMSource.Play();
    }

    private void OnFXPlayEvent(AudioClip clip)
    {
        FXSource.clip = clip;

        FXSource.Play();
    }
}
