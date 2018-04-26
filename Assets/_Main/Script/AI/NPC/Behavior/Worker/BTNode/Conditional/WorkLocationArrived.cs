using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeWorker
{
    [TaskCategory("Main/AI/NPC/Worker")]
    public class WorkLocationArrived : Conditional
    {
        public float distance = 0.5f;
        Worker worker;

        public override void OnStart()
        {
            worker = GetComponent<Worker>();
        }

        public override TaskStatus OnUpdate()
        {
            if ((worker.workLocation.transform.position - transform.position).magnitude < distance)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
    }
}
