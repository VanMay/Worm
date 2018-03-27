using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Main/AI")]
public class FaceCheckTarget : Action
{
    public float rotateSpeed = 180;

    private AlertLevel alertLevel;

	public override void OnStart()
	{
        alertLevel = GetComponent<AlertLevel>();
	}

	public override TaskStatus OnUpdate()
	{
        Vector3 bodyDir = transform.forward;
        Vector3 targetBodyDir = Vector3.ProjectOnPlane(alertLevel.checkPosition - transform.position, Vector3.up);
        float body2MoveAngle = MathAdd.Angle_XZ_180(bodyDir, targetBodyDir);
        if (Mathf.Abs(body2MoveAngle) > rotateSpeed * Time.deltaTime)
        {
            transform.Rotate(Vector3.up, body2MoveAngle / Mathf.Abs(body2MoveAngle) * rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(targetBodyDir);
        }
        return TaskStatus.Success;
    }
}