using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

public class Station : MonoBehaviour {
    public GameObject stationLocation;
    public StationPoint stationPoint;
    public string stationAnimParameter;
    public float stationStartTime;
    public float stationTime;
    [SerializeField]
    private float detectRadius = 3;
    [SerializeField]
    private Memory<StationPoint> memory;
    [SerializeField]
    private float memoryDuration = 20;

    private BehaviorTree behaviorTree;

	void Start () {
        memory = new Memory<StationPoint>();
        behaviorTree = GetComponent<BehaviorTree>();
	}

	void Update () {
		if(stationPoint == null)
        {
            stationPoint = GetStationPoint();
            if (stationPoint != null)
            {
                stationLocation = stationPoint.EnterStation(gameObject);
            }
        }
        else
        {
            MemorizeSurrounding();
        }
        //向行为树发送数据
        SendValueToBehaviorTree();
	}

    /// <summary>
    /// 记住周围环境的StationPoint
    /// </summary>
    public void MemorizeSurrounding()
    {
        List<StationPoint> tempList = new List<StationPoint>();
        List<float> durationList = new List<float>();
        Collider[] objAround = Physics.OverlapSphere(transform.position, detectRadius, 1 << LayerMask.NameToLayer(Layers.AI));
        for (int i = 0; i < objAround.Length; i++)
        {
            StationPoint tempStationPoint = objAround[i].GetComponent<StationPoint>();
            if (tempStationPoint != null)
            {
                tempList.Add(tempStationPoint);
                durationList.Add(memoryDuration);
            }
        }
        memory.KeepInMind(tempList.ToArray(), durationList.ToArray());
    }

    /// <summary>
    /// 进入驻点状态
    /// </summary>
    /// <returns>前往的驻点目标</returns>
    public StationPoint GetStationPoint()
    {
        List<StationPoint> tempList = new List<StationPoint>();
        Collider[] objAround = Physics.OverlapSphere(transform.position, detectRadius, 1 << LayerMask.NameToLayer(Layers.AI));
        for(int i = 0; i < objAround.Length; i++)
        {
            StationPoint tempStationPoint = objAround[i].GetComponent<StationPoint>();
            if (tempStationPoint != null)
            {//筛选出StationPoint
                if(Vector3.Dot(tempStationPoint.transform.position - transform.position, transform.forward) > 0)
                {//筛选出位于角色前方的StationPoint
                    if (!memory.Contains(tempStationPoint))
                    {//记忆中不存在该StationPoint
                        memory.Memorize(tempStationPoint, memoryDuration);
                        if (tempStationPoint.HasEmpty)
                        {//筛选出有空位的StationPoint
                            tempList.Add(tempStationPoint);
                        }
                    }
                }
            }
        }
        if(tempList.Count == 0)
        {
            return null;
        }
        StationPoint target = tempList[Random.Range(0, tempList.Count)];
        if(TurnToStation())
        {//有一定几率转为Station
            return target;
        }
        return null;
    }

    //根据概率是否转为Station（可用于控制行人密度）
    private bool TurnToStation()
    {
        return Random.value > 0f;
    }

    /// <summary>
    /// 离开驻点状态
    /// </summary>
    public void LeaveStationPoint()
    {
        stationPoint.ExitStation(gameObject);
        stationLocation = null;
        stationPoint = null;
        stationAnimParameter = "";
        stationTime = 0;
    }

    /// <summary>
    /// 到达驻点位置
    /// </summary>
    public void ArriveStationPoint()
    {
        stationPoint.ArriveStation(gameObject);
    }

    void SendValueToBehaviorTree()
    {
        behaviorTree.SetVariableValue("StationLocation", stationLocation);
    }
}
