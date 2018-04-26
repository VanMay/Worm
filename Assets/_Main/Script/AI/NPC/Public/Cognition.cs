using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using Main.AI.Public;

public enum CognitionState
{
    DefaultState,
    StressState,
    AlertState,
    ReactState
}

public class Cognition : MonoBehaviour {
    [Header("Result")]
    [Range(0, 1)]
    public float pressure;
    public float pressureIncreaseSpeed = 1;
    public float pressureDecreaseSpeed = 1;
    //认知状态
    public CognitionState state;
    public float stressStateThreshold = 0.1f;
    public float warnStateThreshold = 0.5f;
    public float reactStateThreshold = 0.9f;
    //认知对象
    [Header("Target")]
    public GameObject Target;//当前目标
    public bool HaveCheckPosition = false;//是否已检查过目标
    public Vector3 CheckPosition;//检查的目标位置
    //认知检测
    [Header("Detector")]
    public Vision vision;
    public List<GameObject> conflictionList;
    public Memory<GameObject> enemyMemory;
    public Memory<GameObject> conflictionMemory;

    private Animator anim;
    private BehaviorTree behaviorTree;

    void Start () {
        conflictionList = new List<GameObject>();
        enemyMemory = new Memory<GameObject>();
        conflictionMemory = new Memory<GameObject>();
        anim = GetComponent<Animator>();
        behaviorTree = GetComponent<BehaviorTree>();
    }

	void Update () {
        UpdateCognitionDetection();
        UpdatePressure();
        UpdateCognitionState();

        SendValueToAnimator();
        SendValueToBehaviorTree();
	}

    /* 更新认知检测 */
    void UpdateCognitionDetection()
    {
        List<GameObject> enemyList = vision.EnemyList;
        if (enemyList.Count == 0)
        {
            Target = null;
            return;
        }
        //移除视域外的冲突对象
        for(int i = conflictionList.Count - 1; i >= 0; i--)
        {
            if (!enemyList.Contains(conflictionList[i]))
            {
                conflictionList.RemoveAt(i);
            }
        }
        //添加视域内的冲突对象
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (!enemyMemory.Contains(enemyList[i]))
            {//若首次遇见该敌人，则按照一定概率将其加入当前冲突对象中
                if (TurnEnemyToConfliction())
                {
                    conflictionList.Add(enemyList[i]);
                    conflictionMemory.Memorize(enemyList[i], 30);
                }
            }
            else if (!conflictionList.Contains(enemyList[i]))
            {//若再次遇见记忆中的冲突对象，则将其再次加入当前冲突对象中
                if (conflictionMemory.Contains(enemyList[i]))
                {
                    conflictionList.Add(enemyList[i]);
                    conflictionMemory.Memorize(enemyList[i], 30);
                }
            }
            enemyMemory.Memorize(enemyList[i], 30);
        }

        if (Target == null)
        {
            Target = GetTarget();
        }
    }

    //根据概率将敌人加入冲突列表
    bool TurnEnemyToConfliction()
    {
        return Random.value <= 1f;
    }

    //从冲突列表中选择对象
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

    /* 更新认知对象压力 */
    void UpdatePressure()
    {
        if (Target != null)
        {
            for (int i = 0; i < conflictionList.Count; i++)
            {
                float distance = (conflictionList[i].transform.position - transform.position).magnitude;
                float factor = Mathf.Lerp(1, 0, distance / vision.visionRange);
                pressure += factor * Time.deltaTime;
            }
            HaveCheckPosition = true;
            CheckPosition = Target.transform.position;
        }
        else
        {
            if(state == CognitionState.ReactState)
            {
                if (!HaveCheckPosition)
                {
                    pressure -= pressureDecreaseSpeed * Time.deltaTime;
                }
            }
            else
            {
                pressure -= pressureDecreaseSpeed * Time.deltaTime;
            }
        }
        pressure = Mathf.Clamp01(pressure);
    }

    /* 根据压力更新认知状态 */
    void UpdateCognitionState()
    {
        if (pressure >= reactStateThreshold)
        {
            state = CognitionState.ReactState;
        }
        else if (pressure >= warnStateThreshold)
        {
            state = CognitionState.AlertState;
        }
        else if (pressure >= stressStateThreshold)
        {
            state = CognitionState.StressState;
        }
        else
        {
            state = CognitionState.DefaultState;
        }
    }

    void SendValueToAnimator()
    {
        anim.SetBool("HaveTarget", Target != null);
    }

    void SendValueToBehaviorTree()
    {
        behaviorTree.SetVariableValue("Target", Target);
        behaviorTree.SetVariableValue("HaveCheckPosition", HaveCheckPosition);
        behaviorTree.SetVariableValue("CheckPosition", CheckPosition);
    }
}
