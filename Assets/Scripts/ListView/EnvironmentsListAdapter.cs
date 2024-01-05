using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentsListAdapter : ListView
{
    [SerializeField]
    TrainingUI ui;
    void ClearList()
    {
        foreach (Transform child in ScrollableArea.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void UpdateContent(int count)
    {
        ClearList();
        GameObject temp;
        for (int i = 0; i < count; i++)
        {
            temp = Instantiate(prefab, ScrollableArea);
            int re = i;
            temp.GetComponent<Button>().onClick.AddListener(delegate { ClickButton(re); });
            temp.GetComponentInChildren<TMP_Text>().text = $"Env {i + 1}";
        }
    }
    public void ClickButton(int i)
    {
        //Training UI Update Info Panel
        ui.envindex = i;
    }
}
