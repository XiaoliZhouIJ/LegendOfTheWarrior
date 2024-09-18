using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("��������")]
    public float maxHealth;
    public float currentHealth;

    [Header("�����޵�")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool inculnerable;

    [Header("�����¼�")]
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent<Transform> OnDie;

    [Header("�¼�")]
    public UnityEvent<Character> OnHealthChange;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
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
    }


    public void TakeDamage(Attack attacker)
    {
        if (inculnerable) return;

        currentHealth -= attacker.damage;
        if(currentHealth > 0)
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

        OnHealthChange?.Invoke(this);
    }

    private void TriggerInvuanerable()
    {
        if (!inculnerable)
        {
            inculnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

}
