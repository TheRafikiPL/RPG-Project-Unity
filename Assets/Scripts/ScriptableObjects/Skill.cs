using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "RPG-Project-Unity/Skill", order = 0)]
public class Skill : ScriptableObject 
{
    [SerializeField]
    string skillName;
    [SerializeField]
    string description;
    [SerializeField]
    Sprite affinityTexture;
    [SerializeField]
    int MPCost;
    [SerializeField]
    int HPPercentCost;
    [SerializeField]
    TargetChoice scope;
    [SerializeField]
    SkillType type;
    [SerializeField]
    int hitRatePercent;
    [SerializeField]
    Element affinity;
    [SerializeField]
    int power;
    [SerializeField]
    int variancePercent;
    [SerializeField]
    bool canCrit;
    [SerializeField]
    List<State> states;
    public string SkillName 
    { 
        get 
        { 
            return skillName; 
        } 
    }
    public string SkillCost 
    { 
        get 
        {
            string temp = "";
            if(MPCost>0)
            {
                temp += $"{MPCost} MP";
            }
            if(HPPercentCost>0)
            {
                temp += $"{HPPercentCost}% HP";
            }
            return temp; 
        } 
    }
    public Sprite AffinityTexture
    {
        get
        {
            return affinityTexture;
        }
    }
    public TargetChoice Scope
    {
        get
        {
            return scope;
        }
    }
    public SkillType SkillType 
    { 
        get 
        { 
            return type; 
        } 
    }
    public int HitRate
    {
        get 
        { 
            return hitRatePercent; 
        }
    }
    public Element Affinity
    {
        get
        {
            return affinity;
        }
    }
    public int CalculateDamage(Character attacker, Character defender)
    {
        int damage = 5;
        if(affinity == Element.PHYSICAL)
        {
            damage = (int)(damage * Mathf.Sqrt((float)attacker.Strength / defender.Dexterity * power));
        }
        else
        {
            damage = (int)(damage * Mathf.Sqrt((float)attacker.Magic / defender.Dexterity * power));
        }
        return damage;
    }
    public bool CanCrit
    {
        get
        {
            return canCrit;
        }
    }
    public List<State> States
    {
        get
        {
            return states;
        }
    }
    public int SkillCostNumberMP
    {
        get { return MPCost; }
    }
    public int SkillCostPercentHP
    {
        get { return HPPercentCost; }
    }
    public bool CharacterHaveHP(Character c)
    {
        if(c.CurrentHP < c.MaxHP * HPPercentCost / 100)
        {
            return false;
        }
        return true;
    }
    public bool CharacterHaveMP(Character c)
    {
        if (c.CurrentMP < MPCost)
        {
            return false;
        }
        return true;
    }
}