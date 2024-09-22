using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class Sign : MonoBehaviour
{
    // 组件
    private Animator anim;
    public Transform playerTrans;
    private PlayerInputControl playerInput;

    public GameObject signSprite;

    private IInteractable targetItem;
    public bool canPress;

    private void Awake()
    {
        // anim = GetComponentInChildren<Animator>();           // 开始时关闭，无法获得
        anim = signSprite.GetComponent<Animator>();      

        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInput.Gameplay.Confirm.started += OnConfirm;
    }



    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale = playerTrans.localScale;

    }

    private void OnTriggerStay2D(Collider2D collision)
    { 
        Debug.Log(collision.tag);

        if (collision.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = collision.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
        targetItem = null;
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            targetItem.TriggeAction();
            canPress = false;

            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }

    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange == InputActionChange.ActionStarted)
        {
            // Debug.Log(((InputAction)obj).activeControl.device);
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    anim.Play("Keyboard");
                    break;
                case XInputController:
                    anim.Play("XBox");
                    break;
            }
        }
    }


}
