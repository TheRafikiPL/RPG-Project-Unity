using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character 
{
    string nickname;
    int maxHP;
    int currentHP;
    int maxMP;
    int currentMP;
    int strength;
    int magic;
    int dexterity;
    int agility;
    int luck;
    Sprite sprite;
    List<Skill> skills;
    List<State> states;
    Dictionary<Element, ElementRelation> affinities;
    public Character(string nickname, int maxHP, int currentHP, int maxMP, int currentMP, int strength, int magic, int dexterity, int agility, int luck, Sprite sprite, List<Skill> skills, Dictionary<Element, ElementRelation> affinities)
    {
        this.nickname = nickname;
        this.maxHP = maxHP;
        this.currentHP = currentHP;
        this.maxMP = maxMP;
        this.currentMP = currentMP;
        this.strength = strength;
        this.magic = magic;
        this.dexterity = dexterity;
        this.agility = agility;
        this.luck = luck;
        this.sprite = sprite;
        this.skills = skills;
        states = new List<State>();
        this.affinities = affinities;
    }
    public string Nickname { get { return nickname; } }
    public int MaxHP { get { return maxHP; } }
    public int CurrentHP { get { return currentHP; } }
    public int MaxMP { get {  return maxMP; } }
    public int CurrentMP { get { return currentMP; } }
    public int Strength { get { return strength; } }
    public int Magic { get { return magic; } }
    public int Dexterity { get {  return dexterity; } }
    public int Agility { get {  return agility; } }
    public int Luck { get {  return luck; } }
    public Sprite Sprite { get { return sprite; } }
    public List<Skill> Skills { get { return skills; } }
    public List<State> States { get { return states; } }
    public Dictionary<Element, ElementRelation> Affinities { get { return affinities; } }

    public bool CheckStatesByName(string stateName)
    {
        foreach (State state in states)
        {
            if(stateName == state.StatusName)
            {
                return true;
            }
        }
        return false;
    }
    public void ChangeHP(int value, bool isPercent = false)
    {
        if(isPercent)
        {
            currentHP += maxHP * value / 100;
        }
        else
        {
            currentHP += value;
        }
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        if(currentHP < 0)
        {
            currentHP = 0;
            states.Add(BattleData.instance.statesList[0]);
        }
    }
    public void ChangeMP(int value, bool isPercent = false)
    {
        if (isPercent)
        {
            currentMP += maxMP * value / 100;
        }
        else
        {
            currentMP += value;
        }
        if (currentMP > maxMP)
        {
            currentMP = maxMP;
        }
        if (currentMP < 0)
        {
            currentMP = 0;
        }
    }
}