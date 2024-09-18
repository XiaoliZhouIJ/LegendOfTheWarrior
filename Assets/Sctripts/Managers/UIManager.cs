using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("�¼�����")]
    public CharacterEventSO healthEvent;

    [Header("���")]
    public PlayerStateBar playerStateBar;

    // ע���¼�
    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
    }

    // ע���¼�
    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
    }

    private void OnHealthEvent(Character character)
    {
        float persentage =  character.currentHealth / character.maxHealth;

        playerStateBar.OnHealthChange(persentage);
    }
}
