using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CreatorManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text actorsNumber;
    [SerializeField]
    Slider actorsSlider;

    [SerializeField]
    TMP_Dropdown actorsDropdown;
    [SerializeField]
    GameObject actorCreationWindowPrefab;
    [SerializeField]
    GameObject actorWindowsList;
    [SerializeField]
    List<GameObject> actorsCreationList;
    [SerializeField]
    GameObject ErrorPanel;
    [SerializeField]
    TMP_Text ErrorText;

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
        bool isDone = PrepareBattleData();
        if (!isDone)
        {
            ErrorText.text = "ERROR: Something went wrong with character conversion.";
            StartCoroutine(ErrorMessageVisibilityCooldown());
            return;
        }
        SceneManager.LoadScene("BattleScene");
    }
    bool PrepareBattleData()
    {
        BattleData.instance.party.Clear();
        BattleData.instance.enemies.Clear();
        //PARTY
        for(int i = 0; i<actorsCreationList.Count-1;i++)
        {
            if (!actorsCreationList[i].GetComponent<ActorCreationPanel>().CheckActorForConvertion())
            {
                return false;
            }
            BattleData.instance.party.Add(actorsCreationList[i].GetComponent<ActorCreationPanel>().ConvertActorToCharacter());
        }
        //ENEMY
        if (!actorsCreationList[actorsCreationList.Count - 1].GetComponent<ActorCreationPanel>().CheckActorForConvertion())
        {
            return false;
        }
        BattleData.instance.enemies.Add(actorsCreationList[actorsCreationList.Count - 1].GetComponent<ActorCreationPanel>().ConvertActorToCharacter());
        return true;
    }
    public void ClearActors()
    {
        actorsCreationList.Clear();
        foreach(Transform child in actorWindowsList.transform)
        {
            Destroy(child.gameObject);
        }
        actorsDropdown.ClearOptions();
    }
    public void CreateActors()
    {
        ClearActors();
        GameObject temp;
        List<string> options = new List<string>();
        
        //Create Party
        for(int i = 0; i < actorsSlider.value; i++)
        {
            temp = Instantiate(actorCreationWindowPrefab, actorWindowsList.transform);
            temp.SetActive(false);
            actorsCreationList.Add(temp);
            options.Add($"Actor {i+1}");
        }
        //Create Enemy
        temp = Instantiate(actorCreationWindowPrefab, actorWindowsList.transform);
        temp.SetActive(false);
        actorsCreationList.Add(temp);
        options.Add($"Enemy");

        actorsDropdown.AddOptions(options);
        ShowActorWindow();
    }
    public void ShowActorWindow()
    {
        foreach(GameObject temp in actorsCreationList)
        {
            temp.SetActive(false);
        }
        actorsCreationList[actorsDropdown.value].SetActive(true);
    }
    IEnumerator ErrorMessageVisibilityCooldown()
    {
        ErrorPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        ErrorPanel.SetActive(false);
    }
}
