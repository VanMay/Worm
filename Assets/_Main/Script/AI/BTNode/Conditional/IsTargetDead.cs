using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Main/AI")]
public class IsTargetDead : Conditional
{
	public override TaskStatus OnUpdate()
	{
        GameObject target = GetComponent<AlertLevel>().alertTarget;
        if(target != null)
        {
            Life life = target.GetComponent<Life>();
            if (life != null && life.HP <= 0)
            {
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Failure;
	}
}