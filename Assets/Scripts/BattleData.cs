using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    public static BattleData instance;
    public List<Skill> skillList;
    public List<State> statesList;
    public List<Character> party;
    public List<Character> enemies;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
            party = new List<Character>();
            enemies = new List<Character>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
