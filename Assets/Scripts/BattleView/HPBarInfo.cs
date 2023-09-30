using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBarInfo : MonoBehaviour
{
    public TMP_Text actorName;
    public Slider HPBar;
    public TMP_Text HPBarText;
    public Slider MPBar;
    public TMP_Text MPBarText;
    public void PrepareHPBar(Character c)
    {
        actorName.text = c.Nickname;
        HPBar.maxValue = c.MaxHP;
        HPBar.value = c.CurrentHP;
        HPBarText.text = $"{c.CurrentHP}/{c.MaxHP}";
        MPBar.maxValue = c.MaxMP;
        MPBar.value = c.CurrentMP;
        MPBarText.text = $"{c.CurrentMP}/{c.MaxMP}";
    }
    public void UpdateHPBar(Character c)
    {
        HPBar.value = c.CurrentHP;
        HPBarText.text = $"{c.CurrentHP}/{c.MaxHP}";
        MPBar.value = c.CurrentMP;
        MPBarText.text = $"{c.CurrentMP}/{c.MaxMP}";
    }
}
