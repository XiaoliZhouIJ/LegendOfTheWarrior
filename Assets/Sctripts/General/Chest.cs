using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;

    public Sprite openSprite;
    public Sprite closeSprite;

    public bool isOpened;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (isOpened)
        {
            gameObject.tag = "Untagged";
            spriteRenderer.sprite = openSprite;
        }
        else
        {
            spriteRenderer.sprite = closeSprite;
        }
    }

    public void TriggeAction()
    {
        if (!isOpened)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        spriteRenderer.sprite = openSprite;
        isOpened = true;
        gameObject.tag = "Untagged";
    }
}
