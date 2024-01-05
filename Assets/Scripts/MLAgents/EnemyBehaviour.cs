using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EnemyBehaviour : Agent
{
    [Header("Rewards")]
    public float winReward = 1f;
    public float loseReward = -1f;
    public float neutralReward = 0.2f;  //Neutral Attack
    public float weakReward = 0.5f;
    public float strongReward = -0.1f;
    public float nullReward = -0.3f;
    public float absorbReward = -0.5f;
    public float repelReward = -0.5f;
    public float healReward = 0.3f;
    [Header("Actions")]
    public int chosenSkill;
    public int chosenActor;
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(BattleManager.instance.PrepareObservations());
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        chosenSkill = actions.DiscreteActions[0];
        chosenActor = actions.DiscreteActions[1];
        Debug.Log($"Actions Called:\n{chosenSkill} - Skill Index\n{chosenActor} - Actor Index");
    }
    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        //SetSkills
        List<int> skills = BattleData.instance.PrepareDataForAI();
        for (int i = 0; i < 11; i++)
        {
            //EXCLUDE GUARD SKILL
            if(skills.Contains(i) && i != 1)
            {
                actionMask.SetActionEnabled(0, i, true);
            }
            else
            {
                actionMask.SetActionEnabled(0, i, false);
            }
        }
        //SetActors
        for (int i = 0; i < BattleData.instance.party.Count; i++)
        {
            if (!BattleData.instance.party[i].CheckStatesByName("Death"))
            {
                actionMask.SetActionEnabled(1, i, true);
            }
            else
            {
                actionMask.SetActionEnabled(1, i, false);
            }
        }
        for (int i = BattleData.instance.party.Count; i < 4; i++)
        {
            actionMask.SetActionEnabled(1, i, false);
        }
    }
}
