using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Main/AI")]
public class HaveAlertTarget : Conditional
{
    private AlertLevel alertLevel;

    public override void OnStart()
    {
        alertLevel = GetComponent<AlertLevel>();
    }

    public override TaskStatus OnUpdate()
	{
        if (alertLevel.alertTarget)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}