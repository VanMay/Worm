using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Main/AI")]
public class Patrol : Action
{
    /// <summary>
    /// x,y,z: 路径节点坐标， w: 路径节点停留时间
    /// </summary>
    private Vector3 nextPosition;
    private float idleTime;

    private NavMeshAgent navMeshAgent;
    private BehaviorTree behaviorTree;
    private PatrolInfo patrolInfo;

	public override void OnStart()
	{
        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorTree = GetComponent<BehaviorTree>();
        patrolInfo = GetComponent<PatrolInfo>();

        nextPosition = patrolInfo.GetPatrolInfo();
        idleTime = patrolInfo.GetPatrolInfo().w;
        navMeshAgent.SetDestination(nextPosition);
    }

	public override TaskStatus OnUpdate()
	{
        if (navMeshAgent.remainingDistance < 0.1f)
        {
            patrolInfo.Next();
            nextPosition = patrolInfo.GetPatrolInfo();
            idleTime = patrolInfo.GetPatrolInfo().w;
            navMeshAgent.SetDestination(nextPosition);
            if (idleTime != 0)
            {
                behaviorTree.SetVariableValue("IdleTime", idleTime);
                return TaskStatus.Failure;
            }
        }
        return TaskStatus.Success;
	}
}