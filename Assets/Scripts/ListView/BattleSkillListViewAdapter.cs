using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSkillListViewAdapter : ListView
{
    void ClearList()
    {
        foreach (Transform child in ScrollableArea.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void UpdateContent(Character c)
    {
        ClearList();
        GameObject temp;
        for (int i = 0; i < c.Skills.Count; i++)
        {
            temp = Instantiate(prefab, ScrollableArea);
            temp.GetComponent<BattleSkillInfo>().skillName.text = c.Skills[i].SkillName;
            temp.GetComponent<BattleSkillInfo>().skillAffinity.sprite = c.Skills[i].AffinityTexture;
            temp.GetComponent<BattleSkillInfo>().skillCost.text = c.Skills[i].SkillCost;
            Skill s1 = c.Skills[i];
            temp.GetComponent<Button>().onClick.AddListener(delegate { ClickButton(s1); });
            if(!(s1.CharacterHaveHP(c) && s1.CharacterHaveMP(c)))
            {
                temp.GetComponent<Button>().enabled = false;
                temp.GetComponent<Image>().color = Color.grey;
            }
        }
    }
    public void ClickButton(Skill s)
    {
        BattleManager.instance.selectedSkill = s;
        BattleManager.instance.ShowTargets();
    }
}
