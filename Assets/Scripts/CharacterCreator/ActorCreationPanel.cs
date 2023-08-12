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
}
