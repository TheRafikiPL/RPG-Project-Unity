using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TargetsListAdapter : ListView
{
    void ClearList()
    {
        foreach (Transform child in ScrollableArea.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void UpdateContent(TargetChoice targets)
    {
        ClearList();
        switch(targets)
        {
            case TargetChoice.ONE_ENEMY:
                CreateList(BattleData.instance.enemies);
                break;
            case TargetChoice.ALL_ENEMIES:
                CreateList("All Enemies");
                break;
            case TargetChoice.ONE_ALLY:
                CreateList(BattleData.instance.party);
                break;
            case TargetChoice.ALL_ALLIES:
                CreateList("All Allies");
                break;
            case TargetChoice.ONE_DEAD_ALLY:
                CreateList(BattleData.instance.party, true);
                break;
            case TargetChoice.ALL_DEAD_ALLIES:
                CreateList("All Dead Allies");
                break;
            case TargetChoice.USER:
                CreateList(new List<Character> { BattleData.instance.party[BattleManager.instance.currentIndex] });
                break;
            case TargetChoice.ALL:
                CreateList("All Characters");
                break;
            default:
                CreateList(new List<Character>());
                break;
        }
    }
    public void CreateList(List<Character> targets, bool isDead = false)
    {
        //CREATE ACTOR BUTTONS
        GameObject temp;
        for (int i = 0; i < targets.Count; i++)
        {
            if(isDead == targets[i].CheckStatesByName("Death"))
            {
                temp = Instantiate(prefab, ScrollableArea);
                temp.GetComponent<TargetInfo>().PrepareButton(targets[i].Nickname, new List<Character> { targets[i] });
            }
        }
        //CREATE BACK BUTTON
        temp = Instantiate(prefab, ScrollableArea);
        temp.GetComponent<Button>().onClick.AddListener(() => BackButton());
    }
    public void CreateList(string targets)
    {
        //CREATE ACTOR BUTTONS
        GameObject temp;
        temp = Instantiate(prefab, ScrollableArea);
        List<Character> characters = new List<Character>();
        switch(targets)
        {
            case "All Enemies":
                for (int i = 0; i < BattleData.instance.enemies.Count; i++)
                {
                    if (!BattleData.instance.enemies[i].CheckStatesByName("Death"))
                    {
                        characters.Add(BattleData.instance.enemies[i]);
                    }
                }
                break;
            case "All Allies":
                for (int i = 0; i < BattleData.instance.party.Count; i++)
                {
                    if (!BattleData.instance.party[i].CheckStatesByName("Death"))
                    {
                        characters.Add(BattleData.instance.party[i]);
                    }
                }
                break;
            case "All Dead Allies":
                for (int i = 0; i < BattleData.instance.enemies.Count; i++)
                {
                    if (BattleData.instance.party[i].CheckStatesByName("Death"))
                    {
                        characters.Add(BattleData.instance.party[i]);
                    }
                }
                break;
            case "All Characters":
                for (int i = 0; i < BattleData.instance.party.Count; i++)
                {
                    if (!BattleData.instance.party[i].CheckStatesByName("Death"))
                    {
                        characters.Add(BattleData.instance.party[i]);
                    }
                }
                for (int i = 0; i < BattleData.instance.enemies.Count; i++)
                {
                    if (!BattleData.instance.enemies[i].CheckStatesByName("Death"))
                    {
                        characters.Add(BattleData.instance.enemies[i]);
                    }
                }
                break;
            default:
                break;
        }
        temp.GetComponent<TargetInfo>().PrepareButton(targets, characters);
        //CREATE BACK BUTTON
        temp = Instantiate(prefab, ScrollableArea);
        temp.GetComponent<Button>().onClick.AddListener(() => BackButton());
    }
    public void BackButton()
    {
        BattleManager.instance.HideTargets();
    }
}
