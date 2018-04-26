using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeWorker
{
    [TaskCategory("Main/AI/NPC/Worker")]
    public class EndWork : Action
    {
        Worker worker;

        public override void OnStart()
        {
            worker = GetComponent<Worker>();
        }

        public override TaskStatus OnUpdate()
        {
            worker.EndWork();
            worker.GetWork();
            return TaskStatus.Success;
        }
    }
}
