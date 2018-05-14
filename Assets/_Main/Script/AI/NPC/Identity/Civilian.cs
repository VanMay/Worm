using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class Civilian : Cognition {
    public float detectRange = 10;
    private List<GameObject> CharacterList;

    private BehaviorTree behaviorTree;
    private Animator anim;

	void Start () {
        CivilianList = new List<GameObject>();
        GuardList = new List<GameObject>();
        GangstaList = new List<GameObject>();
        memory = new Memory<GameObject>();
        conflictionList = new List<GameObject>();

        CharacterList = new List<GameObject>();

        behaviorTree = GetComponent<BehaviorTree>();
	}

	void Update () {
        Cognition();
        React();
        Confliction();
	}

    //获取周围信息
    void Cognition()
    {
        player = null;
        CivilianList.Clear();
        GuardList.Clear();
        GangstaList.Clear();
        CharacterList.Clear();
        foreach(Collider character in Physics.OverlapSphere(transform.position, detectRange, 1 << LayerMask.NameToLayer(Layers.Character)))
        {
            CharacterList.Add(character.gameObject);
        }
        for(int i = 0; i < CharacterList.Count; i++)
        {
            if(CharacterList[i].tag == Tags.Player)
            {
                player = CharacterList[i];
            }
            else if(CharacterList[i].tag == Tags.Civilian)
            {
                CivilianList.Add(CharacterList[i]);
            }
            else if(CharacterList[i].tag == Tags.Guard)
            {
                GuardList.Add(CharacterList[i]);
            }
            else if(CharacterList[i].tag == Tags.Gangsta)
            {
                GangstaList.Add(CharacterList[i]);
            }
        }
    }

    //对周围信息作出反应
    //中枢
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
            else
            {
                RemoveConfliction(player);
            }
        }
        //对平民反应
        if (CivilianList.Count > 0)
        {

        }
        //对守卫反应
        if(GuardList.Count > 0)
        {

        }
        //对暴徒反应
        if(GangstaList.Count > 0)
        {
            for(int i = 0; i < GangstaList.Count; i++)
            {
                if(GangstaList[i].GetComponent<Cognition>().conflictionList.Count > 0)
                {
                    SetAsConfliction(GangstaList[i]);
                }
                else
                {
                    RemoveConfliction(GangstaList[i]);
                }
            }
        }
    }

    //控制是否进入冲突
    void Confliction()
    {
        if (conflictionList.Count > 0)
        {
            GameObject target = GetTarget();
            //进入冲突模式
            behaviorTree.SetVariableValue("IsConflicting", true);
        }
        else
        {
            //退出冲突模式
            behaviorTree.SetVariableValue("IsConflicting", false);
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
        if (CharacterList.Contains(result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }

    void SetAsConfliction(GameObject character)
    {
        if (!conflictionList.Contains(character))
        {
            conflictionList.Add(character);
        }
    }

    void RemoveConfliction(GameObject character)
    {
        if (conflictionList.Contains(character))
        {
            conflictionList.Remove(character);
        }
    }

    void SendValueToAnimator()
    {

    }
}
