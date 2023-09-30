using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActorCreationPanel : MonoBehaviour
{
    [SerializeField]
    Image actorSprite;
    [SerializeField]
    TMP_InputField nameInput;
    [SerializeField]
    TMP_InputField healthInput;
    [SerializeField]
    TMP_InputField manaInput;
    [SerializeField]
    TMP_InputField strengthInput;
    [SerializeField]
    TMP_InputField magicInput;
    [SerializeField]
    TMP_InputField dexterityInput;
    [SerializeField]
    TMP_InputField agilityInput;
    [SerializeField]
    TMP_InputField luckInput;
    [SerializeField]
    TMP_Dropdown physicalAffinity;
    [SerializeField]
    TMP_Dropdown fireAffinity;
    [SerializeField]
    TMP_Dropdown iceAffinity;
    [SerializeField]
    TMP_Dropdown windAffinity;
    [SerializeField]
    TMP_Dropdown electricityAffinity;
    [SerializeField]
    TMP_Dropdown lightAffinity;
    [SerializeField]
    TMP_Dropdown darkAffinity;
    [SerializeField]
    RectTransform SkillListContent;
    public void CheckNumericValue(TMP_InputField obj, int value)
    {
        if(obj.text == "")
        {
            return;
        }
        int number = Convert.ToInt32(obj.text);
        if (number < 1)
        {
            obj.text = "1";
            return;
        }
        if (number > value)
        {
            obj.text = value.ToString();
        }
    }
    public void CheckHealth()
    {
        CheckNumericValue(healthInput, 9999);
    }
    public void CheckMana()
    {
        CheckNumericValue(manaInput, 9999);
    }
    public void CheckStrength()
    {
        CheckNumericValue(strengthInput, 99);
    }
    public void CheckMagic()
    {
        CheckNumericValue(magicInput, 99);
    }
    public void CheckDexterity()
    {
        CheckNumericValue(dexterityInput, 99);
    }
    public void CheckAgility()
    {
        CheckNumericValue(agilityInput, 99);
    }
    public void CheckLuck()
    {
        CheckNumericValue(luckInput, 99);
    }
    public bool CheckActorForConvertion()
    {
        if(String.IsNullOrWhiteSpace(nameInput.text))
        {
            return false;
        }
        if (String.IsNullOrWhiteSpace(healthInput.text))
        {
            return false;
        }
        if (String.IsNullOrWhiteSpace(manaInput.text))
        {
            return false;
        }
        if (String.IsNullOrWhiteSpace(strengthInput.text))
        {
            return false;
        }
        if (String.IsNullOrWhiteSpace(magicInput.text))
        {
            return false;
        }
        if (String.IsNullOrWhiteSpace(dexterityInput.text))
        {
            return false;
        }
        if (String.IsNullOrWhiteSpace(agilityInput.text))
        {
            return false;
        }
        if (String.IsNullOrWhiteSpace(luckInput.text))
        {
            return false;
        }
        return true;
    }
    List<Skill> CreateSkillList()
    {
        List<Skill> list = new List<Skill>();
        list.Add(BattleData.instance.skillList[0]);
        list.Add(BattleData.instance.skillList[1]);
        for (int i = 0; i < SkillListContent.childCount; i++)
        {
            if(SkillListContent.GetChild(i).GetComponentInChildren<Toggle>().isOn)
            {
                list.Add(BattleData.instance.skillList[i + 2]);
            }
        }
        return list;
    }
    Dictionary<Element, ElementRelation> CreateAffinities()
    {
        Dictionary<Element, ElementRelation> dict = new Dictionary<Element,ElementRelation>();
        dict.Add(Element.NONE, ElementRelation.NORMAL);
        dict.Add(Element.PHYSICAL, IntToElementRelation(physicalAffinity.value));
        dict.Add(Element.FIRE, IntToElementRelation(fireAffinity.value));
        dict.Add(Element.ICE, IntToElementRelation(iceAffinity.value));
        dict.Add(Element.WIND, IntToElementRelation(windAffinity.value));
        dict.Add(Element.ELECTRICITY, IntToElementRelation(electricityAffinity.value));
        dict.Add(Element.LIGHT, IntToElementRelation(lightAffinity.value));
        dict.Add(Element.DARK, IntToElementRelation(darkAffinity.value));
        dict.Add(Element.ALMIGHTY, ElementRelation.NORMAL);
        return dict;
    }
    ElementRelation IntToElementRelation(int value)
    {
        switch(value)
        {
            case 1:
                return ElementRelation.WEAK;
            case 2:
                return ElementRelation.STRONG;
            case 3:
                return ElementRelation.NULL;
            case 4:
                return ElementRelation.ABSORB;
            case 5:
                return ElementRelation.REPEL;
            default:
                return ElementRelation.NORMAL;
        }
    }
    public Character ConvertActorToCharacter()
    {
        string nickname = nameInput.text;
        int health = Convert.ToInt32(healthInput.text);
        int mana = Convert.ToInt32(manaInput.text);
        int strength = Convert.ToInt32(strengthInput.text);
        int magic = Convert.ToInt32(magicInput.text);
        int dexterity = Convert.ToInt32(dexterityInput.text);
        int agility = Convert.ToInt32(agilityInput.text);
        int luck = Convert.ToInt32(luckInput.text);
        Sprite sprite = actorSprite.sprite;
        List<Skill> skills = CreateSkillList();
        Dictionary<Element, ElementRelation> affinities = CreateAffinities();
        return new Character(nickname, health, health, mana, mana, 
            strength, magic, dexterity, agility, luck, sprite, skills, affinities);
    }
}
