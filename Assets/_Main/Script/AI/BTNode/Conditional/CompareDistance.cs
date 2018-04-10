using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Main/AI")]
[TaskDescription("Compare distance between self position and check position")]
public class CompareDistance : Conditional
{
    public enum Operation
    {
        LessThan,
        LessThanOrEqualTo,
        EqualTo,
        NotEqualTo,
        GreaterThanOrEqualTo,
        GreaterThan
    }

    public Operation operation;
    public float distance;
    

    private AlertLevel alertLevel;

    public override void OnStart()
    {
        alertLevel = GetComponent<AlertLevel>();
    }

    public override TaskStatus OnUpdate()
	{
        float currentDistance = (alertLevel.checkPosition - transform.position).magnitude;
        switch (operation)
        {
            case Operation.EqualTo:
                {
                    if (currentDistance == distance)
                    {
                        return TaskStatus.Success;
                    }
                    break;
                }
            case Operation.GreaterThan:
                {
                    if (currentDistance > distance)
                    {
                        return TaskStatus.Success;
                    }
                    break;
                }
            case Operation.GreaterThanOrEqualTo:
                {
                    if (currentDistance >= distance)
                    {
                        return TaskStatus.Success;
                    }
                    break;
                }
            case Operation.LessThan:
                {
                    if (currentDistance < distance)
                    {
                        return TaskStatus.Success;
                    }
                    break;
                }
            case Operation.LessThanOrEqualTo:
                {
                    if (currentDistance <= distance)
                    {
                        return TaskStatus.Success;
                    }
                    break;
                }
            case Operation.NotEqualTo:
                {
                    if (currentDistance != distance)
                    {
                        return TaskStatus.Success;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
        return TaskStatus.Failure;
	}
}