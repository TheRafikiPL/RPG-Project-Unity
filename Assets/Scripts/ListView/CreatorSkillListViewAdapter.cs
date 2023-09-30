using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreatorSkillListViewAdapter : ListView
{
    void Start()
    {
        GameObject temp;
        for (int i = 2; i < BattleData.instance.skillList.Count; i++)
        {
            temp = Instantiate(prefab, ScrollableArea);
            temp.transform.Find("SkillId").GetComponent<TMP_Text>().text = i.ToString();
            temp.transform.Find("SkillName").GetComponent<TMP_Text>().text = BattleData.instance.skillList[i].SkillName;
            temp.transform.Find("SkillImage").GetComponent<Image>().sprite = BattleData.instance.skillList[i].AffinityTexture;
            temp.transform.Find("SkillCost").GetComponent<TMP_Text>().text = BattleData.instance.skillList[i].SkillCost;
        }
    }
}
