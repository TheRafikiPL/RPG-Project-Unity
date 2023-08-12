using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour 
{
    [SerializeField]
    string nickname;
    [SerializeField]
    int maxHP;
    [SerializeField]
    int currentHP;
    [SerializeField]
    int maxMP;
    [SerializeField]
    int currentMP;
    [SerializeField]
    int Strength;
    [SerializeField]
    int Magic;
    [SerializeField]
    int Dexterity;
    [SerializeField]
    int Agility;
    [SerializeField]
    int Luck;
    [SerializeField]
    Sprite sprite;
    [SerializeField]
    List<Skill> skills;
    [SerializeField]
    List<State> states;
    [SerializeField]
    Dictionary<Element, ElementRelation> affinities;
}