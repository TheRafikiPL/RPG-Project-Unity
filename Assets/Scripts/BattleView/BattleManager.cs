using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    [SerializeField]
    BattleUIController UIController;
    public Skill selectedSkill;
    public bool isPlayerTurn = true;
    public bool battleOn;

    public Character currentCharacter;

    public List<int> moveQueue = new List<int>();
    public int currentIndex;
    public List<bool> turnQueue = new List<bool>();

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        AudioSystem.instance.PlayBattleTheme();
        BattleSetup();
        StartSide();
    }
    void BattleSetup()
    {
        battleOn = true;
        UIController.CreateActors();
        UIController.CreateHPBars();
    }
    void StartSide()
    {
        CreateTurns();
        CreateMoveQueue();
        currentIndex = moveQueue[0];
        moveQueue.RemoveAt(0);
        moveQueue.Add(currentIndex);
        StartTurn();
    }
    void StartTurn()
    {
        SetCurrentCharacter();
        UIController.FillTurnQueue();
        UIController.HighlightActor();
        if(isPlayerTurn)
        {
            UIController.ShowSkills();
        }
        else
        {
            UIController.HideSkills();
        }
    }
    bool CheckWin()
    {
        foreach(Character c in BattleData.instance.enemies)
        {
            if(c.CurrentHP>0)
            {
                return false;
            }
        }
        return true;
    }
    bool CheckLose()
    {
        foreach (Character c in BattleData.instance.party)
        {
            if (c.CurrentHP > 0)
            {
                return false;
            }
        }
        return true;
    }
    void EndTurn()
    {
        UIController.DehighlightActor();
        if(CheckLose())
        {
            UIController.EndBattle(false);
            return;
        }
        if(CheckWin())
        {
            UIController.EndBattle(true);
            return;
        }
        HideTargets();
        if(turnQueue.Count < 1)
        {
            EndSide();
            return;
        }
        currentIndex = moveQueue[0];
        moveQueue.RemoveAt(0);
        moveQueue.Add(currentIndex);
        StartTurn();
    }
    void EndSide()
    {
        isPlayerTurn = !isPlayerTurn;
        StartSide();
    }

    void SetCurrentCharacter()
    {
        if (isPlayerTurn)
        {
            currentCharacter = BattleData.instance.party[moveQueue[0]];
            return;
        }
        currentCharacter = BattleData.instance.enemies[moveQueue[0]];
    }
    void CreateMoveQueue()
    {
        moveQueue.Clear();
        if (isPlayerTurn)
        {
            for (int i = 0; i < BattleData.instance.party.Count; i++)
            {
                moveQueue.Add(i);
            }
            for (int i = 0; i < moveQueue.Count - 1; i++)
            {
                for (int j = i + 1; j < moveQueue.Count; j++)
                {
                    if (BattleData.instance.party[moveQueue[i]].Agility < BattleData.instance.party[moveQueue[j]].Agility)
                    {
                        int temp = moveQueue[i];
                        moveQueue[i] = moveQueue[j];
                        moveQueue[j] = temp;
                    }
                }
            }
            return;
        }
        for (int i = 0; i < BattleData.instance.enemies.Count; i++)
        {
            moveQueue.Add(i);
        }
        for (int i = 0; i < moveQueue.Count - 1; i++)
        {
            for (int j = i + 1; j < moveQueue.Count; j++)
            {
                if (BattleData.instance.enemies[moveQueue[i]].Agility > BattleData.instance.enemies[moveQueue[j]].Agility)
                {
                    int temp = moveQueue[i];
                    moveQueue[i] = moveQueue[j];
                    moveQueue[j] = temp;
                }
            }
        }
    }
    void CreateTurns()
    {
        turnQueue.Clear();
        if (isPlayerTurn)
        {
            for (int i = 0; i < BattleData.instance.party.Count; i++)
            {
                if (!BattleData.instance.party[i].States.Contains(BattleData.instance.statesList[0]))
                {
                    turnQueue.Add(true);
                }
            }
            return;
        }
        for (int i = 0; i < BattleData.instance.enemies.Count; i++)
        {
            if (!BattleData.instance.enemies[i].States.Contains(BattleData.instance.statesList[0]))
            {
                turnQueue.Add(true);
            }
        }
        if (turnQueue.Count < 2)
        {
            turnQueue.Add(true);
        }
    }
    void ChangeTurns(ElementRelation elementRelation)
    {
        switch (elementRelation)
        {
            case ElementRelation.WEAK:
                PressTurn();
                break;
            case ElementRelation.REPEL:
                DeleteTurns(4);
                break;
            case ElementRelation.ABSORB:
                DeleteTurns(4);
                break;
            case ElementRelation.NULL:
                DeleteTurns(2);
                break;
            default:
                DeleteTurns(1);
                break;
        }
    }
    void PressTurn()
    {
        for (int i = 0; i < turnQueue.Count; i++)
        {
            if (turnQueue[turnQueue.Count - i - 1])
            {
                turnQueue[turnQueue.Count - i - 1] = false;
                return;
            }
        }
        DeleteTurns(1);
    }
    void DeleteTurns(int count)
    {
        if (turnQueue.Count - count >= 0)
        {
            for (int i = 0; i < count; i++)
            {
                turnQueue.RemoveAt(turnQueue.Count - 1);
            }
            return;
        }
        turnQueue.Clear();
    }
    //Button functions
    public void LeaveBattle()
    {
        AudioSystem.instance.PlaySound("Click");
        SceneManager.LoadScene("CharacterCreator");
        AudioSystem.instance.PlayMenuBackMusic();
    }
    public void ShowTargets()
    {
        UIController.ShowTargetsUI();
    }
    public void HideTargets()
    {
        UIController.HideTargetsTargetsUI();
    }
    public void UseSkill(List<Character> targets)
    {
        currentCharacter.ChangeHP(-selectedSkill.SkillCostPercentHP, true);
        currentCharacter.ChangeMP(-selectedSkill.SkillCostNumberMP);
        foreach(Character target in targets)
        {
            ChangeTurns(SkillActivation(currentCharacter, target));
        }
        UIController.UpdateHPInfo();
        EndTurn();
    }
    public ElementRelation SkillActivation(Character attacker, Character defender)
    {
        //CALCULATE DAMAGE/HEALING
        int damage = selectedSkill.CalculateDamage(attacker, defender);

        //DO YOUR WORK
        ElementRelation result = defender.Affinities[selectedSkill.Affinity];
        switch (selectedSkill.SkillType)
        {
            case SkillType.HP_DAMAGE:
                SkillDamage(damage, attacker, defender, result);
                break;
            case SkillType.MP_DAMAGE:
                SkillDamage(damage, attacker, defender, result, false);
                break;
            case SkillType.HP_RECOVER:
                result = ElementRelation.NORMAL;
                SkillRecover(damage, attacker);
                break;
            case SkillType.HP_RECOVER_PERCENT:
                result = ElementRelation.NORMAL;
                SkillRecover(damage, attacker, true);
                break;
            case SkillType.MP_RECOVER:
                result = ElementRelation.NORMAL;
                SkillRecover(damage, attacker, false, false);
                break;
            case SkillType.MP_RECOVER_PERCENT:
                result = ElementRelation.NORMAL;
                SkillRecover(damage, attacker, true, false);
                break;
            case SkillType.HP_DRAIN:
                SkillDrain(damage, attacker, defender, result);
                break;
            case SkillType.MP_DRAIN:
                SkillDrain(damage, attacker, defender, result, false);
                break;
            default:
                result = ElementRelation.NORMAL;
                break;
        }
        //RETURN RESULT
        return result;
    }

    void SkillDamage(int damage, Character attacker, Character defender, ElementRelation relation, bool isHP = true)
    {
        if(isHP)
        {
            switch (relation)
            {
                case ElementRelation.WEAK:
                    damage = Mathf.CeilToInt(damage * 1.2f);
                    defender.ChangeHP(-damage);
                    UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname} weak point. Deals {damage} damage.", Color.red);
                    break;
                case ElementRelation.STRONG:
                    damage = Mathf.CeilToInt(damage * 0.8f);
                    defender.ChangeHP(-damage);
                    UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}'s strong point. Deals {damage} damage.", Color.blue);
                    break;
                case ElementRelation.NORMAL:
                    UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}. Deals {damage} damage.", Color.white);
                    defender.ChangeHP(-damage);
                    break;
                case ElementRelation.ABSORB:
                    defender.ChangeHP(damage);
                    UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}'s absorb point. Drains {damage} damage.", Color.green);
                    break;
                case ElementRelation.REPEL:
                    switch(attacker.Affinities[selectedSkill.Affinity])
                    {
                        case ElementRelation.NULL:
                            UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}'s repel point. {attacker.Nickname} nullifies repeled damage.", Color.magenta);
                            break;
                        case ElementRelation.ABSORB: 
                            attacker.ChangeHP(damage);
                            UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}'s repel point. {attacker.Nickname} absorbs {damage} repeled damage.", Color.green);
                            break;
                        case ElementRelation.REPEL:
                            UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}'s repel point. {attacker.Nickname} have repel on this element so damage is nullified.", Color.magenta);
                            break;
                        default:
                            attacker.ChangeHP(-damage);
                            UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}'s repel point. {attacker.Nickname} gets {damage} damage.", Color.cyan);
                            break;
                    }
                    break;
                case ElementRelation.NULL:
                    UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}'s null point.", Color.magenta);
                    break;
            }
        }
        else
        {
            switch (relation)
            {
                case ElementRelation.WEAK:
                    damage = Mathf.CeilToInt(damage * 1.2f);
                    defender.ChangeMP(-damage);
                    break;
                case ElementRelation.STRONG:
                    damage = Mathf.CeilToInt(damage * 0.8f);
                    defender.ChangeMP(-damage);
                    break;
                case ElementRelation.NORMAL:
                    defender.ChangeMP(-damage);
                    break;
                case ElementRelation.ABSORB:
                    defender.ChangeMP(damage);
                    break;
                case ElementRelation.REPEL:
                    switch (attacker.Affinities[selectedSkill.Affinity])
                    {
                        case ElementRelation.NULL:
                            break;
                        case ElementRelation.ABSORB:
                            attacker.ChangeMP(damage);
                            break;
                        case ElementRelation.REPEL:
                            break;
                        default:
                            attacker.ChangeMP(-damage);
                            break;
                    }
                    break;
            }
        }
        
    }
    void SkillRecover(int heal, Character target, bool isPercent = false, bool isHP = true)
    {
        if(isHP)
        {
            if (isPercent)
            {
                target.ChangeHP(heal, true);
            }
            else
            {
                target.ChangeHP(heal);
            }
        }
        else
        {
            if(isPercent)
            {
                target.ChangeMP(heal, true);
            }
            else
            {
                target.ChangeMP(heal);
            }
        }
    }
    void SkillDrain(int damage, Character attacker, Character defender, ElementRelation relation, bool isHP = true)
    {
        if (isHP)
        {
            switch (relation)
            {
                case ElementRelation.WEAK:
                    damage = Mathf.CeilToInt(damage * 1.2f);
                    defender.ChangeHP(-damage);
                    attacker.ChangeHP(damage);
                    break;
                case ElementRelation.STRONG:
                    damage = Mathf.CeilToInt(damage * 0.8f);
                    defender.ChangeHP(-damage);
                    attacker.ChangeHP(damage);
                    break;
                case ElementRelation.NORMAL:
                    defender.ChangeHP(-damage);
                    attacker.ChangeHP(damage);
                    break;
                case ElementRelation.ABSORB:
                    defender.ChangeHP(damage);
                    break;
                case ElementRelation.REPEL:
                    switch (attacker.Affinities[selectedSkill.Affinity])
                    {
                        case ElementRelation.NULL:
                            break;
                        case ElementRelation.ABSORB:
                            attacker.ChangeHP(damage);
                            break;
                        case ElementRelation.REPEL:
                            break;
                        default:
                            attacker.ChangeHP(-damage);
                            defender.ChangeHP(damage);
                            break;
                    }
                    break;
            }
        }
        else
        {
            switch (relation)
            {
                case ElementRelation.WEAK:
                    damage = Mathf.CeilToInt(damage * 1.2f);
                    defender.ChangeMP(-damage);
                    attacker.ChangeMP(damage);
                    break;
                case ElementRelation.STRONG:
                    damage = Mathf.CeilToInt(damage * 0.8f);
                    defender.ChangeMP(-damage);
                    attacker.ChangeMP(damage);
                    break;
                case ElementRelation.NORMAL:
                    defender.ChangeMP(-damage);
                    attacker.ChangeMP(damage);
                    break;
                case ElementRelation.ABSORB:
                    defender.ChangeMP(damage);
                    break;
                case ElementRelation.REPEL:
                    switch (attacker.Affinities[selectedSkill.Affinity])
                    {
                        case ElementRelation.NULL:
                            break;
                        case ElementRelation.ABSORB:
                            attacker.ChangeMP(damage);
                            break;
                        case ElementRelation.REPEL:
                            break;
                        default:
                            attacker.ChangeMP(-damage);
                            defender.ChangeMP(damage);
                            break;
                    }
                    break;
            }
        }
    }
}