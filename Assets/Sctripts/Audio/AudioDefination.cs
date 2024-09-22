using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    public AudioPlayEventSO audioPlayEvent;

    public AudioClip audioClip;

    public bool playOnEnable;

    private void OnEnable()
    {
        if (playOnEnable)
            PlayAudioClip();
    }

    public void PlayAudioClip()
    {
        audioPlayEvent?.OnAudioPlayEventRaised(audioClip);
    }
}
