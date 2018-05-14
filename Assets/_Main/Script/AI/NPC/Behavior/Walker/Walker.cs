using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walker : MonoBehaviour {
    public WalkerGenerator walkerGenerator;
    public WalkPoint walkPoint;
    private WalkPoint lastWalkPoint;

    private Animator anim;
    private NavMeshAgent agent;

	void Start () {
        Init();
    }

    public void Init()
    {
        walkPoint = walkerGenerator.walkPointList[Random.Range(0, walkerGenerator.walkPointList.Count)];
        //随机动画初始状态
        anim = GetComponent<Animator>();
        anim.SetFloat("AnimCycleOffset", Random.value);
        agent = GetComponent<NavMeshAgent>();
    }

	void Update () {
        float speed = Mathf.Clamp01(agent.velocity.magnitude / agent.speed);
        anim.SetFloat("Speed", speed);
    }

    /// <summary>
    /// 下一个移动点
    /// </summary>
    public void NextWalkPoint()
    {
        if (walkPoint.connectedWalkPoints.Count > 1)
        {//有其他路径选择时，不走回头路
            int index = Random.Range(0, walkPoint.connectedWalkPoints.Count);
            while(lastWalkPoint == walkPoint.connectedWalkPoints[index])
            {
                index = Random.Range(0, walkPoint.connectedWalkPoints.Count);
            }
            lastWalkPoint = walkPoint;
            walkPoint = walkPoint.connectedWalkPoints[index];
        }
        else
        {//无其他路径选择时，只能走回头路
            lastWalkPoint = walkPoint;
            walkPoint = walkPoint.connectedWalkPoints[0];
        }
    }
}
