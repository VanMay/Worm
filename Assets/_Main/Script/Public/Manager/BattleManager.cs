using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {
    public static BattleManager instance;

    public TextAsset enemyFile;
    private Dictionary<string, List<string>> enemyList;

    public TextAsset allyFile;
    private Dictionary<string, List<string>> allyList;

    public bool playerInBattle;
    public List<AlertLevel> playerEnemyList;

    void Awake()
    {
        instance = this;
    }

	void Start () {
        GetRelationship();
	}

    void Update()
    {
        playerInBattle = playerEnemyList.Count > 0;
    }

    void GetRelationship()
    {
        enemyList = DataReader.JsonToObject<Dictionary<string, List<string>>>(enemyFile.text);
        allyList = DataReader.JsonToObject<Dictionary<string, List<string>>>(allyFile.text);
    }

    public bool IsEnemy(string selfTag, string targetTag)
    {
        for (int i = 0; i < enemyList[selfTag].Count; i++)
        {
            if (enemyList[selfTag][i] == targetTag)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsAlly(string selfTag, string targetTag)
    {
        for (int i = 0; i < allyList[selfTag].Count; i++)
        {
            if (allyList[selfTag][i] == targetTag)
            {
                return true;
            }
        }
        return false;
    }
}
