using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TrainingManager : MonoBehaviour
{
    public EnemyTrainer trainer;

    public List<Character> party = new List<Character>();
    public List<Character> enemies = new List<Character>();
    public Character currentCharacter;
    public Skill selectedSkill;

    public List<State> statesList;
    public List<Skill> skills;

    public List<int> moveQueue = new List<int>();
    public int currentIndex = 0;
    public List<bool> turnQueue = new List<bool>();
    public bool isPlayerTurn = true;

    public int pointsMin = 1;
    public int pointsMax = 9999;
    public int statMin = 1;
    public int statMax = 99;
    public int minActors = 1;
    public int maxActors = 4;

    public float startSideTime = 5f;
    public float startTurnTime = 5f;
    public bool isBusy = true;
    public GameState gameState;
    public int maxTurns = 400;
    public int turns = 0;

    public void PrepareData()
    {
        turns = 0;
        moveQueue = new List<int>();
        currentIndex = 0;
        turnQueue = new List<bool>();
        isPlayerTurn = true;
        int HP, MP, strength, magic, dexterity, agility, luck;
        
        party.Clear();
        //Randomize party
        int partyCount = Random.Range(minActors,maxActors+1);
        //Debug.Log($"Party Count - {partyCount}");

        for (int i = 0; i < partyCount; i++)
        {
            HP = Random.Range(pointsMin, pointsMax + 1);
            MP = Random.Range(pointsMin, pointsMax + 1);
            strength = Random.Range(statMin, statMax + 1);
            magic = Random.Range(statMin, statMax + 1);
            dexterity = Random.Range(statMin, statMax + 1);
            agility = Random.Range(statMin, statMax + 1);
            luck = Random.Range(statMin, statMax + 1);
            //Debug.Log($"Party Member {i}: {HP}, {MP}, {strength}, {magic}, {dexterity}, {agility}, {luck}");
            Character character = new Character(i.ToString(), HP, HP, MP, MP, strength, magic, dexterity, agility, luck, null, RandomizeSkills(), RandomizeAffinities());
            party.Add(character);
        }
        enemies.Clear();
        //Randomize enemy
        HP = Random.Range(pointsMin, pointsMax + 1);
        MP = Random.Range(pointsMin, pointsMax + 1);
        strength = Random.Range(statMin, statMax + 1);
        magic = Random.Range(statMin, statMax + 1);
        dexterity = Random.Range(statMin, statMax + 1);
        agility = Random.Range(statMin, statMax + 1);
        luck = Random.Range(statMin, statMax + 1);
        //Debug.Log($"Enemy: {HP}, {MP}, {strength}, {magic}, {dexterity}, {agility}, {luck}");
        Character c = new Character("Enemy", HP, HP, MP, MP, strength, magic, dexterity, agility, luck, null, RandomizeSkills(), RandomizeAffinities());
        enemies.Add(c);
    }

    void Start()
    {
        ResetEnvironment();
    }
    private void Update()
    {
        if(isBusy)
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
    List<Skill> RandomizeSkills()
    {
        string log = "";
        List<Skill> list = new List<Skill>();
        list.Add(skills[0]);
        log += skills[0].SkillName + ", ";
        list.Add(skills[1]);
        log += skills[1].SkillName + ", ";
        for (int i = 2; i < skills.Count; i++)
        {
            if(Random.Range(0,2)>0)
            {
                list.Add(skills[i]);
                log += skills[i].SkillName+", ";
            }
        }
        Debug.Log(log);
        return list;
    }

    Dictionary<Element, ElementRelation> RandomizeAffinities()
    {
        //string log = "";
        Dictionary<Element, ElementRelation> dict = new Dictionary<Element, ElementRelation>();
        dict.Add(Element.NONE, ElementRelation.NORMAL);
        for (int i = 1; i < 8; i++)
        {
            dict.Add((Element)i,(ElementRelation)Random.Range(0,6));
            //log += $"{((Element)i).ToString()} - {((ElementRelation)dict[(Element)i]).ToString()}\n";
        }
        dict.Add(Element.ALMIGHTY, ElementRelation.NORMAL);
        //Debug.Log(log);
        return dict;
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
        turns++;
        SetCurrentCharacter();
        if (isPlayerTurn)
        {
            //RandomSkill
            UseRandomSkill();
        }
        else
        {
            //AILearn
            AIMove();
        }
        //Academy.Instance.EnvironmentStep();

        gameState = GameState.END_TURN;
        isBusy = false;
        //EndTurn();
    }
    bool CheckWin()
    {
        foreach (Character c in enemies)
        {
            if (c.CurrentHP > 0)
            {
                return false;
            }
        }
        //Debug.Log("Victory");
        return true;
    }
    bool CheckLose()
    {
        foreach (Character c in party)
        {
            if (c.CurrentHP > 0)
            {
                return false;
            }
        }
        //Debug.Log("Lose");
        return true;
    }
    void EndTurn()
    {
        if (CheckLose() || CheckWin() || turns > maxTurns)
        {
            trainer.NotifyEndEpisode();
            return;
        }
        if (turnQueue.Count < 1)
        {
            gameState = GameState.END_SIDE;
            isBusy = false;
            //EndSide();
            return;
        }
        currentIndex = moveQueue[0];
        moveQueue.RemoveAt(0);
        moveQueue.Add(currentIndex);
        if(isPlayerTurn)
        {
            CheckMoveQueue();
        }
        gameState = GameState.START_TURN;
        isBusy = false;
        //Invoke("StartTurn", startTurnTime);
        //startButton.gameObject.SetActive(true);
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
            currentCharacter = party[moveQueue[0]];
            return;
        }
        currentCharacter = enemies[moveQueue[0]];
    }
    void CreateMoveQueue()
    {
        moveQueue.Clear();
        if (isPlayerTurn)
        {
            for (int i = 0; i < party.Count; i++)
            {
                if (!party[i].CheckStatesByName("Death"))
                {
                    moveQueue.Add(i);
                }  
            }
            for (int i = 0; i < moveQueue.Count - 1; i++)
            {
                for (int j = i + 1; j < moveQueue.Count; j++)
                {
                    if (party[moveQueue[i]].Agility < party[moveQueue[j]].Agility)
                    {
                        int temp = moveQueue[i];
                        moveQueue[i] = moveQueue[j];
                        moveQueue[j] = temp;
                    }
                }
            }
            return;
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            moveQueue.Add(i);
        }
        for (int i = 0; i < moveQueue.Count - 1; i++)
        {
            for (int j = i + 1; j < moveQueue.Count; j++)
            {
                if (enemies[moveQueue[i]].Agility > enemies[moveQueue[j]].Agility)
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
            for (int i = 0; i < party.Count; i++)
            {
                if (!party[i].States.Contains(statesList[0]))
                {
                    turnQueue.Add(true);
                }
            }
            return;
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].States.Contains(statesList[0]))
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
    public void UseSkill(List<Character> targets)
    {
        //Debug.Log($"{currentCharacter.Nickname} - {selectedSkill.SkillName}");
        if (isPlayerTurn)
        {
            currentCharacter.ChangeHP(-selectedSkill.SkillCostPercentHP, true, statesList[0]);
        }
        currentCharacter.ChangeMP(-selectedSkill.SkillCostNumberMP);
        foreach (Character target in targets)
        {
            ChangeTurns(SkillActivation(currentCharacter, target));
        }
    }
    public ElementRelation SkillActivation(Character attacker, Character defender)
    {
        if(selectedSkill.SkillName == "Wait")
        {
            return ElementRelation.NORMAL;
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
                    defender.ChangeHP(-damage, false, statesList[0]);
                    break;
                case ElementRelation.STRONG:
                    damage = Mathf.CeilToInt(damage * 0.8f);
                    defender.ChangeHP(-damage, false, statesList[0]);
                    break;
                case ElementRelation.NORMAL:
                    defender.ChangeHP(-damage, false, statesList[0]);
                    break;
                case ElementRelation.ABSORB:
                    defender.ChangeHP(damage, false, statesList[0]);
                    break;
                case ElementRelation.REPEL:
                    switch (attacker.Affinities[selectedSkill.Affinity])
                    {
                        case ElementRelation.NULL:
                            break;
                        case ElementRelation.ABSORB:
                            attacker.ChangeHP(damage, false, statesList[0]);
                            break;
                        case ElementRelation.REPEL:
                            break;
                        default:
                            attacker.ChangeHP(-damage, false, statesList[0]);
                            break;
                    }
                    break;
                case ElementRelation.NULL:
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
        if (!isPlayerTurn)
        {
            trainer.positiveRewards += trainer.healReward;
        }
        if (isHP)
        {
            if (isPercent)
            {
                target.ChangeHP(heal, true, statesList[0]);
            }
            else
            {
                target.ChangeHP(heal, false, statesList[0]);
            }
        }
        else
        {
            if (isPercent)
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
                    defender.ChangeHP(-damage, false, statesList[0]);
                    attacker.ChangeHP(damage, false, statesList[0]);
                    break;
                case ElementRelation.STRONG:
                    damage = Mathf.CeilToInt(damage * 0.8f);
                    defender.ChangeHP(-damage, false, statesList[0]);
                    attacker.ChangeHP(damage, false, statesList[0]);
                    break;
                case ElementRelation.NORMAL:
                    defender.ChangeHP(-damage, false, statesList[0]);
                    attacker.ChangeHP(damage, false, statesList[0]);
                    break;
                case ElementRelation.ABSORB:
                    defender.ChangeHP(damage, false, statesList[0]);
                    break;
                case ElementRelation.REPEL:
                    switch (attacker.Affinities[selectedSkill.Affinity])
                    {
                        case ElementRelation.NULL:
                            break;
                        case ElementRelation.ABSORB:
                            attacker.ChangeHP(damage, false, statesList[0]);
                            break;
                        case ElementRelation.REPEL:
                            break;
                        default:
                            attacker.ChangeHP(-damage, false, statesList[0]);
                            defender.ChangeHP(damage, false, statesList[0]);
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
    void ChooseRewardByRelation(ElementRelation relation)
    {
        switch (relation)
        {
            case ElementRelation.WEAK:
                trainer.positiveRewards += trainer.weakReward;
                break;
            case ElementRelation.STRONG:
                trainer.negativeRewards += trainer.strongReward;
                break;
            case ElementRelation.NORMAL:
                trainer.positiveRewards += trainer.neutralReward;
                break;
            case ElementRelation.ABSORB:
                trainer.negativeRewards += trainer.absorbReward;
                break;
            case ElementRelation.REPEL:
                trainer.negativeRewards += trainer.repelReward;
                break;
            case ElementRelation.NULL:
                trainer.negativeRewards += trainer.nullReward;
                break;
        }
    }
    void UseRandomSkill()
    {
        int skillRand;
        do
        {
            skillRand = Random.Range(0, currentCharacter.Skills.Count);
        } while (!(currentCharacter.Skills[skillRand].CharacterHaveHP(currentCharacter) && currentCharacter.Skills[skillRand].CharacterHaveMP(currentCharacter)));
        selectedSkill = currentCharacter.Skills[skillRand];
        List<Character> targets;
        //Only works for one enemy and 1-4 allies, not for multiple enemies
        switch (selectedSkill.Scope)
        {
            case TargetChoice.ONE_ENEMY:
                targets = enemies;
                break;
            case TargetChoice.ALL_ENEMIES:
                targets = enemies;
                break;
            case TargetChoice.ONE_ALLY:
                targets = new List<Character> { party[RandomizeParty()] };
                break;
            case TargetChoice.ALL_ALLIES:
                targets = enemies;
                break;
            case TargetChoice.USER:
                targets = new List<Character> { currentCharacter };
                break;
            case TargetChoice.ALL:
                targets = new List<Character>();
                for (int i = 0; i < party.Count; i++)
                {
                    if (!party[i].CheckStatesByName("Death"))
                    {
                        targets.Add(party[i]);
                    }
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (!enemies[i].CheckStatesByName("Death"))
                    {
                        targets.Add(enemies[i]);
                    }
                }
                break;
            default:
                targets = new List<Character>();
                break;
        }
        UseSkill(targets);
    }
    int RandomizeParty()
    {
        int rand;
        do
        {
            rand = Random.Range(0, party.Count);
        } while (party[rand].CheckStatesByName("Death"));
        return rand;
    }
    void AIMove()
    {
        trainer.RequestDecision();
        trainer.GetOutput();
        Academy.Instance.EnvironmentStep();

        //Use Skill
        selectedSkill = skills[trainer.chosenSkill];
        //Debug.Log($"AIMove - {trainer.chosenSkill}, {selectedSkill.SkillName}");
        UseSkill(AICreateTargetsFromSkill(skills[trainer.chosenSkill]));
    }
    List<Character> AICreateTargetsFromSkill(Skill s)
    {
        switch (s.Scope)
        {
            case TargetChoice.ONE_ENEMY:
                return new List<Character> { party[trainer.chosenActor] };
            case TargetChoice.ALL_ENEMIES:
                return party;
            case TargetChoice.ONE_ALLY:
                return enemies;
            case TargetChoice.ALL_ALLIES:
                return enemies;
            case TargetChoice.USER:
                return enemies;
            case TargetChoice.ALL:
                List<Character> characters = new List<Character>();
                for (int i = 0; i < party.Count; i++)
                {
                    if (!party[i].CheckStatesByName("Death"))
                    {
                        characters.Add(party[i]);
                    }
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (!enemies[i].CheckStatesByName("Death"))
                    {
                        characters.Add(enemies[i]);
                    }
                }
                return characters;
            default:
                return new List<Character>();
        }
    }

    public List<float> PrepareObservations()
    {
        List<float> observations = new List<float>();

        //Take Self Data
        GetCharacterData(ref observations, enemies[0]);

        //Take Enemies Data
        for (int i = 0; i < party.Count; i++)
        {
            GetCharacterData(ref observations, party[i]);
        }
        for (int i = party.Count; i < 4; i++)
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
    public List<int> PrepareDataForAI()
    {
        List<int> data = new List<int>();
        for (int i = 0; i < skills.Count; i++)
        {
            if (enemies[0].Skills.Contains(skills[i]) && skills[i].CharacterHaveMP(enemies[0]))
            {
                data.Add(i);
            }
        }
        return data;
    }
    public void ResetEnvironment()
    {
        PrepareData();
        gameState = GameState.START_SIDE;
        isBusy = false;
        //StartSide();
    }
    public void ResetEnvBtn()
    {
        trainer.NotifyEndEpisode();
    }
    public void CheckMoveQueue()
    {
        for (int i = 0; i < moveQueue.Count; i++)
        {
            if (party[moveQueue[i]].CheckStatesByName("Death"))
            {
                moveQueue.RemoveAt(i);
            }
        }
    }
}
