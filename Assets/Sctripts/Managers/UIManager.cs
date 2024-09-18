using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("事件监听")]
    public CharacterEventSO healthEvent;

    [Header("组件")]
    public PlayerStateBar playerStateBar;

    // 注册事件
    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
    }

    // 注销事件
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
