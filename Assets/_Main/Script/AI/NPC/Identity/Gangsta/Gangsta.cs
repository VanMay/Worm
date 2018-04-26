using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gangsta : MonoBehaviour {
    public Vision vision;
    private GameObject player;
    private List<GameObject> CivilianList;
    private List<GameObject> GuardList;
    private List<GameObject> GangstaList;
    private Memory<GameObject> memory;

    public List<GameObject> conflictionList;
    public List<string> message;

	void Start () {
        CivilianList = new List<GameObject>();
        GuardList = new List<GameObject>();
        GangstaList = new List<GameObject>();
	}

	void Update () {
		
	}

    void Cognition()
    {
        player = null;
        CivilianList.Clear();
        GuardList.Clear();
        GangstaList.Clear();
        for(int i = 0; i < vision.ObjectsInVision.Count; i++)
        {
            if(vision.ObjectsInVision[i].tag == Tags.Player)
            {
                player = vision.ObjectsInVision[i];
            }
            else if(vision.ObjectsInVision[i].tag == Tags.Civilian)
            {
                CivilianList.Add(vision.ObjectsInVision[i]);
            }
            else if(vision.ObjectsInVision[i].tag == Tags.Guard)
            {
                GuardList.Add(vision.ObjectsInVision[i]);
            }
            else if(vision.ObjectsInVision[i].tag == Tags.Gangsta)
            {
                GangstaList.Add(vision.ObjectsInVision[i]);
            }
        }
    }

    void React()
    {
        //对玩家反应
        if (player != null)
        {
            //玩家武器指向自己时冲突
            if (player.GetComponent<PlayerController>().isAiming)
            {
                if (Vector3.Dot(player.transform.forward, transform.position - player.transform.position) > 0)
                {
                    SetAsConfliction(player);
                }
            }
        }
        //对平民反应
        if (CivilianList.Count > 0)
        {
            //一定几率进行嘲讽
            GameObject tauntTarget = null;
            for(int i = 0; i < CivilianList.Count; i++)
            {
                if (!memory.Contains(CivilianList[i]))
                {
                    if (Random.value < 0.1f)
                    {
                        tauntTarget = CivilianList[i];
                        memory.Memorize(CivilianList[i], 30);
                    }
                }
            }
            if (tauntTarget != null)
            {
                //嘲讽
                Debug.Log(transform.name + " --> [Taunt] --> " + tauntTarget.name);
            }
        }
        //对守卫反应
        if(GuardList.Count > 0)
        {
            //停止冲突
            if (conflictionList.Count > 0)
            {
                conflictionList.Clear();
            }
        }
        //对暴徒反应
        if(GangstaList.Count > 0)
        {
            
        }
    }

    void Confliction()
    {
        if (conflictionList.Count > 0)
        {
            GameObject target = GetTarget();
            //进入冲突模式
        }
        else
        {
            //退出冲突模式
        }
    }

    GameObject GetTarget()
    {
        GameObject result = null;
        float minAngle = float.MaxValue;
        for (int i = 0; i < conflictionList.Count; i++)
        {
            if (minAngle > Vector3.Angle(transform.forward, conflictionList[i].transform.position - transform.position))
            {
                result = conflictionList[i];
            }
        }
        return result;
    }

    void SetAsConfliction(GameObject character)
    {
        if (!conflictionList.Contains(character))
        {
            conflictionList.Add(character);
        }
    }
}
