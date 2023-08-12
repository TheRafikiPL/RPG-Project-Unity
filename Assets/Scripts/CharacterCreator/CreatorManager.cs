using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreatorManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text actorsNumber;
    [SerializeField]
    Slider actorsSlider;

    [SerializeField]
    GameObject actorCreationWindowPrefab;
    [SerializeField]
    List<GameObject> actorsCreationList;

    public void ButtonClick()
    {
        AudioSystem.instance.PlaySound("Click");
    }
    public void UpdateActorsNumber()
    {
        actorsNumber.text = actorsSlider.value.ToString();
    }
    public void GoBackButton()
    {
        AudioSystem.instance.PlaySound("Click");
        SceneManager.LoadScene("MainMenu");
    }
    public void StartBattleButton()
    {
        AudioSystem.instance.PlaySound("Click");
        SceneManager.LoadScene("BattleScene");
    }
}
