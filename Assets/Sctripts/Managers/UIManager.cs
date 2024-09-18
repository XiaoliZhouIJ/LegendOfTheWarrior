using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("事件监听")]
    public CharacterEventSO statehEvent;

    [Header("组件")]
    public PlayerStateBar playerStateBar;

    // 注册事件
    private void OnEnable()
    {
        statehEvent.OnEventRaised += OnStateEvent;
    }

    // 注销事件
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
