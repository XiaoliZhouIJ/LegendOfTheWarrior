using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("�¼�����")]
    public CharacterEventSO statehEvent;

    [Header("���")]
    public PlayerStateBar playerStateBar;

    // ע���¼�
    private void OnEnable()
    {
        statehEvent.OnEventRaised += OnStateEvent;
    }

    // ע���¼�
    private void OnDisable()
    {
        statehEvent.OnEventRaised -= OnStateEvent;
    }

    private void OnStateEvent(Character character)
    {
        float persentage =  character.currentHealth / character.maxHealth;

        playerStateBar.OnHealthChange(persentage);
        playerStateBar.OnPowerChange(character);
    }
}
