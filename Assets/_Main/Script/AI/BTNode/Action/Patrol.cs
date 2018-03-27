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
    public List<Vector4> patrolRoute;
    private int nextPositionIndex = 0;
    private Vector3 nextPosition;
    private float idleTime;

    private NavMeshAgent navMeshAgent;
    private BehaviorTree behaviorTree;

	public override void OnStart()
	{
        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorTree = GetComponent<BehaviorTree>();

        nextPosition = patrolRoute[nextPositionIndex];
        idleTime = patrolRoute[nextPositionIndex].w;
        navMeshAgent.SetDestination(nextPosition);
    }

	public override TaskStatus OnUpdate()
	{
        if (navMeshAgent.remainingDistance < 0.1f)
        {
            nextPositionIndex = (nextPositionIndex + 1) % patrolRoute.Count;
            nextPosition = patrolRoute[nextPositionIndex];
            idleTime = patrolRoute[nextPositionIndex].w;
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