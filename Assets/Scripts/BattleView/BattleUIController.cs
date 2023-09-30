using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BattleUIController : MonoBehaviour
{
    [Header("Background Canvas")]
    [SerializeField]
    Image backgroundImage;

    [Header("Battle Canvas")]
    [SerializeField]
    RectTransform turns;
    [SerializeField]
    GameObject turnPrefab;
    [SerializeField]
    Sprite turnFalseImage;
    [SerializeField]
    RectTransform HPBarList;
    [SerializeField]
    List<HPBarInfo> partyHPBars;
    [SerializeField]
    GameObject HPBarPrefab;
    [SerializeField]
    BattleSkillListViewAdapter skillList;
    [SerializeField]
    TargetsListAdapter targetList;
    public BattleLog battleLog;
    [SerializeField]
    GameObject messageBox;
    [SerializeField]
    TMP_Text messageText;

    [Header("Actors Canvas")]
    [SerializeField]
    RectTransform partyBox;
    [SerializeField]
    RectTransform enemiesBox;
    [SerializeField]
    GameObject partyPrefab;
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    List<GameObject> partyList;
    [SerializeField]
    GameObject enemy;

    public void ShowSkills()
    {
        skillList.gameObject.SetActive(true);
        if (BattleManager.instance.isPlayerTurn)
        {
            skillList.GetComponent<BattleSkillListViewAdapter>().UpdateContent(BattleData.instance.party[BattleManager.instance.currentIndex]);
        }
    }
    public void HideSkills()
    {
        skillList.gameObject.SetActive(false);
    }
    public void CreateActors()
    {
        GameObject temp;
        foreach(Character c in BattleData.instance.party)
        {
            temp = Instantiate(partyPrefab, partyBox);
            temp.GetComponent<Image>().sprite = c.Sprite;
            partyList.Add(temp);
        }
        temp = Instantiate(enemyPrefab, enemiesBox);
        temp.GetComponent<Image>().sprite = BattleData.instance.enemies[0].Sprite;
        enemy = temp;
    }

    public void CreateHPBars()
    {
        GameObject temp;
        foreach(Character c in BattleData.instance.party)
        {
            temp = Instantiate(HPBarPrefab, HPBarList);
            temp.GetComponent<HPBarInfo>().PrepareHPBar(c);
            partyHPBars.Add(temp.GetComponent<HPBarInfo>());
        }
        enemy.GetComponent<EnemyPrefab>().PrepareHPBar(BattleData.instance.enemies[0]);
    }
    public void DehighlightActor()
    {
        if (BattleManager.instance.isPlayerTurn)
        {
            partyBox.GetChild(BattleManager.instance.currentIndex).transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
        enemiesBox.GetChild(BattleManager.instance.currentIndex).transform.GetChild(0).gameObject.SetActive(false);
    }
    public void HighlightActor()
    {
        if(BattleManager.instance.isPlayerTurn)
        {
            partyBox.GetChild(BattleManager.instance.currentIndex).transform.GetChild(0).gameObject.SetActive(true);
            return;
        }
        enemiesBox.GetChild(BattleManager.instance.currentIndex).transform.GetChild(0).gameObject.SetActive(true);
    }
    public void UpdateHPInfo()
    {
        for (int i = 0; i < partyHPBars.Count; i++)
        {
            partyHPBars[i].UpdateHPBar(BattleData.instance.party[i]);
        }
        enemy.GetComponent<EnemyPrefab>().UpdateHPBar(BattleData.instance.enemies[0]);
    }
    public void FillTurnQueue()
    {
        for (int i = 0; i < turns.childCount; i++)
        {
            Destroy(turns.GetChild(i).gameObject);
        }
        GameObject temp;
        foreach (bool turn in BattleManager.instance.turnQueue)
        {
            temp = Instantiate(turnPrefab, turns);
            if(!turn)
            {
                temp.GetComponent<Image>().sprite = turnFalseImage;
            }
        }
    }
    public void ShowTargetsUI()
    {
        targetList.gameObject.SetActive(true);
        targetList.UpdateContent(BattleManager.instance.selectedSkill.Scope);
        skillList.gameObject.SetActive(false);
    }
    public void HideTargetsTargetsUI()
    {
        skillList.gameObject.SetActive(true);
        targetList.gameObject.SetActive(false);
    }
    public void EndBattle(bool isWin)
    {
        skillList.gameObject.SetActive(false);
        targetList.gameObject.SetActive(false);
        if (isWin)
        {
            messageText.text = "Victory";
        }
        else
        {
            messageText.text = "Defeat";
        }
        messageBox.gameObject.SetActive(true);
    }
}
