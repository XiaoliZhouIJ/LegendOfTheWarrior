using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;

    [Header("受伤无敌")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool inculnerable;

    [Header("动画事件")]
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent<Transform> OnDie;

    [Header("事件")]
    public UnityEvent<Character> OnStateChange;

    private void Start()
    {
        currentHealth = maxHealth;
        currentPower = maxPower;
        OnStateChange?.Invoke(this);
    }

    private void Update()
    {
        if (inculnerable)
        {
            invulnerableCounter -= Time.deltaTime;

            if (invulnerableCounter <= 0)
            {
                inculnerable = false;
            }
        }

        if (currentPower < maxPower)
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
    }


    public void TakeDamage(Attack attacker)
    {
        if (inculnerable) return;

        currentHealth -= attacker.damage;
        if (currentHealth > 0)
        {
            TriggerInvuanerable();
            // 受伤
            OnTakeDamage?.Invoke(attacker.transform);

        }
        else
        {
            currentHealth = 0;
            // 人物死亡
            OnDie?.Invoke(attacker.transform);
        }

        OnStateChange?.Invoke(this);
    }

    private void TriggerInvuanerable()
    {
        if (!inculnerable)
        {
            inculnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

    public bool OnSlide(int cost)
    {
        if (PowerCost(cost))
        {
            OnStateChange?.Invoke(this);
            return true;
        }

        else
        {
            return false;
        }
        
    }

    private bool PowerCost(int cost)
    {
        if (currentPower < cost)
            return false;

        currentPower -= cost;

        return true;
    }
}
