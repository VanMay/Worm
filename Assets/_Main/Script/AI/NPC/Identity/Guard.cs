using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class Guard : Cognition {
    public Vision vision;

    private BehaviorTree behaviorTree;
    private AlertLevel alertLevel;
    private Animator anim;

	void Start () {
        CivilianList = new List<GameObject>();
        GuardList = new List<GameObject>();
        GangstaList = new List<GameObject>();
        memory = new Memory<GameObject>();
        conflictionList = new List<GameObject>();

        behaviorTree = GetComponent<BehaviorTree>();
        alertLevel = GetComponent<AlertLevel>();
        anim = GetComponent<Animator>();
    }

	void Update () {
        Cognition();
        React();
        Confliction();
        SendValueToAnimator();

        for(int i = conflictionList.Count - 1; i >= 0; i--)
        {
            if (!memory.Contains(conflictionList[i]))
            {
                conflictionList.RemoveAt(i);
            }
        }
    }

    //获取周围信息
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
            else if(alertLevel.state != AlertState.ConflictingState)
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
            alertLevel.SetAlertTarget(target);
        }
        else
        {
            //退出冲突模式
            behaviorTree.SetVariableValue("IsConflicting", false);
            alertLevel.SetAlertTarget(null);
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
        if (vision.ObjectsInVision.Contains(result))
        {
            return result;
        }
        else if (alertLevel.state == AlertState.ConflictingState
            && (result.transform.position - transform.position).magnitude < 8)
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
        memory.Memorize(character, 30);
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
        anim.SetFloat("AlertLevel", alertLevel.alertLevel);
    }
}
