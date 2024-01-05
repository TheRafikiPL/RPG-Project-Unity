using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnvData: MonoBehaviour
{
    public TrainingManager TrainingManager;
    [Header("Base")]
    public int PressedTurns;
    public int Turns;
    public int ChosenSkill;
    public int ChosenActor;
    [Header("Actor1")]
    public int a1MaxHP;
    public int a1CurrentHP;
    public int a1MaxMP;
    public int a1CurrentMP;
    public int a1Strength;
    public int a1Magic;
    public int a1Dexterity;
    public int a1Agility;
    public int a1Luck;
    public int a1Physical;
    public int a1Fire;
    public int a1Ice;
    public int a1Electricity;
    public int a1Wind;
    public int a1Light;
    public int a1Dark;
    [Header("Actor2")]
    public int a2MaxHP;
    public int a2CurrentHP;
    public int a2MaxMP;
    public int a2CurrentMP;
    public int a2Strength;
    public int a2Magic;
    public int a2Dexterity;
    public int a2Agility;
    public int a2Luck;
    public int a2Physical;
    public int a2Fire;
    public int a2Ice;
    public int a2Electricity;
    public int a2Wind;
    public int a2Light;
    public int a2Dark;
    [Header("Actor3")]
    public int a3MaxHP;
    public int a3CurrentHP;
    public int a3MaxMP;
    public int a3CurrentMP;
    public int a3Strength;
    public int a3Magic;
    public int a3Dexterity;
    public int a3Agility;
    public int a3Luck;
    public int a3Physical;
    public int a3Fire;
    public int a3Ice;
    public int a3Electricity;
    public int a3Wind;
    public int a3Light;
    public int a3Dark;
    [Header("Actor4")]
    public int a4MaxHP;
    public int a4CurrentHP;
    public int a4MaxMP;
    public int a4CurrentMP;
    public int a4Strength;
    public int a4Magic;
    public int a4Dexterity;
    public int a4Agility;
    public int a4Luck;
    public int a4Physical;
    public int a4Fire;
    public int a4Ice;
    public int a4Electricity;
    public int a4Wind;
    public int a4Light;
    public int a4Dark;
    [Header("Enemy")]
    public int eMaxHP;
    public int eCurrentHP;
    public int eMaxMP;
    public int eCurrentMP;
    public int eStrength;
    public int eMagic;
    public int eDexterity;
    public int eAgility;
    public int eLuck;
    public int ePhysical;
    public int eFire;
    public int eIce;
    public int eElectricity;
    public int eWind;
    public int eLight;
    public int eDark;

    private void Update()
    {
        GetInfoFromManager();
    }
    void GetInfoFromManager()
    {
        PressedTurns = 0;
        Turns = 0;
        foreach(bool turn in TrainingManager.turnQueue)
        {
            Turns++;
            if(!turn)
            {
                PressedTurns++;
            }
        }
        ChosenActor = TrainingManager.trainer.chosenActor;
        ChosenSkill = TrainingManager.trainer.chosenSkill;
        if (TrainingManager.party.Count>0)
        {
            a1MaxHP = TrainingManager.party[0].MaxHP;
            a1CurrentHP = TrainingManager.party[0].CurrentHP;
            a1MaxMP = TrainingManager.party[0].MaxMP;
            a1CurrentMP = TrainingManager.party[0].CurrentMP;
            a1Strength = TrainingManager.party[0].Strength;
            a1Magic = TrainingManager.party[0].Magic;
            a1Dexterity = TrainingManager.party[0].Dexterity;
            a1Agility = TrainingManager.party[0].Agility;
            a1Luck = TrainingManager.party[0].Luck;
            a1Physical = (int)TrainingManager.party[0].Affinities[Element.PHYSICAL];
            a1Fire = (int)TrainingManager.party[0].Affinities[Element.FIRE];
            a1Ice = (int)TrainingManager.party[0].Affinities[Element.ICE];
            a1Electricity = (int)TrainingManager.party[0].Affinities[Element.ELECTRICITY];
            a1Wind = (int)TrainingManager.party[0].Affinities[Element.WIND];
            a1Light = (int)TrainingManager.party[0].Affinities[Element.LIGHT];
            a1Dark = (int)TrainingManager.party[0].Affinities[Element.DARK];
        }
        else
        {
            a1MaxHP = 0;
            a1CurrentHP = 0;
            a1MaxMP = 0;
            a1CurrentMP = 0;
            a1Strength = 0;
            a1Magic = 0;
            a1Dexterity = 0;
            a1Agility = 0;
            a1Luck = 0;
            a1Physical = 3;
            a1Fire = 3;
            a1Ice = 3;
            a1Electricity = 3;
            a1Wind = 3;
            a1Light = 3;
            a1Dark = 3;
        }
        if (TrainingManager.party.Count > 1)
        {
            a2MaxHP = TrainingManager.party[1].MaxHP;
            a2CurrentHP = TrainingManager.party[1].CurrentHP;
            a2MaxMP = TrainingManager.party[1].MaxMP;
            a2CurrentMP = TrainingManager.party[1].CurrentMP;
            a2Strength = TrainingManager.party[1].Strength;
            a2Magic = TrainingManager.party[1].Magic;
            a2Dexterity = TrainingManager.party[1].Dexterity;
            a2Agility = TrainingManager.party[1].Agility;
            a2Luck = TrainingManager.party[1].Luck;
            a2Physical = (int)TrainingManager.party[1].Affinities[Element.PHYSICAL];
            a2Fire = (int)TrainingManager.party[1].Affinities[Element.FIRE];
            a2Ice = (int)TrainingManager.party[1].Affinities[Element.ICE];
            a2Electricity = (int)TrainingManager.party[1].Affinities[Element.ELECTRICITY];
            a2Wind = (int)TrainingManager.party[1].Affinities[Element.WIND];
            a2Light = (int)TrainingManager.party[1].Affinities[Element.LIGHT];
            a2Dark = (int)TrainingManager.party[1].Affinities[Element.DARK];
        }
        else
        {
            a2MaxHP = 0;
            a2CurrentHP = 0;
            a2MaxMP = 0;
            a2CurrentMP = 0;
            a2Strength = 0;
            a2Magic = 0;
            a2Dexterity = 0;
            a2Agility = 0;
            a2Luck = 0;
            a2Physical = 3;
            a2Fire = 3;
            a2Ice = 3;
            a2Electricity = 3;
            a2Wind = 3;
            a2Light = 3;
            a2Dark = 3;
        }
        if (TrainingManager.party.Count > 2)
        {
            a3MaxHP = TrainingManager.party[2].MaxHP;
            a3CurrentHP = TrainingManager.party[2].CurrentHP;
            a3MaxMP = TrainingManager.party[2].MaxMP;
            a3CurrentMP = TrainingManager.party[2].CurrentMP;
            a3Strength = TrainingManager.party[2].Strength;
            a3Magic = TrainingManager.party[2].Magic;
            a3Dexterity = TrainingManager.party[2].Dexterity;
            a3Agility = TrainingManager.party[2].Agility;
            a3Luck = TrainingManager.party[2].Luck;
            a3Physical = (int)TrainingManager.party[2].Affinities[Element.PHYSICAL];
            a3Fire = (int)TrainingManager.party[2].Affinities[Element.FIRE];
            a3Ice = (int)TrainingManager.party[2].Affinities[Element.ICE];
            a3Electricity = (int)TrainingManager.party[2].Affinities[Element.ELECTRICITY];
            a3Wind = (int)TrainingManager.party[2].Affinities[Element.WIND];
            a3Light = (int)TrainingManager.party[2].Affinities[Element.LIGHT];
            a3Dark = (int)TrainingManager.party[2].Affinities[Element.DARK];
        }
        else
        {
            a3MaxHP = 0;
            a3CurrentHP = 0;
            a3MaxMP = 0;
            a3CurrentMP = 0;
            a3Strength = 0;
            a3Magic = 0;
            a3Dexterity = 0;
            a3Agility = 0;
            a3Luck = 0;
            a3Physical = 3;
            a3Fire = 3;
            a3Ice = 3;
            a3Electricity = 3;
            a3Wind = 3;
            a3Light = 3;
            a3Dark = 3;
        }
        if (TrainingManager.party.Count > 3)
        {
            a4MaxHP = TrainingManager.party[3].MaxHP;
            a4CurrentHP = TrainingManager.party[3].CurrentHP;
            a4MaxMP = TrainingManager.party[3].MaxMP;
            a4CurrentMP = TrainingManager.party[3].CurrentMP;
            a4Strength = TrainingManager.party[3].Strength;
            a4Magic = TrainingManager.party[3].Magic;
            a4Dexterity = TrainingManager.party[3].Dexterity;
            a4Agility = TrainingManager.party[3].Agility;
            a4Luck = TrainingManager.party[3].Luck;
            a4Physical = (int)TrainingManager.party[3].Affinities[Element.PHYSICAL];
            a4Fire = (int)TrainingManager.party[3].Affinities[Element.FIRE];
            a4Ice = (int)TrainingManager.party[3].Affinities[Element.ICE];
            a4Electricity = (int)TrainingManager.party[3].Affinities[Element.ELECTRICITY];
            a4Wind = (int)TrainingManager.party[3].Affinities[Element.WIND];
            a4Light = (int)TrainingManager.party[3].Affinities[Element.LIGHT];
            a4Dark = (int)TrainingManager.party[3].Affinities[Element.DARK];
        }
        else
        {
            a4MaxHP = 0;
            a4CurrentHP = 0;
            a4MaxMP = 0;
            a4CurrentMP = 0;
            a4Strength = 0;
            a4Magic = 0;
            a4Dexterity = 0;
            a4Agility = 0;
            a4Luck = 0;
            a4Physical = 3;
            a4Fire = 3;
            a4Ice = 3;
            a4Electricity = 3;
            a4Wind = 3;
            a4Light = 3;
            a4Dark = 3;
        }
        if (TrainingManager.enemies.Count > 0)
        {
            eMaxHP = TrainingManager.enemies[0].MaxHP;
            eCurrentHP = TrainingManager.enemies[0].CurrentHP;
            eMaxMP = TrainingManager.enemies[0].MaxMP;
            eCurrentMP = TrainingManager.enemies[0].CurrentMP;
            eStrength = TrainingManager.enemies[0].Strength;
            eMagic = TrainingManager.enemies[0].Magic;
            eDexterity = TrainingManager.enemies[0].Dexterity;
            eAgility = TrainingManager.enemies[0].Agility;
            eLuck = TrainingManager.enemies[0].Luck;
            ePhysical = (int)TrainingManager.enemies[0].Affinities[Element.PHYSICAL];
            eFire = (int)TrainingManager.enemies[0].Affinities[Element.FIRE];
            eIce = (int)TrainingManager.enemies[0].Affinities[Element.ICE];
            eElectricity = (int)TrainingManager.enemies[0].Affinities[Element.ELECTRICITY];
            eWind = (int)TrainingManager.enemies[0].Affinities[Element.WIND];
            eLight = (int)TrainingManager.enemies[0].Affinities[Element.LIGHT];
            eDark = (int)TrainingManager.enemies[0].Affinities[Element.DARK];
        }
    }
}
