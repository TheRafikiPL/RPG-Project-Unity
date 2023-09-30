using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleLog : ListView
{
    public void AddLog(string message, Color color)
    {
        GameObject temp;
        temp = Instantiate(prefab, ScrollableArea);
        temp.GetComponent<TMP_Text>().text = message;
        temp.GetComponent<TMP_Text>().color = color;
    }
}
