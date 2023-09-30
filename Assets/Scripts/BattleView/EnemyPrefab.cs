using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPrefab : MonoBehaviour
{
    public Slider HPBar;
    public TMP_Text HPText;
    public void PrepareHPBar(Character c)
    {
        HPBar.maxValue = c.MaxHP;
        HPBar.value = c.CurrentHP;
        HPText.text = $"{c.CurrentHP}/{c.MaxHP}";
    }
    public void UpdateHPBar(Character c) 
    {
        HPBar.value = c.CurrentHP;
        HPText.text = $"{c.CurrentHP}/{c.MaxHP}";
    }
}
