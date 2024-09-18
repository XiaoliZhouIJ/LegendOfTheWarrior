using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    private Character currentCharacter;

    public Image healthBarImage;
    public Image healthDelayBarImage;
    public Image powerBarImage;

    private bool isRecovering;

    public float delaySpeed;

    private void Update()
    {
        if (healthDelayBarImage.fillAmount > healthBarImage.fillAmount)
        {
            healthDelayBarImage.fillAmount -= Time.deltaTime;
        }

        if (isRecovering)
        {
            float persentage = currentCharacter.currentPower / currentCharacter.maxPower;
            powerBarImage.fillAmount = persentage;

            if (persentage >= 1)
            {
                isRecovering = false;
            }
        }
    }

    /// <summary>
    /// 接受Health变更百分比
    /// </summary>
    /// <param name="persentage">血量百分比</param>
    public void OnHealthChange(float persentage)
    {
        healthBarImage.fillAmount = persentage;

        StartCoroutine(DelayBarChange());
    }

    private IEnumerator DelayBarChange()
    {
        float delta = healthDelayBarImage.fillAmount - healthBarImage.fillAmount;

        while (healthDelayBarImage.fillAmount > healthBarImage.fillAmount )
        {
            healthDelayBarImage.fillAmount -= delta /delaySpeed;

            yield return null;
        }

        
    }

    public void OnPowerChange(Character character)
    {
        isRecovering = true;

        currentCharacter = character;
    }
}
