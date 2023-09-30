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
    public string StatusName
    {
        get { return statusName; }
    }
    public Sprite Sprite
    {
        get { return sprite; }
    }
    public int MinDuration
    {
        get { return minDuration; }
    }
    public int MaxDuration
    {
        get { return maxDuration; }
    }
    public Restriction Restriction
    {
        get { return restriction; }
    }
    public AutoRemainingTime Timing
    { 
        get { return timing; } 
    }
}