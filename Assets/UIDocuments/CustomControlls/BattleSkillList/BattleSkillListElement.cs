using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleSkillListElement
{
    VisualElement visualElement;

    public void SetVisualElement(VisualElement element)
    {
        visualElement = element;
    }
    public void SetSkillData(Skill skill)
    {
        visualElement.Q<Button>("SkillAffinity").style.backgroundImage = new StyleBackground(skill.AffinityTexture);
        visualElement.Q<Label>("SkillName").text = skill.SkillName;
        visualElement.Q<Label>("SkillCost").text = skill.SkillCost;
    }
}
