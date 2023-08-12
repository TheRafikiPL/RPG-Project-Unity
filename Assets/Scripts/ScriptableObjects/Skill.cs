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
    Texture2D affinityTexture;
    [SerializeField]
    int MPCost;
    [SerializeField]
    int HPPercentCost;
    [SerializeField]
    TargetChoice Scope;
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
    public void ActivateSkill()
    {

    }
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
    public Texture2D AffinityTexture
    {
        get
        {
            return affinityTexture;
        }
    }
}