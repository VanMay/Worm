using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeWalker
{
    [TaskCategory("Main/AI/NPC/Walker")]
    public class WalkPointArrived : Conditional
    {
        public float distance = 2;
        Walker walker;

        public override void OnStart()
        {
            walker = GetComponent<Walker>();
        }

        public override TaskStatus OnUpdate()
        {
            if ((walker.walkPoint.transform.position - transform.position).magnitude < distance)
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
