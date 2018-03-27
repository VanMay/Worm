using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

[TaskCategory("Main/AI")]
public class CheckAround : Action
{
    public float minRange;
    public float maxRange;

    private Vector3 nextCheckPos;
    private AlertLevel alertLevel;
    private NavMeshAgent navMeshAgent;

    public override void OnStart()
	{
        alertLevel = GetComponent<AlertLevel>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        GetNextCheckPosition();
    }

    void GetNextCheckPosition()
    {
        Vector3 randomDir = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f).normalized;
        float randomDist = Mathf.Lerp(minRange, maxRange, Random.value);
        nextCheckPos = transform.position + randomDir * randomDist;
        navMeshAgent.destination = nextCheckPos;
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(nextCheckPos, path);
        if (path.status == NavMeshPathStatus.PathPartial)
        {
            GetNextCheckPosition();
        }
    }

	public override TaskStatus OnUpdate()
	{
        alertLevel.checkPosition = nextCheckPos;
		return TaskStatus.Failure;
	}
}