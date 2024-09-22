using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/AudioPlayEventSO")]
public class AudioPlayEventSO : ScriptableObject
{
    public UnityAction<AudioClip> OnAudioPlayEventRaised;

    public void RaiseAudioPlayEvent(AudioClip audioClip)
    {
        OnAudioPlayEventRaised?.Invoke(audioClip);
    }
}

