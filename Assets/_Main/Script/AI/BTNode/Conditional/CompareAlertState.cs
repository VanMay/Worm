using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Main/AI")]
public class CompareAlertState : Conditional
{
    public AlertState state;
    private AlertLevel alertLevel;

    public override void OnStart()
    {
        alertLevel = GetComponent<AlertLevel>();
    }

    public override TaskStatus OnUpdate()
	{
        if (alertLevel.state == state)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
	}
}