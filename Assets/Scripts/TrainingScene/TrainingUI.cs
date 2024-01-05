using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TrainingUI : MonoBehaviour
{
    [SerializeField]
    GameObject envs;
    [SerializeField]
    EnvStats stats;
    [SerializeField]
    List<EnvData> environments;
    [SerializeField]
    EnvironmentsListAdapter list;
    [SerializeField]
    public int envindex = 0;
    void Start()
    {
        foreach(EnvData env in envs.GetComponentsInChildren<EnvData>())
        {
            environments.Add(env);
        }
        list.UpdateContent(environments.Count);
    }
    private void Update()
    {
        UpdateEnvStats();
    }
    public void ManualResetEnv()
    {
        environments[envindex].TrainingManager.ResetEnvBtn();
    }
    string ToShortElementRelation(int el)
    {
        ElementRelation temp = (ElementRelation)el;
        switch(temp)
        {
            case ElementRelation.WEAK:
                return "wk";
            case ElementRelation.STRONG:
                return "str";
            case ElementRelation.NORMAL:
                return "-";
            case ElementRelation.ABSORB:
                return "abs";
            case ElementRelation.REPEL:
                return "rep";
            case ElementRelation.NULL:
                return "null";
        }
        return "";
    }
    private void UpdateEnvStats()
    {
        EnvData temp = environments[envindex];
        stats.PressTurns.text = temp.PressedTurns.ToString();
        stats.Turns.text = temp.Turns.ToString();
        stats.ChosenSkill.text = temp.ChosenSkill.ToString();
        stats.ChosenActor.text = temp.ChosenActor.ToString();

        stats.actor1.MaxHP.text = temp.a1MaxHP.ToString();
        stats.actor1.CurrentHP.text = temp.a1CurrentHP.ToString();
        stats.actor1.MaxMP.text = temp.a1MaxMP.ToString();
        stats.actor1.CurrentMP.text = temp.a1CurrentMP.ToString();
        stats.actor1.Strength.text = temp.a1Strength.ToString();
        stats.actor1.Magic.text = temp.a1Magic.ToString();
        stats.actor1.Dexterity.text = temp.a1Dexterity.ToString();
        stats.actor1.Agility.text = temp.a1Agility.ToString();
        stats.actor1.Luck.text = temp.a1Luck.ToString();
        stats.actor1.Physical.text = ToShortElementRelation(temp.a1Physical);
        stats.actor1.Fire.text = ToShortElementRelation(temp.a1Fire);
        stats.actor1.Ice.text = ToShortElementRelation(temp.a1Ice);
        stats.actor1.Electricity.text = ToShortElementRelation(temp.a1Electricity);
        stats.actor1.Wind.text = ToShortElementRelation(temp.a1Wind);
        stats.actor1.Light.text = ToShortElementRelation(temp.a1Light);
        stats.actor1.Dark.text = ToShortElementRelation(temp.a1Dark);

        stats.actor2.MaxHP.text = temp.a2MaxHP.ToString();
        stats.actor2.CurrentHP.text = temp.a2CurrentHP.ToString();
        stats.actor2.MaxMP.text = temp.a2MaxMP.ToString();
        stats.actor2.CurrentMP.text = temp.a2CurrentMP.ToString();
        stats.actor2.Strength.text = temp.a2Strength.ToString();
        stats.actor2.Magic.text = temp.a2Magic.ToString();
        stats.actor2.Dexterity.text = temp.a2Dexterity.ToString();
        stats.actor2.Agility.text = temp.a2Agility.ToString();
        stats.actor2.Luck.text = temp.a2Luck.ToString();
        stats.actor2.Physical.text = ToShortElementRelation(temp.a2Physical);
        stats.actor2.Fire.text = ToShortElementRelation(temp.a2Fire);
        stats.actor2.Ice.text = ToShortElementRelation(temp.a2Ice);
        stats.actor2.Electricity.text = ToShortElementRelation(temp.a2Electricity);
        stats.actor2.Wind.text = ToShortElementRelation(temp.a2Wind);
        stats.actor2.Light.text = ToShortElementRelation(temp.a2Light);
        stats.actor2.Dark.text = ToShortElementRelation(temp.a2Dark);

        stats.actor3.MaxHP.text = temp.a3MaxHP.ToString();
        stats.actor3.CurrentHP.text = temp.a3CurrentHP.ToString();
        stats.actor3.MaxMP.text = temp.a3MaxMP.ToString();
        stats.actor3.CurrentMP.text = temp.a3CurrentMP.ToString();
        stats.actor3.Strength.text = temp.a3Strength.ToString();
        stats.actor3.Magic.text = temp.a3Magic.ToString();
        stats.actor3.Dexterity.text = temp.a3Dexterity.ToString();
        stats.actor3.Agility.text = temp.a3Agility.ToString();
        stats.actor3.Luck.text = temp.a3Luck.ToString();
        stats.actor3.Physical.text = ToShortElementRelation(temp.a3Physical);
        stats.actor3.Fire.text = ToShortElementRelation(temp.a3Fire);
        stats.actor3.Ice.text = ToShortElementRelation(temp.a3Ice);
        stats.actor3.Electricity.text = ToShortElementRelation(temp.a3Electricity);
        stats.actor3.Wind.text = ToShortElementRelation(temp.a3Wind);
        stats.actor3.Light.text = ToShortElementRelation(temp.a3Light);
        stats.actor3.Dark.text = ToShortElementRelation(temp.a3Dark);

        stats.actor4.MaxHP.text = temp.a4MaxHP.ToString();
        stats.actor4.CurrentHP.text = temp.a4CurrentHP.ToString();
        stats.actor4.MaxMP.text = temp.a4MaxMP.ToString();
        stats.actor4.CurrentMP.text = temp.a4CurrentMP.ToString();
        stats.actor4.Strength.text = temp.a4Strength.ToString();
        stats.actor4.Magic.text = temp.a4Magic.ToString();
        stats.actor4.Dexterity.text = temp.a4Dexterity.ToString();
        stats.actor4.Agility.text = temp.a4Agility.ToString();
        stats.actor4.Luck.text = temp.a4Luck.ToString();
        stats.actor4.Physical.text = ToShortElementRelation(temp.a4Physical);
        stats.actor4.Fire.text = ToShortElementRelation(temp.a4Fire);
        stats.actor4.Ice.text = ToShortElementRelation(temp.a4Ice);
        stats.actor4.Electricity.text = ToShortElementRelation(temp.a4Electricity);
        stats.actor4.Wind.text = ToShortElementRelation(temp.a4Wind);
        stats.actor4.Light.text = ToShortElementRelation(temp.a4Light);
        stats.actor4.Dark.text = ToShortElementRelation(temp.a4Dark);

        stats.enemy.MaxHP.text = temp.eMaxHP.ToString();
        stats.enemy.CurrentHP.text = temp.eCurrentHP.ToString();
        stats.enemy.MaxMP.text = temp.eMaxMP.ToString();
        stats.enemy.CurrentMP.text = temp.eCurrentMP.ToString();
        stats.enemy.Strength.text = temp.eStrength.ToString();
        stats.enemy.Magic.text = temp.eMagic.ToString();
        stats.enemy.Dexterity.text = temp.eDexterity.ToString();
        stats.enemy.Agility.text = temp.eAgility.ToString();
        stats.enemy.Luck.text = temp.eLuck.ToString();
        stats.enemy.Physical.text = ToShortElementRelation(temp.ePhysical);
        stats.enemy.Fire.text = ToShortElementRelation(temp.eFire);
        stats.enemy.Ice.text = ToShortElementRelation(temp.eIce);
        stats.enemy.Electricity.text = ToShortElementRelation(temp.eElectricity);
        stats.enemy.Wind.text = ToShortElementRelation(temp.eWind);
        stats.enemy.Light.text = ToShortElementRelation(temp.eLight);
        stats.enemy.Dark.text = ToShortElementRelation(temp.eDark);
    }
}
