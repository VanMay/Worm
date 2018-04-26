using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeWalker
{
    [TaskCategory("Main/AI/NPC/Walker")]
    public class NextWalkPoint : Action
    {
        Walker walker;

        public override void OnStart()
        {
            walker = GetComponent<Walker>();
        }

        public override TaskStatus OnUpdate()
        {
            walker.NextWalkPoint();
            return TaskStatus.Success;
        }
    }
}
