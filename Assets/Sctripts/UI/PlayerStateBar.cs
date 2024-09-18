using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    public Image healthBarImage;
    public Image healthDelayBarImage;
    public Image powerBarImage;

    public float delaySpeed;

    private void Update()
    {
        //if(healthDelayBarImage.fillAmount > healthBarImage.fillAmount)
        //{
        //    healthDelayBarImage.fillAmount -= Time.deltaTime;
        //}
    }

    /// <summary>
    /// ����Health����ٷֱ�
    /// </summary>
    /// <param name="persentage">Ѫ���ٷֱ�</param>
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
}
