using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

[TaskCategory("Main/AI")]
public class InAttackRange : Conditional
{
    public float attackAngle = 30;
    public float attackRange = 1;

    private AlertLevel alertLevel;
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    public override void OnStart()
    {
        alertLevel = GetComponent<AlertLevel>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    public override TaskStatus OnUpdate()
	{
        if (alertLevel.alertTarget)
        {
            Vector3 dir = alertLevel.alertTarget.transform.position - transform.position;
            float distance = dir.magnitude;
            if (distance <= attackRange)
            {
                float angle = Vector3.Angle(transform.forward, dir);
                if (angle <= attackAngle / 2)
                {
                    return TaskStatus.Success;
                }
            }
        }
		return TaskStatus.Failure;
	}
}