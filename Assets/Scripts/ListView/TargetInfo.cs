using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetInfo : MonoBehaviour
{
    [SerializeField]
    Button button;
    [SerializeField]
    TMP_Text text;

    public void PrepareButton(string targetName, List<Character> targets)
    {
        text.text = targetName;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { BattleManager.instance.UseSkill(targets); });
    }
}
