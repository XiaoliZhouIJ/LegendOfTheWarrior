using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName ="Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    // 事件订阅
    public UnityAction<Character> OnEventRaised;

    // 事件启用（/调用）
    public void RaiseEvent(Character character)
    { 
        OnEventRaised?.Invoke(character); 
    }
}
