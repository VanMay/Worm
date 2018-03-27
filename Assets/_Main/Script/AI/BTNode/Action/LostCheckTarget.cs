using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Main/AI")]
public class LostCheckTarget : Action
{
    private AlertLevel alertLevel;

	public override void OnStart()
	{
        alertLevel = GetComponent<AlertLevel>();
	}

	public override TaskStatus OnUpdate()
	{
        alertLevel.hasCheckTarget = false;
		return TaskStatus.Success;
	}
}