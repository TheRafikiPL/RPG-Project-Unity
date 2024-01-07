using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    [SerializeField]
    BattleUIController UIController;
    public Skill selectedSkill;
    public bool isPlayerTurn = true;
    public bool isBusy = true;
    public GameState gameState;

    public Character currentCharacter;
    public List<int> moveQueue = new List<int>();
    public int currentIndex = 0;
    public List<bool> turnQueue = new List<bool>();

    public EnemyBehaviour enemyAI;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        AudioSystem.instance.PlayBattleTheme();
        BattleSetup();
        gameState = GameState.START_SIDE;
        isBusy = false;
    }
    private void Update()
    {
        if (isBusy)
        {
            return;
        }
        isBusy = true;
        switch (gameState)
        {
            case GameState.START_SIDE:
                StartSide();
                break;
            case GameState.START_TURN:
                StartTurn();
                break;
            case GameState.END_SIDE:
                EndSide();
                break;
            case GameState.END_TURN:
                EndTurn();
                break;
        }
    }
    void BattleSetup()
    {
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
        //StartTurn();

        gameState = GameState.START_TURN;
        isBusy = false;
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
            AIActions();
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
            enemyAI.NotifyEndEpisode();
            UIController.EndBattle(false);
            return;
        }
        if(CheckWin())
        {
            enemyAI.NotifyEndEpisode();
            UIController.EndBattle(true);
            return;
        }
        HideTargets();
        if(turnQueue.Count < 1)
        {
            gameState = GameState.END_SIDE;
            isBusy = false;
            //EndSide();
            return;
        }
        currentIndex = moveQueue[0];
        moveQueue.RemoveAt(0);
        moveQueue.Add(currentIndex);
        if (isPlayerTurn)
        {
            CheckMoveQueue();
        }
        gameState = GameState.START_TURN;
        isBusy = false;
        //StartTurn();
    }
    void EndSide()
    {
        isPlayerTurn = !isPlayerTurn;
        gameState = GameState.START_SIDE;
        isBusy = false;
        //StartSide();
    }

    void SetCurrentCharacter()
    {
        if (isPlayerTurn)
        {
            currentCharacter = BattleData.instance.party[currentIndex];
            return;
        }
        currentCharacter = BattleData.instance.enemies[currentIndex];
    }
    void CreateMoveQueue()
    {
        moveQueue.Clear();
        if (isPlayerTurn)
        {
            for (int i = 0; i < BattleData.instance.party.Count; i++)
            {
                if (!BattleData.instance.party[i].CheckStatesByName("Death"))
                {
                    moveQueue.Add(i);
                }
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
        UIController.battleLog.AddLog($"{currentCharacter.Nickname} uses {selectedSkill.SkillName}.", Color.white);
        if(isPlayerTurn)
        {
            currentCharacter.ChangeHP(-selectedSkill.SkillCostPercentHP, true, BattleData.instance.statesList[0]);
        }
        currentCharacter.ChangeMP(-selectedSkill.SkillCostNumberMP);
        foreach(Character target in targets)
        {
            ChangeTurns(SkillActivation(currentCharacter, target));
        }
        UIController.UpdateHPInfo();
        gameState = GameState.END_TURN;
        isBusy = false;
        //EndTurn();
    }
    public ElementRelation SkillActivation(Character attacker, Character defender)
    {
        if (selectedSkill.SkillName == "Wait")
        {
            return ElementRelation.WEAK;
        }
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
        if (!isPlayerTurn)
        {
            ChooseRewardByRelation(relation);
        }
        if (isHP)
        {
            switch (relation)
            {
                case ElementRelation.WEAK:
                    damage = Mathf.CeilToInt(damage * 1.2f);
                    defender.ChangeHP(-damage, false, BattleData.instance.statesList[0]);
                    UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname} weak point. Deals {damage} damage.", Color.red);
                    break;
                case ElementRelation.STRONG:
                    damage = Mathf.CeilToInt(damage * 0.8f);
                    defender.ChangeHP(-damage, false, BattleData.instance.statesList[0]);
                    UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}'s strong point. Deals {damage} damage.", Color.blue);
                    break;
                case ElementRelation.NORMAL:
                    UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}. Deals {damage} damage.", Color.white);
                    defender.ChangeHP(-damage, false, BattleData.instance.statesList[0]);
                    break;
                case ElementRelation.ABSORB:
                    defender.ChangeHP(damage, false, BattleData.instance.statesList[0]);
                    UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}'s absorb point. Drains {damage} damage.", Color.green);
                    break;
                case ElementRelation.REPEL:
                    switch(attacker.Affinities[selectedSkill.Affinity])
                    {
                        case ElementRelation.NULL:
                            UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}'s repel point. {attacker.Nickname} nullifies repeled damage.", Color.magenta);
                            break;
                        case ElementRelation.ABSORB: 
                            attacker.ChangeHP(damage, false, BattleData.instance.statesList[0]);
                            UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}'s repel point. {attacker.Nickname} absorbs {damage} repeled damage.", Color.green);
                            break;
                        case ElementRelation.REPEL:
                            UIController.battleLog.AddLog($"{attacker.Nickname} attacks {defender.Nickname}'s repel point. {attacker.Nickname} have repel on this element so damage is nullified.", Color.magenta);
                            break;
                        default:
                            attacker.ChangeHP(-damage, false, BattleData.instance.statesList[0]);
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
        if(!isPlayerTurn)
        {
            enemyAI.positiveRewards += enemyAI.healReward;
        }
        if(isHP)
        {
            if (isPercent)
            {
                target.ChangeHP(heal, true, BattleData.instance.statesList[0]);
            }
            else
            {
                target.ChangeHP(heal, false, BattleData.instance.statesList[0]);
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
        if (!isPlayerTurn)
        {
            ChooseRewardByRelation(relation);
        }
        if (isHP)
        {
            switch (relation)
            {
                case ElementRelation.WEAK:
                    damage = Mathf.CeilToInt(damage * 1.2f);
                    defender.ChangeHP(-damage, false, BattleData.instance.statesList[0]);
                    attacker.ChangeHP(damage, false, BattleData.instance.statesList[0]);
                    break;
                case ElementRelation.STRONG:
                    damage = Mathf.CeilToInt(damage * 0.8f);
                    defender.ChangeHP(-damage, false, BattleData.instance.statesList[0]);
                    attacker.ChangeHP(damage, false, BattleData.instance.statesList[0]);
                    break;
                case ElementRelation.NORMAL:
                    defender.ChangeHP(-damage, false, BattleData.instance.statesList[0]);
                    attacker.ChangeHP(damage, false, BattleData.instance.statesList[0]);
                    break;
                case ElementRelation.ABSORB:
                    defender.ChangeHP(damage, false, BattleData.instance.statesList[0]);
                    break;
                case ElementRelation.REPEL:
                    switch (attacker.Affinities[selectedSkill.Affinity])
                    {
                        case ElementRelation.NULL:
                            break;
                        case ElementRelation.ABSORB:
                            attacker.ChangeHP(damage, false, BattleData.instance.statesList[0]);
                            break;
                        case ElementRelation.REPEL:
                            break;
                        default:
                            attacker.ChangeHP(-damage, false, BattleData.instance.statesList[0]);
                            defender.ChangeHP(damage, false, BattleData.instance.statesList[0]);
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

    //AI Specific
    public void AIActions()
    {
        enemyAI.RequestDecision();
        enemyAI.GetOutput();
        Academy.Instance.EnvironmentStep();

        //Use Skill
        selectedSkill = BattleData.instance.skillList[enemyAI.chosenSkill];
        //Debug.Log($"{enemyAI.chosenSkill}, {selectedSkill.SkillName}");
        UseSkill(AICreateTargetsFromSkill(BattleData.instance.skillList[enemyAI.chosenSkill]));
    }
    List<Character> AICreateTargetsFromSkill(Skill s)
    {
        switch(s.Scope)
        {
            case TargetChoice.ONE_ENEMY:
                return new List<Character> { BattleData.instance.party[enemyAI.chosenActor] };
            case TargetChoice.ALL_ENEMIES:
                return BattleData.instance.party;
            case TargetChoice.ONE_ALLY:
                return BattleData.instance.enemies;
            case TargetChoice.ALL_ALLIES:
                return BattleData.instance.enemies;
            case TargetChoice.USER:
                return BattleData.instance.enemies;
            case TargetChoice.ALL:
                List<Character> characters = new List<Character>();
                for (int i = 0; i < BattleData.instance.party.Count; i++)
                {
                    if (!BattleData.instance.party[i].CheckStatesByName("Death"))
                    {
                        characters.Add(BattleData.instance.party[i]);
                    }
                }
                for (int i = 0; i < BattleData.instance.enemies.Count; i++)
                {
                    if (!BattleData.instance.enemies[i].CheckStatesByName("Death"))
                    {
                        characters.Add(BattleData.instance.enemies[i]);
                    }
                }
                return characters;
            default:
                return new List<Character>();
        }
    }
    void ChooseRewardByRelation(ElementRelation relation)
    {
        switch (relation)
        {
            case ElementRelation.WEAK:
                enemyAI.positiveRewards += enemyAI.weakReward;
                break;
            case ElementRelation.STRONG:
                enemyAI.negativeRewards += enemyAI.strongReward;
                break;
            case ElementRelation.NORMAL:
                enemyAI.positiveRewards += enemyAI.neutralReward;
                break;
            case ElementRelation.ABSORB:
                enemyAI.negativeRewards += enemyAI.absorbReward;
                break;
            case ElementRelation.REPEL:
                enemyAI.negativeRewards += enemyAI.repelReward;
                break;
            case ElementRelation.NULL:
                enemyAI.negativeRewards += enemyAI.nullReward;
                break;
        }
    }

    public List<float> PrepareObservations()
    {
        List<float> observations = new List<float>();

        //Take Self Data
        GetCharacterData(ref observations, BattleData.instance.enemies[0]);

        //Take Enemies Data
        for (int i = 0; i < BattleData.instance.party.Count; i++)
        {
            GetCharacterData(ref observations, BattleData.instance.party[i]);
        }
        for (int i = BattleData.instance.party.Count; i < 4; i++)
        {
            GetClearData(ref observations);
        }

        //Take Turn Data
        int pressedTurns = 0;
        int normalTurns = 0;
        for (int i = 0; i < turnQueue.Count; i++)
        {
            if (turnQueue[i])
            {
                normalTurns++;
            }
            else
            {
                pressedTurns++;
            }
        }
        observations.Add(normalTurns / 4f);
        observations.Add(pressedTurns / 4f);

        return observations;
    }
    void GetCharacterData(ref List<float> observations, Character c)
    {
        observations.Add(c.MaxHPNormalized);
        observations.Add(c.CurrentHPNormalized);
        observations.Add(c.MaxMPNormalized);
        observations.Add(c.CurrentMPNormalized);
        observations.Add(c.StrengthNormalized);
        observations.Add(c.MagicNormalized);
        observations.Add(c.DexterityNormalized);
        observations.Add(c.AgilityNormalized);
        observations.Add(c.LuckNormalized);
        observations.Add(c.PhysicalAffinityNormalized);
        observations.Add(c.FireAffinityNormalized);
        observations.Add(c.IceAffinityNormalized);
        observations.Add(c.ElectricityAffinityNormalized);
        observations.Add(c.WindAffinityNormalized);
        observations.Add(c.LightAffinityNormalized);
        observations.Add(c.DarkAffinityNormalized);
        observations.Add(c.AlmightyAffinityNormalized);
    }
    void GetClearData(ref List<float> observations)
    {
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
        observations.Add(0f);
    }
    public void CheckMoveQueue()
    {
        for (int i = 0; i < moveQueue.Count; i++)
        {
            if (BattleData.instance.party[moveQueue[i]].CheckStatesByName("Death"))
            {
                moveQueue.RemoveAt(i);
            }
        }
    }
}