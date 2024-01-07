using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EnemyTrainer : Agent
{
    public TrainingManager trainingManager;
    public int ends = 0;
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

    public float positiveRewards = 0f;
    public float negativeRewards = 0f;

    bool tryingToReset = false;
    bool firstRun = true;
    bool resultGenerated = false;
    bool episodeOver = false;

    private void Awake()
    {
        Academy.Instance.AutomaticSteppingEnabled = false;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    { }
    public override void CollectObservations(VectorSensor sensor)
    {
        if(sensor == null)
        {
            Debug.LogWarning("Input is null");
            return;
        }
        sensor.AddObservation(trainingManager.PrepareObservations());
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        resultGenerated = true;
        chosenSkill = actions.DiscreteActions[0];
        chosenActor = actions.DiscreteActions[1];
        //Debug.Log($"Actions Called:\n{chosenSkill} - Skill Index\n{chosenActor} - Actor Index");
    }
    float CalculateReward()
    {
        if(positiveRewards-negativeRewards>0 || positiveRewards - negativeRewards < 0)
        {
            return (positiveRewards + negativeRewards) / (positiveRewards - negativeRewards);
        }
        return 0;
    }
    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        //SetSkills
        List<int> skills = trainingManager.PrepareDataForAI();
        for (int i = 0; i < 11; i++)
        {
            //EXCLUDE GUARD SKILL
            if (skills.Contains(i))
            {
                actionMask.SetActionEnabled(0, i, true);
            }
            else
            {
                actionMask.SetActionEnabled(0, i, false);
            }
        }
        //SetActors
        bool isSomeoneAlive = false;
        for (int i = 0; i < trainingManager.party.Count; i++)
        {
            if(!trainingManager.party[i].CheckStatesByName("Death"))
            {
                isSomeoneAlive = true;
                break;
            }
        }
        if(isSomeoneAlive)
        {
            for (int i = 0; i < trainingManager.party.Count; i++)
            {
                if (!trainingManager.party[i].CheckStatesByName("Death"))
                {
                    actionMask.SetActionEnabled(1, i, true);
                }
                else
                {
                    actionMask.SetActionEnabled(1, i, false);
                }
            }
            for (int i = trainingManager.party.Count; i < 4; i++)
            {
                actionMask.SetActionEnabled(1, i, false);
            }
        }
    }
    public override void OnEpisodeBegin()
    {
        if (firstRun)
        {
            firstRun = false;
            return;
        }
        if (tryingToReset)
        {
            Debug.LogWarning("Already trying to reset...");
        }
        tryingToReset = true;
        trainingManager.ResetEnvironment();
        positiveRewards = 0f;
        negativeRewards = 0f;
        episodeOver = false;
        /*trainingManager.confirmation.SetActive(true);
        trainingManager.startButton.gameObject.SetActive(false);*/
    }
    public void GetOutput()
    {
        int counter = 0;

        while (!resultGenerated)
        {
            Thread.Sleep(1);
            counter++;
            if (counter >= 5000)
            {
                Debug.LogWarning("Possible infinite loop: breaking out of loop.");
                break;
            }
            if (episodeOver)
            {
                Debug.LogWarning("Episode over, breaking.");
                break;
            }
        }

        resultGenerated = false;
    }
    public void NotifyEndEpisode()
    {
        if (episodeOver)
        {
            return;
        }
        AddReward(CalculateReward());
        episodeOver = true;
        /*if(ends>20)
        {
            Debug.Break();
        }*/
        EndEpisode();
    }
}
