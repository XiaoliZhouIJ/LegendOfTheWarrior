using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName ="Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    // �¼�����
    public UnityAction<Character> OnEventRaised;

    // �¼����ã�/���ã�
    public void RaiseEvent(Character character)
    { 
        OnEventRaised?.Invoke(character); 
    }
}
