using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleUIController : MonoBehaviour
{
    //To attach from Unity
    [SerializeField]
    UIDocument battleUI;
    [SerializeField]
    UnityEngine.UI.Image backgroundImage;
    [SerializeField]
    VisualTreeAsset skillListElement;

    //To attach from Start
    BattleSkillListController skillListController;
    ListView targetList;
    ListView hpBarList;

    void Start()
    {
        var root = battleUI.rootVisualElement;
        skillListController = new BattleSkillListController();
        skillListController.InitializeSkillList(root.Q<ListView>("SkillList"),skillListElement);
        skillListController.FillSkillList(BattleManager.instance.skillList);
        hpBarList = root.Q<ListView>("HealthPanel");
    }

    //Skills Functions

}
