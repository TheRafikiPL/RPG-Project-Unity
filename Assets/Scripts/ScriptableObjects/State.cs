using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State", menuName = "RPG-Project-Unity/State", order = 0)]
public class State : ScriptableObject 
{
    [SerializeField]
    string statusName;
    [SerializeField]
    Sprite sprite;
    [SerializeField]
    int minDuration;
    [SerializeField]
    int maxDuration;
    [SerializeField]
    Restriction restriction;
    [SerializeField]
    AutoRemainingTime timing;
}