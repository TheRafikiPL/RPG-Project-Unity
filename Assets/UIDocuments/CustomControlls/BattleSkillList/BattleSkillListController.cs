using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleSkillListController
{
    VisualTreeAsset ListEntryTemplate;

    ListView SkillList;

    public void InitializeSkillList(ListView root, VisualTreeAsset listElementTemplate)
    {
        ListEntryTemplate = listElementTemplate;
        SkillList = root;
        //SkillList.selectionChanged += OnCharacterSelected;
    }

    List<Skill> AllSkills;

    void EnumerateAllSkills(List<Skill> skills)
    {
        AllSkills = new List<Skill>();
        AllSkills.AddRange(skills);
    }

    public void FillSkillList(List<Skill> skills)
    {
        EnumerateAllSkills(skills);
        SkillList.makeItem = () =>
        {
            var newListEntry = ListEntryTemplate.Instantiate();
            var newListEntryLogic = new BattleSkillListElement();
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);
            return newListEntry;
        };

        SkillList.bindItem = (item, index) =>
        {
            (item.userData as BattleSkillListElement).SetSkillData(AllSkills[index]);
        };
        SkillList.fixedItemHeight = 125;
        SkillList.itemsSource = AllSkills;
    }

    /*void OnSkillSelected(IEnumerable<object> selectedItems)
    {

    }*/
}
