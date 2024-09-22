using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("��������")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;

    [Header("�����޵�")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool inculnerable;

    [Header("�����¼�")]
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent<Transform> OnDie;

    [Header("�¼�")]
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Water"))
        {
            // ����
            currentHealth = 0;
            OnStateChange?.Invoke(this);
            OnDie?.Invoke(transform); 
        }
    }

    public void TakeDamage(Attack attacker)
    {
        if (inculnerable) return;

        currentHealth -= attacker.damage;
        if (currentHealth > 0)
        {
            TriggerInvuanerable();
            // ����
            OnTakeDamage?.Invoke(attacker.transform);

        }
        else
        {
            currentHealth = 0;
            // ��������
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
