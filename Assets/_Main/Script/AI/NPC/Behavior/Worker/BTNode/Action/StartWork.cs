using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeWorker
{
    [TaskCategory("Main/AI/NPC/Worker")]
    public class StartWork : Action
    {
        Worker worker;

        public override void OnStart()
        {
            worker = GetComponent<Worker>();
        }

        public override TaskStatus OnUpdate()
        {
            worker.StartWork();
            return TaskStatus.Success;
        }
    }
}
